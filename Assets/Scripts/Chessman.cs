using UnityEngine;
using System;
using UnityEngine.UIElements;
using Photon.Pun;
using static Game;
using System.Collections;
using UnityEngine.UI;

public class Chessman : MonoBehaviour
{
   
    public MoveThePlate MoveThePlatescript;
    public Game gameManager;
    public LegalMovesManager LegalMovesManager;
    public MultiplayerManager MpMan;
    public GameObject controller;
    public GameObject movePlate;
    public string player;
    public string currentPlayer = "white";
    public string MyTurn=null;
    public GameObject DMPawn;
    public int xBoard = -1;
    public int yBoard = -1;
    public Sprite black_king, black_queen, black_bishop, black_knight, black_rook, black_pawn;
    public Sprite white_king, white_queen, white_bishop, white_knight, white_rook, white_pawn;
    public GameObject Piece;
    public GameObject Rook;
    public bool wCastling = true;
    public bool bCastling = true;
    public bool wqCastling = true;
    public bool wkCastling = true;
    public bool bqCastling = true;
    public bool bkCastling = true;
    public int XR;
    public int YR;
    public int TXR;
    public int TYR;
    public bool castling = false;
    public bool multiplayer = false;

   
    public void Activate()
    {
        GameObject[] MM = GameObject.FindGameObjectsWithTag("tag2");
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (MM[0].GetComponent<Chessman>().MyTurn == "white" || MM[0].GetComponent<Chessman>().MyTurn == "black")
        {
            Debug.Log("works 100%");
            MyTurn = MM[0].GetComponent<Chessman>().MyTurn;
            MM[0].GetComponent<Chessman>().multiplayer = true;
          
        }
        else
            MyTurn = "white";
        string e = PlayerPrefs.GetString("Multiplayer");
        if(e=="yes")
            multiplayer = true;
        SetCoords();
        switch (this.name)
        {
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;

            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;

        }
    }
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;
        this.transform.position = new Vector3(x, y, 80);
    }


    public int GetXBoard()
    {
        return xBoard;
    }


    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;

    }

   
    public void OnMouseUp()
    {
        Debug.Log(MyTurn);
        LegalMovesManager lg= controller.GetComponent<LegalMovesManager>();
         if(multiplayer && !lg.GetGameOver()){
        if (this.gameObject.name == "tabla_sah")
                DestroyMovePlates();
            else if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player && player==MyTurn)
            {
                DestroyMovePlates();
                InitatiateMovePlates();
            }
        }
        
        else if (!lg.GetGameOver())
        {
            if (this.gameObject.name == "tabla_sah")
                DestroyMovePlates();
            else if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player )
            {
                DestroyMovePlates();
                InitatiateMovePlates();
            }
        }
    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitatiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_king":
                SurroundMovePlate();
                BCastling();
                break;
            case "white_king":
                SurroundMovePlate();
                WCastling();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                BPawnMovePlate(xBoard, yBoard);
                break;
            case "white_pawn":
                WPawnMovePlate(xBoard, yBoard);
                break;
        }
    }

    public void LineMovePlate(int xI, int yI)
    {
        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        int x = xBoard + xI;
        int y = yBoard + yI;

        while (gm.PositionOnBoard(x, y) && gm.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y, xBoard, yBoard);
            x += xI;
            y += yI;
        }
        if (gm.PositionOnBoard(x, y) && gm.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(gameObject, x, y, xBoard, yBoard);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2, xBoard, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 2, xBoard, yBoard);
        PointMovePlate(xBoard + 2, yBoard + 1, xBoard, yBoard);
        PointMovePlate(xBoard - 2, yBoard + 1, xBoard, yBoard);
        PointMovePlate(xBoard - 2, yBoard - 1, xBoard, yBoard);
        PointMovePlate(xBoard - 1, yBoard - 2, xBoard, yBoard);
        PointMovePlate(xBoard + 1, yBoard - 2, xBoard, yBoard);
        PointMovePlate(xBoard + 2, yBoard - 1, xBoard, yBoard);
    }
    public void SurroundMovePlate()
    {
        Game gm = controller.GetComponent<Game>();
        PointMovePlate(xBoard, yBoard -1, xBoard, yBoard);
        PointMovePlate(xBoard , yBoard + 1, xBoard, yBoard);
        PointMovePlate(xBoard + 1, yBoard - 1, xBoard, yBoard);
        PointMovePlate(xBoard +1, yBoard , xBoard, yBoard);
        PointMovePlate(xBoard +1, yBoard + 1, xBoard, yBoard);
        PointMovePlate(xBoard - 1, yBoard -1, xBoard, yBoard);
        PointMovePlate(xBoard - 1, yBoard  , xBoard, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1, xBoard, yBoard);

    }

    public void PointMovePlate(int x, int y, int a, int b)
    {
        Game gm = controller.GetComponent<Game>();

        if (gm.PositionOnBoard(x, y))
        {
            GameObject chp = gm.GetPosition(x, y);
            if (chp == null)
            {
                MovePlateSpawn(x, y, a, b);

            }

            else if (gm.PositionOnBoard(x, y) && gm.GetPosition(x, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(gameObject, x, y, a, b);
                
            }

        }
    }
    public void WPawnMovePlate(int x, int y)
    {
        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        if (y == 1 && gm.PositionOnBoard(x, y + 1) && gm.PositionOnBoard(x, y + 2) && gm.GetPosition(x, y + 1) == null && gm.GetPosition(x, y + 2) == null)
        {
            MovePlateSpawn(x, y + 2, x, y);
        }

        if (gm.PositionOnBoard(x, y + 1))
        {
            if (gm.GetPosition(x, y + 1) == null)
            {
                MovePlateSpawn(x, y + 1, x, y);
            }
            if (gm.PositionOnBoard(x + 1, y + 1) && gm.GetPosition(x + 1, y + 1) != null && gm.GetPosition(x + 1, y + 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(gameObject, x + 1, y + 1, x, y);
            }
            if (gm.PositionOnBoard(x - 1, y + 1) && gm.GetPosition(x - 1, y + 1) != null && gm.GetPosition(x - 1, y + 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(gameObject, x - 1, y + 1, x, y);
            }
            if (y == 4 && gm.PositionOnBoard(x + 1, y + 1) && GetEnPassant(x + 1, y, Piece))
            {
                EnPassant(x + 1, y + 1,x,y);

            }
            if (y == 4 && gm.PositionOnBoard(x - 1, y + 1) && GetEnPassant(x - 1, y, Piece))
            {
                EnPassant(x - 1, y + 1,x,y);
            }
        }
    }
    public void BPawnMovePlate(int x, int y)
    {
        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        if (y == 6 && gm.PositionOnBoard(x, y - 1) && gm.PositionOnBoard(x, y - 2) && gm.GetPosition(x, y - 1) == null && gm.GetPosition(x, y - 2) == null)
        {
            MovePlateSpawn(x, y - 2, x, y);
        }
        if (gm.PositionOnBoard(x, y - 1))
        {
            if (gm.GetPosition(x, y - 1) == null)
            {
                MovePlateSpawn(x, y - 1, x, y);
            }
            if (gm.PositionOnBoard(x - 1, y - 1) && gm.GetPosition(x - 1, y - 1) != null && gm.GetPosition(x - 1, y - 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(gameObject, x - 1, y - 1, x, y);
            }
            if (gm.PositionOnBoard(x + 1, y - 1) && gm.GetPosition(x + 1, y - 1) != null && gm.GetPosition(x + 1, y - 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(gameObject,x + 1, y - 1, x, y);
            }
            if (y == 3 && gm.PositionOnBoard(x + 1, y - 1) && GetEnPassant(x + 1, y, Piece))
            {
                EnPassant(x + 1, y - 1,x,y);
            }
            if (y == 3 && gm.PositionOnBoard(x - 1, y - 1) && GetEnPassant(x - 1, y, Piece))
            {
                EnPassant(x - 1, y - 1,x,y);
            }
        }
    }


    public bool GetEnPassant(int tox, int toy, GameObject obj)
    {
        Game game = controller.GetComponent<Game>();
        bool y=game.EnPassant;
        Game.Move a = game.GetTheLastMove();

        if (a != null)
        {
            if (a.piece.name != "black_pawn" && a.piece.name != "white_pawn")
            {
                return false;

            }
            else if (toy == 5 && a.piece.name != "black_pawn")
            {
                return false;
            }
            else if (toy == 2 && a.piece.name != "white_pawn")
            {
                return false;
            }
            else if ((a.toX == tox) && (a.toY == toy))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(y)
        {
            GameObject objj = game.positions[tox,toy];
            if(objj!=null &&game.EPassant==objj)
            return true ;
            return false;
        }
        else 
        {
            return false;
        }
    }

    public void PawnToQueen()
    {
        if (gameObject.name == "white_pawn")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = white_queen;
            gameObject.name = "white_queen";
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = black_queen;
            gameObject.name = "black_queen";
        }
    }
    public void EnPassant(int matrixX, int matrixY,int X, int Y)
    {
        Debug.Log("why, just to suffer..");
        float x = matrixX;
        float y = matrixY;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, 80), Quaternion.identity);
        MoveThePlate mpScript = mp.GetComponent<MoveThePlate>();
        Game game = controller.GetComponent<Game>();
        Game.Move a = game.GetTheLastMove();
        mpScript.DMPawn = a.piece;
        mpScript.PwnTQn = false;
        mpScript.piece = Piece;
        mpScript.enPassant = true;
        mpScript.attack = true;
        mpScript.IX = X;
        mpScript.IY = Y;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void BCastling()
    {
        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        if (gm.GetComponent<Game>().BCastling)
        {
            if (bqCastling && gm.GetPosition(1, 7) == null && gm.GetPosition(2, 7) == null && gm.GetPosition(3, 7) == null)
            {
                Rook = gm.GetComponent<Game>().GetPosition(0, 7);
                castling = true;
                XR = 0;
                YR = 7;
                TXR = 3;
                TYR = 7;
                MovePlateSpawn(2, 7, 4, 7);

            }
            if (bkCastling && gm.GetPosition(6, 7) == null && gm.GetPosition(5, 7) == null)
            {
                Rook = gm.GetComponent<Game>().GetPosition(7, 7);
                castling = true;
                XR = 7;
                YR = 7;
                TXR = 5;
                TYR = 7;
                MovePlateSpawn(6, 7, 4, 7);
            }
        }
    }

    public void WCastling()
    {

        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        if (gm.GetComponent<Game>().WCastling)
        {
            if (wqCastling && gm.GetPosition(1, 0) == null && gm.GetPosition(2, 0) == null && gm.GetPosition(3, 0) == null)
            {
                Rook = gm.GetComponent<Game>().GetPosition(0, 0);
                castling = true;
                XR = 0;
                YR = 0;
                TXR = 3;
                TYR = 0;
                MovePlateSpawn(2, 0, 4, 0);

            }
            if (wkCastling && gm.GetPosition(6, 0) == null && gm.GetPosition(5, 0) == null)
            {
                Rook = gm.GetComponent<Game>().GetPosition(7, 0);
                castling = true;
                XR = 7;
                YR = 0;
                TXR = 5;
                TYR = 0;
                MovePlateSpawn(6, 0, 4, 0);
            }
        }
    }

    

    public void MovePlateSpawn(int matrixX, int matrixY, int i, int j)
    {
        GameObject Piece = gameObject;
        LegalMovesManager =controller.GetComponent<LegalMovesManager>();
       if (LegalMovesManager.IsLegal(Piece, i, j, matrixX,matrixY))
        {
           
            float x = matrixX;
            float y = matrixY;
            x *= 6.06f;
            y *= 6.06f;
            x -= 21.24f;
            y -= 21.24f;

            GameObject mp = Instantiate(movePlate, new Vector3(x, y, 80), Quaternion.identity);
            MoveThePlate mpScript = mp.GetComponent<MoveThePlate>();
            mpScript.attack = false;
            mpScript.piece = Piece;
            if (Piece.name == "white_pawn" || Piece.name == "black_pawn")
            {
                if (matrixY == 7 || matrixY == 0)
                {
                    mpScript.PwnTQn = true;
                }
                else
                {
                    mpScript.PwnTQn = false;
                }
            }
            if (castling)
            {
                mpScript.castling = true;
                mpScript.Rook = Rook;
                mpScript.xr = XR;
                mpScript.yr = YR;
                mpScript.txr = TXR;
                mpScript.tyr = TYR;
            }
            mpScript.IX = i;
            mpScript.IY = j;
            mpScript.SetReference(gameObject);
            mpScript.SetCoords(matrixX, matrixY);
            castling = false;
        }
       
    }

    public void MovePlateAttackSpawn(GameObject Piece, int matrixX, int matrixY, int i, int j)
    {
        LegalMovesManager = controller.GetComponent<LegalMovesManager>();
        if (LegalMovesManager.IsLegal(Piece, i, j, matrixX, matrixY))
        {
          
            float x = matrixX;
            float y = matrixY;
            x *= 6.06f;
            y *= 6.06f;
            x -= 21.24f;
            y -= 21.24f;

            GameObject mp = Instantiate(movePlate, new Vector3(x, y, 80), Quaternion.identity);
            MoveThePlate mpScript = mp.GetComponent<MoveThePlate>();
            mpScript.attack = true;
            mpScript.piece = Piece;
            mpScript.castling = false;
            if (Piece.name == "white_pawn" || Piece.name == "black_pawn")
            {
                if (matrixY == 7 || matrixY == 0)
                {
                    mpScript.PwnTQn = true;
                }
                else
                {
                    mpScript.PwnTQn = false;
                }
            }
            mpScript.SetReference(gameObject);
            mpScript.SetCoords(matrixX, matrixY);
            mpScript.IX = i;
            mpScript.IY = j;
         
        }
       

    }
}
