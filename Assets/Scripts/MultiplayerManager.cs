using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Text;


public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public Chessman chessman;
    public GameObject controll;
    public Button CancelButton;
    public PhotonView ptnview;
    public string code = "0";
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

    public Button Joining;
    public InputField key;

    public string MyName;
    public string MyTurn;
    public string OppName;
    public string roomKey;
    public bool multiplayer = false;
    public PhotonView view;
    private void Start()
    {       
       if (this.CompareTag("tag2")){

        DontDestroyOnLoad(gameObject);

       }
    }
    public void TypeOfOpperations(string opper)
    {
        GameObject go = GameObject.FindGameObjectWithTag("tag2");
        go.GetComponent<MultiplayerManager>().code = opper;
    }

    public void TryingTheKey()
    {
        GameObject go = GameObject.FindGameObjectWithTag("tag2");
        go.GetComponent<MultiplayerManager>().code = "2";
        go.GetComponent<MultiplayerManager>().roomKey =key.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void TryingToJoin()
    {
        OnlineImage.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(true);
        key.gameObject.SetActive(true);
        Joining.gameObject.SetActive(true);
        TypeOfOpperations("2");

    }
    public void WhileConnectingToServer(string key)
    {
        TypeOfOpperations(key);
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
        key.gameObject.SetActive(false);
        Joining.gameObject.SetActive(false);
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        if (gameObject.tag == "tag2" && code=="0")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("chessRoom", roomOptions, TypedLobby.Default);
        }
        else if(gameObject.tag == "tag2" && code=="1")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false; 
            roomOptions.MaxPlayers = 2;
            const string chars = "123456789abcdefghijklmnopqrstuvwxyz"; 
            StringBuilder keyBuilder = new StringBuilder();

            System.Random random = new System.Random();
            for (int i = 0; i < 6; i++)
            {
                int index = random.Next(chars.Length);
                keyBuilder.Append(chars[index]);
            }
            string roomKey = keyBuilder.ToString();
           
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            roomOptions.CustomRoomProperties.Add("RoomKey", roomKey);
            Tips.gameObject.SetActive(false);
            Tips.text = roomKey;
            Tips.gameObject.SetActive(true);
            PhotonNetwork.CreateRoom(roomKey, roomOptions, null);
          
        }
        else if(gameObject.tag == "tag2")
        {
          
            PhotonNetwork.JoinRoom(roomKey);
        }
    }
    public override void OnJoinedRoom()
    {
       
        if (gameObject.tag == "tag2")
        {
            if (code == "2")
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RoomKey"))
                {
                    string roomKeyInRoom = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomKey"];
                    if (roomKeyInRoom == roomKey)
                    {
                    
                    }
                    else
                    {
                        Tips.text = "wrong key";
                        PhotonNetwork.Disconnect();
                    }
                }
            }
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
              
                GameObject CC = GameObject.FindGameObjectWithTag("tag2");
              
                    view = CC.GetComponent<PhotonView>();
                    if (!view.IsMine)
                    {
                        view.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                string myname;
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("username")))
                    myname = PlayerPrefs.GetString("username");
                else
                    myname = "Guest";

                view.RPC("RecieveYourColour", RpcTarget.Others, opponentColour, myname);
                    
                    CC.GetComponent<Game>().MyNamet = myname;
                    CC.GetComponent<Chessman>().MyTurn = yourColour;
                    CC.GetComponent<MultiplayerManager>().MyTurn = yourColour;
                    PlayerPrefs.SetString("Continue", "no");
                    PlayerPrefs.SetString("Multiplayer", "yes");
                
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
        GameObject controller = GameObject.FindGameObjectWithTag("tag2");
        controller.GetComponent<MoveThePlate>().RecieveMove(x,y,a,b);
    }
    [PunRPC]
   void RecieveYourColour(string colour,string name)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("tag2");
        controller.GetComponent<MultiplayerManager>().OppName= name;
        controller.GetComponent<Chessman>().MyTurn = colour;
        controller.GetComponent<MultiplayerManager>().MyTurn= colour;
        controller.GetComponent<Chessman>().multiplayer = true;
        multiplayer = true;
        string myname;
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("username")))
            myname = PlayerPrefs.GetString("username");
        else
            myname = "Guest";
        controller.GetComponent<MultiplayerManager>().MyName = myname;
        PlayerPrefs.SetString("Continue", "no");
        PlayerPrefs.SetString("Multiplayer", "yes");
        GameObject CC = GameObject.FindGameObjectWithTag("tag2");
            view = CC.GetComponent<PhotonView>();
            if (!view.IsMine)
            {
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            string hiscolor=colour=="white"? "black":"white";

            view.RPC("GetYourOpp", RpcTarget.Others, myname,hiscolor);

        SceneManager.LoadScene("Multiplayer lobby");

    }
    [PunRPC]
    void GetYourOpp(string name,string hiscolor)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("tag2");
        controller.GetComponent<MultiplayerManager>().OppName = name;
        string myname;
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("username")) )
            myname = PlayerPrefs.GetString("username");
        else
            myname = "Guest";
        controller.GetComponent<MultiplayerManager>().MyName= myname;
        controller.GetComponent<Chessman>().multiplayer = true;
        controller.GetComponent<Chessman>().MyTurn=hiscolor;
        SceneManager.LoadScene("Multiplayer lobby");
    }
    [PunRPC]
    public void OppGaveUp(string name)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (OppName == name)
            name = MyName;
        else
            name = OppName;
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
            GameObject MM = GameObject.FindGameObjectWithTag("tag2");
            view = MM.GetPhotonView();
            view.RPC("OppGaveUp", RpcTarget.All, myname);
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
        {
            GameObject qwop = GameObject.FindGameObjectWithTag("tag2");
            Destroy(qwop);
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Game UI");
            

        }
        else
            DoYouGiveUp();
    }
}