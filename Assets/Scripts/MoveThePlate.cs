using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Game;


public class MoveThePlate : MonoBehaviour
{
    public GameObject controller;
    public GameObject lastMovePlate;
    public GameObject movePlate;
    public LegalMovesManager legalMovesManager;

    GameObject reference = null;
    public GameObject test;
    int matrixX;
    int matrixY;
    public int IX;
    public int IY;
    public bool attack =false;
    public bool PwnTQn = false;
    public GameObject piece;
    public bool enPassant;
    public GameObject DMPawn;
    public GameObject Rook;
    public bool castling=false;
    public int xr;
    public int yr;
    public int tyr;
    public int txr;
    public bool checkl=false;
    public AudioSource source;
    public AudioClip move, moveattack, check;
    public float a, b;
    public void Start()
    {
       
        if (attack)
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
           
            GameObject chp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            if (chp != null)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else if (enPassant && chp==null)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            }
        }
    }

    public void PlaySound(AudioClip Clip)
    {
        GameObject surce = GameObject.FindGameObjectWithTag("source");
        source.enabled=true;
        
        surce.GetComponent<AudioSource>().clip = Clip;
        surce.GetComponent<AudioSource>().Play();
      
    }

    public void OnMouseUp()
    {
        MakeMove();
        GameObject[] MM = GameObject.FindGameObjectsWithTag("tag2");
        if (MM.Length>1 || PlayerPrefs.GetString("Multiplayer")=="yes")
            controller.GetComponent<MultiplayerManager>().SendMove(matrixX, matrixY, IX, IY);
        else
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<ChessBot>().BotTurn();
        }

    }
    public void MakeMove() {
        Debug.Log(this);
            controller = GameObject.FindGameObjectWithTag("GameController");
         
            if (attack == true)
            {
                GameObject chp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
                if (enPassant)
                {
                    chp = DMPawn;
                enabled = false;
                }
            if (controller.GetComponent<LegalMovesManager>().PlayerColour(chp) == 1)
            {
                int f = 0;
                for (int i = 0; i < controller.GetComponent<Game>().playerWhite.Length; i++)
                {
                    if (controller.GetComponent<Game>().playerWhite[i] == chp)
                    {
                        f = i;
                        break;
                    }
                }
                controller.GetComponent<Game>().playerWhite[f] = null; 
            }
            else
            {
                int f = 0;
                for (int i = 0; i < controller.GetComponent<Game>().playerBlack.Length; i++)
                {
                    if (controller.GetComponent<Game>().playerBlack[i] == chp)
                    {
                        f = i;
                        break;
                    }
                }
                controller.GetComponent<Game>().playerBlack[f] = null;
            }

                Destroy(chp);
                controller.GetComponent<Game>().MadeMoves = 0;
                controller.GetComponent<Game>().FullMoves = 0;
                controller.GetComponent<Game>().positions[matrixX,matrixY] = piece;
            }
       
            controller.GetComponent<Game>().SetEmptyPosition(IX, IY);

            piece.GetComponent<Chessman>().SetXBoard(matrixX);
            piece.GetComponent<Chessman>().SetYBoard(matrixY);
            piece.GetComponent<Chessman>().SetCoords();
      
           if (piece.name=="white_pawn" || piece.name == "black_pawn")
           {
                controller.GetComponent<Game>().MadeMoves = 0;
                controller.GetComponent<Game>().FullMoves = 0;
           }
            else
            {
                controller.GetComponent<Game>().MadeMoves ++;
                controller.GetComponent<Game>().FullMoves = controller.GetComponent<Game>().MadeMoves/2;
            }
            controller.GetComponent<Game>().SetPosition(piece);
            controller.GetComponent<Game>().NextTurn();
            piece.GetComponent<Chessman>().DestroyMovePlates();
        Debug.Log("aproape");
       
           if (PwnTQn)
            {
                piece.GetComponent<Chessman>().PawnToQueen();
            }
            else if (castling)
            {
                controller.GetComponent<Game>().SetEmptyPosition(xr, yr);
                Rook.GetComponent<Chessman>().SetXBoard(txr);
                Rook.GetComponent<Chessman>().SetYBoard(tyr);
                Rook.GetComponent<Chessman>().SetCoords();
                controller.GetComponent<Game>().SetPosition(Rook);
                if (Rook.name == "white_rook")
                    Rook.GetComponent<Chessman>().wCastling = false;
                else Rook.GetComponent<Chessman>().bCastling = false;
                Rook.GetComponent<Chessman>().castling = false;
                castling = false;
                Rook.GetComponent<Chessman>().castling = false;
            }
            controller.GetComponent<Game>().RecordTheMove(piece, IX, IY, piece.GetComponent<Chessman>().GetXBoard(), piece.GetComponent<Chessman>().GetYBoard());
        legalMovesManager =controller.GetComponent<LegalMovesManager>();

        
         if (legalMovesManager.IsKingInCheck())
            {
            a = controller.GetComponent<LegalMovesManager>().Xkposition; 
            b= controller.GetComponent<LegalMovesManager>().Ykposition;
            a *= 6.06f;
            b *= 6.06f;
            a -= 21.24f;
            b -= 21.24f;
            Debug.Log(controller.GetComponent<LegalMovesManager>().GetCheckPieceFast());
            if (legalMovesManager.IsCheckmate())
                {
                controller.GetComponent<Game>().Winner();
                    legalMovesManager.SetGameOver();
                }
                else
                {
                    Debug.Log("sah " + controller.GetComponent<Game>().currentPlayer);
                }
            checkl = true;
            Debug.Log(this);
            PlaySound(check);
            }
         
        
         else if (legalMovesManager.GetComponent<LegalMovesManager>().IsStalemate() || controller.GetComponent<Game>().GetFiftyMoveRule())
            {
                Debug.Log("pat");
                legalMovesManager.SetGameOver();
            }
          
            float x = matrixX;
            float y = matrixY;
            x *= 6.06f;
            y *= 6.06f;
            x -= 21.24f;
            y -= 21.24f;
            float X = IX;
            float Y = IY;
            X *= 6.06f;
            Y *= 6.06f;
            X -= 21.24f;
            Y -= 21.24f;
            GameObject[] lastMovePlat = GameObject.FindGameObjectsWithTag("lastMovePlate");
            for(int i=0; i<lastMovePlat.Length; i++)
            {
                Destroy(lastMovePlat[i]);
            }
            GameObject LMP=Instantiate(lastMovePlate, new Vector3(x, y, 82), Quaternion.identity);
            GameObject mp=Instantiate(lastMovePlate, new Vector3(X, Y, 82), Quaternion.identity);
            if(checkl)
            {
                GameObject kng = Instantiate(lastMovePlate, new Vector3(a, b, 80), Quaternion.identity);
                kng.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0.0f, 1.0f);
                checkl = false;
            }
            mp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
            if (attack) {
            LMP.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.4f, 0.3f, 1.0f);
            PlaySound(moveattack);
                    attack = false;
                }
                else
                {
                    PlaySound(move);
           
            LMP.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.6f, 0.3f, 1.0f);
        }
       
    }
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    

    public void RecieveMove(int a,int b, int X, int Y)
    {
        //a,b pct unde ajunge

        controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Game>().positions[a,b] != null)
            attack = true;
        else
            attack = false;
        float x =X;
        float y =Y;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;
        matrixX = a; matrixY=b;
        IX=X; IY = Y;
        piece = controller.GetComponent<Game>().GetPosition(X,Y);
        piece.transform.position = new Vector3(x, y, 80);
        reference = piece;
        if (piece.name == "white_pawn" || piece.name == "black_pawn")
        {
            if (matrixY == 7 || matrixY == 0)
            {
                PwnTQn = true;
            }
            else
            {
                PwnTQn = false;
            }
            if (piece.name == "white_pawn" && Y == 4 && b == 5)
            {
                if (controller.GetComponent<Game>().positions[a,b] == null)
                {

                    DMPawn = controller.GetComponent<Game>().positions[a,Y];
                   
                    attack = true;
                    enPassant = true;
                }
               
            }
            else if (piece.name == "black_pawn" && Y == 3 && b == 2)
            {
                if (controller.GetComponent<Game>().positions[a,b] == null)
                {
                    DMPawn = controller.GetComponent<Game>().positions[a,Y];
                    attack = true;
                    enPassant = true;
                }
            }
        }
        MakeMove();
    }
    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference() { 
        return reference; 
    }

   


}