//��Ʈ��ũ ���̺귯�� �߰�
using Photon.Pun;
using UnityEngine;


public class PlayerControlSync : MonoBehaviourPun, IPunObservable   //��ü�� ��ũ�� ���ߴ� �������̽� //MonoBehaviour
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

    //������ ��ź ����
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
            dir.y = 0; //y��ǥ�� ���ƹ�����

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
        else //�� Ŭ���̾�Ʈ�� �� ��ü ��� �ƴѰ�� -> Remote (���� ��ü)
        {
            //���� ��� - �������� �������� (����) �߻�
            //mud - mug online (��Ŷpackit�� ���� �ø���) - ����
            this.transform.position = Vector3.Lerp(this.transform.position, setPos, Time.deltaTime * 30f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, setRot, Time.deltaTime * 30f);
        }
    }
    //�����Ͱ� �������� ���������� ������ ��
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true) //�� ��ü�� ���̳� �ൿ�� �̷������ ��(PhotoneView=mine)
        {
            //������ ���� �� ��ü(Remote)�� ���� �־�߰���
            //������ �Լ� - stream.sendNext
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);

        }
        if (stream.IsReading)        // //������ ���� �� ��ü(PhotoneView=Remote)�϶�
        {
            setPos = (Vector3)stream.ReceiveNext();   //�� ó�� position���� �־�����, posiotion ��
            setRot = (Quaternion)stream.ReceiveNext();   //rotation �� �־����� rotation ��     
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