using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public Chessman chessman;
    public GameObject controll;
    public Button CancelButton;
    public PhotonView ptnview;
    public char randomValue;
    public string yourColour = null;
    public string opponentColour;
    public Image OnlineImage;
    public Text WFP;
    public Text Tips;
    public Image GiveUp;
    public Image Draw;
    public Text giveUp;
    public Text draw;
    public Button yeah;
    public Button nope;
    public Button ye;
    public Button no;
    
    public bool multiplayer = false;
    public PhotonView view;
    private void Start()
    {
        GameObject[] MM = GameObject.FindGameObjectsWithTag("tag2");
        if (MM.Length < 2)
        {
            if (this.CompareTag("tag2") || this.CompareTag("tag4"))
            {
                DontDestroyOnLoad(gameObject);

            }
        }
    }
    public void WhileConnectingToServer()
    {
        OnlineImage.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(true);
        WFP.gameObject.SetActive(true);
        Tips.gameObject.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();


    }
    public void CancelTheConnection()
    {
        CancelButton.gameObject.SetActive(false);
        OnlineImage.gameObject.SetActive(false);
        WFP.gameObject.SetActive(false);
        Tips.gameObject.SetActive(false);
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        if (gameObject.tag == "tag2")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("chessRoom", roomOptions, TypedLobby.Default);
        }
    }
    public override void OnJoinedRoom()
    {
        if (gameObject.tag == "tag2")
        {
          
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                randomValue = (Random.Range(0, 2) == 0) ? 'b' : 'w';
                if (randomValue == 'b')
                {
                    yourColour = "black";
                    opponentColour = "white";
                }
                else
                {
                    yourColour = "white";
                    opponentColour = "black";
                }
                multiplayer = true;
              
                GameObject[] CC = GameObject.FindGameObjectsWithTag("tag2");
                if (CC.Length == 1)
                {
                    GameObject controller1 = PhotonNetwork.Instantiate(controll.name, Vector3.zero, Quaternion.identity);
              
                    view = controller1.GetComponent<PhotonView>();
                    if (!view.IsMine)
                    {
                        view.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }


                    view.RPC("RecieveYourColour", RpcTarget.Others, opponentColour, name);
                    string myname;
                    if (PlayerPrefs.GetString("username") != null)
                        myname = PlayerPrefs.GetString("username");
                    else
                        myname = "Guest";
                    controller1.GetComponent<Game>().MyNamet = myname;
                    controller1.GetComponent<Chessman>().MyTurn = yourColour;
                    controller1.GetComponent<Game>().MyTurn = yourColour;
                    PlayerPrefs.SetString("Continue", "no");
                    PlayerPrefs.SetString("Multiplayer", "yes");
                }
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                string myname;
                if (PlayerPrefs.GetString("username") != null)
                    myname = PlayerPrefs.GetString("username");
                else
                    myname = "Guest";
               
            }
               
        }
                
    }

    IEnumerator WaitForSecondPlayer()
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            yield return new WaitForSeconds(1);
    }

    public void SendMove(int x, int y, int a, int b)
    {
        GameObject [] controll = GameObject.FindGameObjectsWithTag("tag2");
        if (controll != null)
        {
            view = controll[0].GetComponent<PhotonView>();
            Debug.Log(controll.Length + " "+view);
            if (view.IsMine==false)
            {
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            view.RPC("RecieveMove", RpcTarget.Others, x, y, a, b);
        }
    }
    [PunRPC]
    void RecieveMove(int x,int y,int a, int b)
    {
        GameObject[] controller = GameObject.FindGameObjectsWithTag("tag2");
        controller[0].GetComponent<MoveThePlate>().RecieveMove(x,y,a,b);
    }
    [PunRPC]
   void RecieveYourColour(string colour,string name)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("tag2");
        controller.GetComponent<Game>().OppNamet= name;
        controller.GetComponent<Chessman>().MyTurn = colour;
        controller.GetComponent<Game>().MyTurn= colour;
        controller.GetComponent<Chessman>().multiplayer = true;
        multiplayer = true;
        string myname;
        if (PlayerPrefs.GetString("username") != null)
            myname = PlayerPrefs.GetString("username");
        else
            myname = "Guest";
        controller.GetComponent<Game>().MyNamet = myname;
        Debug.Log("da");
        PlayerPrefs.SetString("Continue", "no");
        PlayerPrefs.SetString("Multiplayer", "yes");
        GameObject[] CC = GameObject.FindGameObjectsWithTag("tag2");
        if (CC.Length == 1)
        {
            GameObject controller1 = PhotonNetwork.Instantiate(controll.name, Vector3.zero, Quaternion.identity);


            view = controller1.GetComponent<PhotonView>();
            if (!view.IsMine)
            {
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            view.RPC("GetYourOpp", RpcTarget.Others, myname);
        }
        else
        {

            view = controll.GetComponent<PhotonView>();
            if (!view.IsMine)
            {
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            string hiscolor=colour=="white"? "black":"white";
            view.RPC("GetYourOpp", RpcTarget.Others, myname,hiscolor);
        }
        SceneManager.LoadScene("Multiplayer lobby");

    }
    [PunRPC]
    void GetYourOpp(string name,string hiscolor)
    {
        Debug.Log("yes");
        GameObject[] controller = GameObject.FindGameObjectsWithTag("tag2");
        controller[0].GetComponent<Game>().OppNamet= name;
        string myname;
        if (PlayerPrefs.GetString("username") != null)
            myname = PlayerPrefs.GetString("username");
        else
            myname = "Guest";
        controller[0].GetComponent<Game>().MyNamet= myname;
        controller[0].GetComponent<Chessman>().multiplayer = true;
        controller[0].GetComponent<Chessman>().MyTurn=hiscolor;
        SceneManager.LoadScene("Multiplayer lobby");
    }
    [PunRPC]
    public void OppGaveUp(string name)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Game>().WinnerText(name);
        controller.GetComponent<LegalMovesManager>().SetGameOver();
    }
    public bool drawbool = false;

    [PunRPC]
    public void DUWantDraw()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<MultiplayerManager>().Draw.gameObject.SetActive(true);
        controller.GetComponent<MultiplayerManager>().draw.gameObject.SetActive(true);
        controller.GetComponent<MultiplayerManager>().ye.gameObject.SetActive(true);
        controller.GetComponent<MultiplayerManager>().no.gameObject.SetActive(true);
        controller.GetComponent<MultiplayerManager>().drawbool = true;
    }
    //I haven't slept in 3 days
    public void IGotTired(string ans)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<MultiplayerManager>().drawbool)
        {
            Draw.gameObject.SetActive(false);
            draw.gameObject.SetActive(false);
            ye.gameObject.SetActive(false);
            no.gameObject.SetActive(false);
            if (ans == "ye")
            {
                GameObject MM = GameObject.FindGameObjectWithTag("tag2");
                view = MM.GetPhotonView();
                view.RPC("DrawWon", RpcTarget.All);
            }
            controller.GetComponent<MultiplayerManager>().drawbool  = false;
        }
    }
    [PunRPC]
    public void DrawWon()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Game>().Draw();
        controller.GetComponent<LegalMovesManager>().SetGameOver();
    }
    public void DoYouGiveUp()
    {
        GiveUp.gameObject.SetActive(true);
        giveUp.gameObject.SetActive(true);
        yeah.gameObject.SetActive(true);
        nope.gameObject.SetActive(true);
    }

    public void AreYouSure(string answer)
    {
        if(answer == "ye")
        {
            string myname;
            if (PlayerPrefs.GetString("username") != null)
                myname = PlayerPrefs.GetString("username");
            else
                myname = "Guest";
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<Game>().WinnerText(myname);
            controller.GetComponent<LegalMovesManager>().SetGameOver();
            GameObject MM = GameObject.FindGameObjectWithTag("tag2");
            view = MM.GetPhotonView();
            view.RPC("OppGaveUp", RpcTarget.Others, myname);
        }


        GiveUp.gameObject.SetActive(false);
        giveUp.gameObject.SetActive(false);
        yeah.gameObject.SetActive(false);
        nope.gameObject.SetActive(false);
    }

    public void DrawOrNot()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Game>().gameOver==false)
        {
            Draw.gameObject.SetActive(true);
            draw.gameObject.SetActive(true);
            ye.gameObject.SetActive(true);
            no.gameObject.SetActive(true);
        }
    }

    public void RUSure(string ans)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<MultiplayerManager>().drawbool == false)
        {
            GameObject MM = GameObject.FindGameObjectWithTag("tag2");
            view = MM.GetPhotonView();
            if (ans == "ye")
                view.RPC("DUWantDraw", RpcTarget.Others);
            Draw.gameObject.SetActive(false);
            draw.gameObject.SetActive(false);
            ye.gameObject.SetActive(false);
            no.gameObject.SetActive(false);

        }
    }

    public void GetOut()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Game>().gameOver)
        SceneManager.LoadScene("Game UI");
        else
        DoYouGiveUp();
    }
}