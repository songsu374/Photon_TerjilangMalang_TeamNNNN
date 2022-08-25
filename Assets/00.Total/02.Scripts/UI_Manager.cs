using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject drawUI;

    public bool isWin = false;
    public bool isLose = false;
    public bool isdraw = false;

    // Start is called before the first frame update
    void Start()
    {
        winUI.SetActive(false);
        loseUI.SetActive(false);
        drawUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClikGoLobbyBtn()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel("Re_MainTitle");
    }

    public void YouWin()
    {
        if(isLose == false && isdraw == false)
        {
        winUI.SetActive(true);
        isWin = true;

        }
    }

    public void YouLose()
    {
        if (isWin == false && isdraw == false)
        {
            loseUI.SetActive(true);
            isLose = true;
        }
            
    }

    public void YouDraw()
    {
        if (isWin == false && isLose == false)
        {
            drawUI.SetActive(true);
            isdraw = true;
        }
            
    }
}
