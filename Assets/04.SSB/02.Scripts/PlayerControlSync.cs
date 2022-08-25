//네트워크 라이브러리 추가
using Photon.Pun;
using UnityEngine;


public class PlayerControlSync : MonoBehaviourPun, IPunObservable   //객체의 싱크를 맞추는 인터페이스 //MonoBehaviour
{
    Animator anim;

    bool isLive = true;
    bool isWin = false;
    public Rigidbody rb;
    public float speed = 5f;
    //float rotSpeed = 180.0f;
    //float movSpeed = 3.0f;

    int spwanlimit = 1;

    public Vector3 setPos;
    public Quaternion setRot;

    //포톤뷰랑 폭탄 연동
    public PhotonView PV;
    public GameObject[] bombFactory;
    public GameObject bomb;

    [SerializeField]
     GameObject loseUI;
    [SerializeField]
    GameObject winUI;
    [SerializeField]
    GameObject[] findPlayer;

    bool isLose = false;

    void Start()
    {
        //Vector2 initPos = Random.insideUnitCircle * 1.5f;
        //if (spwanlimit < 2)
        //{
        //    PhotonNetwork.Instantiate("Player", new Vector3(initPos.x, 1, initPos.y), Quaternion.identity);
        //    spwanlimit--;
        //}
        PV = GetComponent<PhotonView>();
        anim = GetComponentInChildren<Animator>();

        
        loseUI = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        winUI = GameObject.Find("Canvas").transform.GetChild(1).gameObject;

        //int actorNumber = (int)PV.InstantiationData[0];
        //string myName = (string)PV.InstantiationData[1];
    }

    // Update is called once per frame
    void Update()
    {
        MoveRot();
        findPlayer = GameObject.FindGameObjectsWithTag("Player");
        print(PhotonNetwork.CurrentRoom.PlayerCount);
        print(isWin);

        Invoke("OnWinUI", 10f);

        
    }

    private void MoveRot()
    {
        if (photonView.IsMine == true)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = Vector3.right * h + Vector3.forward * v;
            dir.y = 0; //y좌표값 막아버리기

            //dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0;
            dir.Normalize();

            if(isLive == true)
            {
                anim.SetFloat("h", h);
                anim.SetFloat("v", v);
                rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //PhotonNetwork.Instantiate("EarthBomb", transform.position, Quaternion.identity);
                    PhotonNetwork.Instantiate(bomb.name, transform.position, Quaternion.identity);
                }
            }       
        }
        else //내 클라이언트의 내 객체 제어가 아닌경우 -> Remote (상대방 객체)
        {
            //기존 방식 - 프레임이 끊어지는 (지연) 발생
            //mud - mug online (패킷packit의 양을 늘린다) - 보정
            this.transform.position = Vector3.Lerp(this.transform.position, setPos, Time.deltaTime * 30f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, setRot, Time.deltaTime * 30f);
        }
    }
    //데이터가 차곡차곡 순차적으로 오고가고 함
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true) //내 객체의 값이나 행동이 이루어졌을 때(PhotoneView=mine)
        {
            //상대방이 보는 내 객체(Remote)에 값을 주어야겠지
            //보내는 함수 - stream.sendNext
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);

        }
        if (stream.IsReading)        // //상대방이 보는 내 객체(PhotoneView=Remote)일때
        {
            setPos = (Vector3)stream.ReceiveNext();   //맨 처음 position으로 넣었으니, posiotion 값
            setRot = (Quaternion)stream.ReceiveNext();   //rotation 값 넣었으니 rotation 값     
        }
    }

    public void GoLobby()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        //PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel("Re_MainTitle");

    }

    public void OnWinUI()
    {
        if(findPlayer.Length < 2 && isLose == false)
        {
            UI_Manager.instance.YouWin();
        }

    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Explosion")
        {
           

            if (photonView.IsMine == true)
            {
                isLive = false;
                anim.SetTrigger("IsDie");
               
                isLose = true;
                UI_Manager.instance.YouLose();
                PhotonNetwork.Destroy(this.gameObject);
            }
            
           
        }
    }

}