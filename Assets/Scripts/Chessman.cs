using UnityEngine;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Chessman : MonoBehaviour
{

    public MoveThePlate MoveThePlatescript;
    public Game gameManager;
    public GameObject controller;
    public GameObject movePlate;
    public string player;
    public string currentPlayer = "white";
    public GameObject DMPawn;
    public int xBoard = -1;
    public int yBoard = -1;
    public Sprite black_king, black_queen, black_bishop, black_knight, black_rook, black_pawn;
    public Sprite white_king, white_queen, white_bishop, white_knight, white_rook, white_pawn;
    public GameObject Piece;
    public GameObject Rook;
    public bool wCastling=true;
    public bool bCastling=true;
    public bool wr00Castling=true;
    public bool wr70Castling=true;
    public bool br07Castling=true;
    public bool br77Castling=true;
    public int XR;
    public int YR;
    public int TXR;
    public int TYR;
    public bool castling=false;
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
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
        this.transform.position = new Vector3(x, y, -1.0f);
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
        if (this.gameObject.name== "tabla_sah") 
        DestroyMovePlates();
        //se activeaza cand apas pe un obiect 
        else if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitatiateMovePlates();
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
            MovePlateAttackSpawn(x, y, xBoard,yBoard);
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
        if (gameObject.name == "black_king")
        {

            if (gm.PositionOnBoard(xBoard, yBoard+1) &&(gm.GetPosition(xBoard, yBoard + 1) == null || gm.GetPosition(xBoard, yBoard + 1).name != "white_king"))
                PointMovePlate(xBoard, yBoard + 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard, yBoard - 1)&& (gm.GetPosition(xBoard, yBoard - 1) == null || gm.GetPosition(xBoard, yBoard - 1).name != "white_king"))
                PointMovePlate(xBoard, yBoard - 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard-1, yBoard - 1) && (gm.GetPosition(xBoard - 1, yBoard - 1) == null || gm.GetPosition(xBoard - 1, yBoard - 1).name != "white_king"))
                PointMovePlate(xBoard - 1, yBoard - 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard-1, yBoard ) && (gm.GetPosition(xBoard - 1, yBoard) == null || gm.GetPosition(xBoard - 1, yBoard).name != "white_king"))
                PointMovePlate(xBoard - 1, yBoard, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard-1, yBoard + 1) &&( gm.GetPosition(xBoard - 1, yBoard + 1) == null || gm.GetPosition(xBoard - 1, yBoard + 1).name != "white_king"))
                PointMovePlate(xBoard - 1, yBoard + 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard+1, yBoard - 1) && (gm.GetPosition(xBoard + 1, yBoard - 1) == null || gm.GetPosition(xBoard + 1, yBoard - 1).name != "white_king"))
                PointMovePlate(xBoard + 1, yBoard - 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard+1, yBoard ) &&( gm.GetPosition(xBoard + 1, yBoard) == null || gm.GetPosition(xBoard + 1, yBoard).name != "white_king"))
                PointMovePlate(xBoard + 1, yBoard, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard+1, yBoard + 1) && (gm.GetPosition(xBoard + 1, yBoard + 1) == null || gm.GetPosition(xBoard + 1, yBoard + 1).name != "white_king"))
                PointMovePlate(xBoard + 1, yBoard + 1, xBoard, yBoard);
        }
        else
        {
            if (gm.PositionOnBoard(xBoard, yBoard + 1) && (gm.GetPosition(xBoard, yBoard + 1) == null || gm.GetPosition(xBoard, yBoard + 1).name != "black_king"))
                PointMovePlate(xBoard, yBoard + 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard, yBoard - 1) && (gm.GetPosition(xBoard, yBoard - 1) == null || gm.GetPosition(xBoard, yBoard - 1).name != "black_king"))
                PointMovePlate(xBoard, yBoard - 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard - 1, yBoard - 1) && (gm.GetPosition(xBoard - 1, yBoard - 1) == null || gm.GetPosition(xBoard - 1, yBoard - 1).name != "black_king"))
                PointMovePlate(xBoard - 1, yBoard - 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard - 1, yBoard) && (gm.GetPosition(xBoard - 1, yBoard) == null || gm.GetPosition(xBoard - 1, yBoard).name != "black_king"))
                PointMovePlate(xBoard - 1, yBoard, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard - 1, yBoard + 1) && (gm.GetPosition(xBoard - 1, yBoard + 1) == null || gm.GetPosition(xBoard - 1, yBoard + 1).name != "black_king"))
                PointMovePlate(xBoard - 1, yBoard + 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard + 1, yBoard - 1) && (gm.GetPosition(xBoard + 1, yBoard - 1) == null || gm.GetPosition(xBoard + 1, yBoard - 1).name != "black_king"))
                PointMovePlate(xBoard + 1, yBoard - 1, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard + 1, yBoard) && (gm.GetPosition(xBoard + 1, yBoard) == null || gm.GetPosition(xBoard + 1, yBoard).name != "black_king"))
                PointMovePlate(xBoard + 1, yBoard, xBoard, yBoard);
            if (gm.PositionOnBoard(xBoard + 1, yBoard + 1) &&( gm.GetPosition(xBoard + 1, yBoard + 1) == null || gm.GetPosition(xBoard + 1, yBoard + 1).name != "black_king"))
                PointMovePlate(xBoard + 1, yBoard + 1, xBoard, yBoard);
        }

    }

    public void PointMovePlate(int x, int y,int a ,int b)
    {
        Game gm = controller.GetComponent<Game>();

        if (gm.PositionOnBoard(x, y))
        {
            GameObject chp = gm.GetPosition(x, y);
            if (chp == null)
            {
                MovePlateSpawn(x, y, a,b);
            }

            else if (gm.PositionOnBoard(x, y) && gm.GetPosition(x, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y, a,b);
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
                MovePlateAttackSpawn(x + 1, y + 1, x, y);
            }
            if (gm.PositionOnBoard(x - 1, y + 1) && gm.GetPosition(x - 1, y + 1) != null && gm.GetPosition(x - 1, y + 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y + 1,x,y);
            }
            if (y == 4 && gm.PositionOnBoard(x + 1, y + 1) && GetEnPassant(x + 1, y, Piece))
            {
                EnPassant(x + 1, y + 1);
               
            }   
            if (y == 4 && gm.PositionOnBoard(x - 1, y + 1) && GetEnPassant(x - 1, y, Piece))
            {
                EnPassant(x - 1, y + 1);
            }
        }
    }
    public void BPawnMovePlate(int x, int y)
    {
        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        if (y == 6 && gm.PositionOnBoard(x, y - 1) && gm.PositionOnBoard(x, y - 2) && gm.GetPosition(x, y - 1) == null && gm.GetPosition(x, y - 2) == null)
        {
            MovePlateSpawn(x, y - 2,x,y);
        }
        if (gm.PositionOnBoard(x, y - 1))
        {
            if (gm.GetPosition(x, y - 1) == null)
            {
                MovePlateSpawn(x, y - 1, x, y);
            }
            if (gm.PositionOnBoard(x - 1, y - 1) && gm.GetPosition(x - 1, y - 1) != null && gm.GetPosition(x - 1, y - 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y - 1,x,y);
            }
            if (gm.PositionOnBoard(x + 1, y - 1) && gm.GetPosition(x + 1, y - 1) != null && gm.GetPosition(x + 1, y - 1).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y - 1,x,y);
            }
            if (y == 3 && gm.PositionOnBoard(x + 1, y - 1) && GetEnPassant(x+1,y,Piece))
            {
                EnPassant(x + 1, y - 1);
            }
            if (y == 3 && gm.PositionOnBoard(x - 1, y - 1) && GetEnPassant(x - 1, y,Piece))
            {
                EnPassant(x - 1, y - 1);
            }
        }
    }


    public bool GetEnPassant(int tox, int toy,GameObject obj)
    {
        Game game = controller.GetComponent<Game>();
        Game.Move a =game.GetTheLastMove();

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
    public void EnPassant(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -1), Quaternion.identity);
        MoveThePlate mpScript = mp.GetComponent<MoveThePlate>();
        Game game = controller.GetComponent<Game>();
        Game.Move a = game.GetTheLastMove();
        mpScript.DMPawn =a.piece;
        mpScript.PwnTQn = false;
        mpScript.piece = Piece;
        mpScript.enPassant = true;
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void BCastling()
    {
        Piece = gameObject;
        Game gm = controller.GetComponent<Game>();
        if (gm.GetComponent<Game>().BCastling)
        {
            if (br07Castling && gm.GetPosition(1, 7) == null && gm.GetPosition(2, 7) == null && gm.GetPosition(3, 7) == null)
            {
                Rook = gm.GetComponent<Game>().GetPosition(0, 7);
                castling = true;
                XR = 0;
                YR = 7;
                TXR = 3;
                TYR = 7;
                MovePlateSpawn(2, 7, 4, 7);

            }
            if (br77Castling && gm.GetPosition(6, 7) == null && gm.GetPosition(5, 7) == null)
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
            if(wr00Castling && gm.GetPosition(1,0) == null && gm.GetPosition(2,0) == null && gm.GetPosition(3,0) == null)
            {
                Rook=gm.GetComponent<Game>().GetPosition(0,0);
                castling = true;
                XR = 0;
                YR = 0;
                TXR = 3;
                TYR = 0;
                MovePlateSpawn(2, 0,4,0);
                
            }
            if(wr70Castling && gm.GetPosition(6, 0) == null && gm.GetPosition(5, 0) == null)
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
    public void MovePlateSpawn(int matrixX, int matrixY,int i,int j)
    {
        Piece = gameObject;
        float x = matrixX;
        float y = matrixY;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -2), Quaternion.identity);
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
                mpScript.PwnTQn= false;
            }
        }
        if(castling)
        {
         mpScript.castling = true;
            mpScript.Rook = Rook;
            mpScript.xr = XR;
            mpScript.yr = YR;
            mpScript.txr = TXR; 
            mpScript.tyr=TYR;
        }
        mpScript.IX = i;
        mpScript.IY = j;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        castling = false;
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY,int i,int j)
    {
        Piece = gameObject;
        float x = matrixX;
        float y = matrixY;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -2), Quaternion.identity);
        MoveThePlate mpScript = mp.GetComponent<MoveThePlate>();
        mpScript.attack = true;
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
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.IX = i;
        mpScript.IY = j;
    }
    public bool IsLegalMove(Vector2Int destination)
    {
        Game game =controller.GetComponent<Game>();

       if(!game.IsKingInCheck())
        return true;
       else return false;
    }

}
