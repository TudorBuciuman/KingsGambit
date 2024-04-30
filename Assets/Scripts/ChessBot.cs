using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Realtime;

public class ChessBot : MonoBehaviour
{
    public GameObject controller;
    public GameObject target;
    public LegalMovesManager LegalMovesManager;
    public Game game;
    public void BotTurn()
    {
        game = GetComponent<Game>();
        List<GameObject> BotPieces = game.GetPlayerPieces();
        int r=BotPieces.Count;
        System.Random systemRandom = new System.Random();
        int t;
        do
        {
            t = 0;

            int y=systemRandom.Next(0, r-1);
            t=SearchForMove(BotPieces[y]);
            t = IsOkeyDokey();
            Debug.Log(t);
        } while (t==0);

        Q = 0;
    }

    public int SearchForMove(GameObject piece)
    {
       int x=piece.GetComponent<Chessman>().GetXBoard();
       int y=piece.GetComponent<Chessman>().GetYBoard();
        PossibleMoves(x,y,piece);
        return 0;
    }

    public void PossibleMoves(int a, int b, GameObject piece)
    {
            switch (piece.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(piece,a,b,1, 0);
                LineMovePlate(piece, a, b, 0, 1);
                LineMovePlate(piece, a, b, 1, 1);
                LineMovePlate(piece, a, b, -1, 0);
                LineMovePlate(piece, a, b, 0, -1);
                LineMovePlate(piece, a, b, -1, -1);
                LineMovePlate(piece, a, b, -1, 1);
                LineMovePlate(piece, a, b, 1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate(a,b);
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(piece, a, b, 1, 1);
                LineMovePlate(piece, a, b, -1, -1);
                LineMovePlate(piece, a, b, -1, 1);
                LineMovePlate(piece, a, b, 1, -1);
                break;
            case "black_king":
                SurroundMovePlate(a,b);
               // BCastling();
                break;
            case "white_king":
                SurroundMovePlate(a,b);
               // WCastling();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(piece, a, b, 1, 0);
                LineMovePlate(piece, a, b, 0, 1);
                LineMovePlate(piece, a, b, -1, 0);
                LineMovePlate(piece, a, b, 0, -1);
                break;
            case "black_pawn":
                BPawnMovePlate(a,b);
                break;
            case "white_pawn":
                WPawnMovePlate(a,b);
                break;
        }
    }
    public int Q = 0;
    public int IsOkeyDokey()
    {
        return controller.GetComponent<ChessBot>().Q;
    }
    public bool LineMovePlate(GameObject piece, int a, int b, int x, int y)
    {
        if (Q == 0)
        {
            int q = x, r = y;
            while (PositionOnBoard(q, r) && GetPosition(q, r) == null)
            {
                q += a;
                r += b;
                if (PositionOnBoard(q, r))
                {
                    PointMovePlate(x, y, q, r);
                }
            }
            if (PositionOnBoard(q, r) && PlayerColour(GetPosition(q, r)) != PlayerColour(piece))
            {
                Debug.Log(PlayerColour(GetPosition(q, r)) + " " + q + " " + r + " " + PlayerColour(piece));
                PointMovePlate(x, y, q, r);
            }
        }
        return false;
    }
    public void LMovePlate(int a,int b)
    {
        PointMovePlate(a + 1, b + 2, a, b);
        PointMovePlate(a - 1, b + 2, a, b);
        PointMovePlate(a + 2, b + 1, a, b);
        PointMovePlate(a - 2, b + 1, a, b);
        PointMovePlate(a - 2, b - 1, a, b);
        PointMovePlate(a - 1, b - 2, a, b);
        PointMovePlate(a + 1, b - 2, a, b);
        PointMovePlate(a + 2, b - 1, a, b);
    }
    public void SurroundMovePlate(int a, int b)
    {
        Game gm = controller.GetComponent<Game>();
        PointMovePlate(a, b - 1, a, b);
        PointMovePlate(a, b + 1, a, b);
        PointMovePlate(a + 1, b - 1, a, b);
        PointMovePlate(a + 1, b, a, b);
        PointMovePlate(a + 1, b + 1, a, b);
        PointMovePlate(a - 1, b - 1, a, b);
        PointMovePlate(a - 1, b, a, b);
        PointMovePlate(a - 1, b + 1, a, b);

    }

    public void WPawnMovePlate(int x, int y)
    {
        GameObject Piece = game.GetPosition(x,y);
        Game gm = controller.GetComponent<Game>();
        if (y == 1 && gm.PositionOnBoard(x, y + 1) && gm.PositionOnBoard(x, y + 2) && gm.GetPosition(x, y + 1) == null && gm.GetPosition(x, y + 2) == null)
        {
            MovePlateSpawn(Piece,x, y + 2, x, y);
        }

        if (gm.PositionOnBoard(x, y + 1))
        {
            if (gm.GetPosition(x, y + 1) == null)
            {
                MovePlateSpawn(Piece, x, y + 1, x, y);
            }
            else if (gm.PositionOnBoard(x + 1, y + 1) && gm.GetPosition(x + 1, y + 1) != null && gm.GetPosition(x + 1, y + 1).GetComponent<Chessman>().player != gm.GetPosition(x - 1, y - 1).GetComponent<Chessman>().player)
            {
                MovePlateAttackSpawn(gameObject, x + 1, y + 1, x, y);
            }
            else if (gm.PositionOnBoard(x - 1, y + 1) && gm.GetPosition(x - 1, y + 1) != null && gm.GetPosition(x - 1, y + 1).GetComponent<Chessman>().player != gm.GetPosition(x, y).GetComponent<Chessman>().player)
            {
                MovePlateAttackSpawn(gameObject, x - 1, y + 1, x, y);
            }
            else if (y == 4 && gm.PositionOnBoard(x + 1, y + 1) && GetEnPassant(x + 1, y, Piece))
            {
                EnPassant(x + 1, y + 1, x, y);

            }
            else if (y == 4 && gm.PositionOnBoard(x - 1, y + 1) && GetEnPassant(x - 1, y, Piece))
            {
                EnPassant(x - 1, y + 1, x, y);
            }
        }
    }
    public void BPawnMovePlate(int x, int y)
    {
        GameObject Piece = game.GetPosition(x,y);
        Game gm = controller.GetComponent<Game>();
        if (y == 6 && gm.PositionOnBoard(x, y - 1) && gm.PositionOnBoard(x, y - 2) && gm.GetPosition(x, y - 1) == null && gm.GetPosition(x, y - 2) == null)
        {
            MovePlateSpawn(Piece,x, y - 2, x, y);
        }
        if (gm.PositionOnBoard(x, y - 1))
        {
            if (gm.GetPosition(x, y - 1) == null)
            {
                MovePlateSpawn(Piece, x, y - 1, x, y);
            }
            else if (gm.PositionOnBoard(x - 1, y - 1) && gm.GetPosition(x - 1, y - 1) != null && gm.GetPosition(x - 1, y - 1).GetComponent<Chessman>().player != gm.GetPosition(x, y).GetComponent<Chessman>().player)
            {
                MovePlateAttackSpawn(gameObject, x - 1, y - 1, x, y);
            }
            else if (gm.PositionOnBoard(x + 1, y - 1) && gm.GetPosition(x + 1, y - 1) != null && gm.GetPosition(x + 1, y - 1).GetComponent<Chessman>().player != gm.GetPosition(x, y ).GetComponent<Chessman>().player)
            {
                MovePlateAttackSpawn(gameObject, x + 1, y - 1, x, y);
            }
            else if (y == 3 && gm.PositionOnBoard(x + 1, y - 1) && GetEnPassant(x + 1, y, Piece))
            {
                EnPassant(x + 1, y - 1, x, y);
            }
            else if (y == 3 && gm.PositionOnBoard(x - 1, y - 1) && GetEnPassant(x - 1, y, Piece))
            {
                EnPassant(x - 1, y - 1, x, y);
            }
        }
    }

    public void EnPassant(int matrixX, int matrixY, int X, int Y)
    {
        Debug.Log("why, just to suffer..");
        float x = matrixX;
        float y = matrixY;
        x *= 6.06f;
        y *= 6.06f;
        x -= 21.24f;
        y -= 21.24f;
        MoveThePlate mpScript = controller.GetComponent<MoveThePlate>();
        Game game = controller.GetComponent<Game>();
        Game.Move a = game.GetTheLastMove();
        mpScript.DMPawn = a.piece;
        mpScript.PwnTQn = false;
        mpScript.piece = game.GetPosition((int)Math.Round(x), (int)Math.Round( y));
        mpScript.enPassant = true;
        mpScript.attack = true;
        mpScript.IX = X;
        mpScript.IY = Y;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.MakeMove();
    }

    public bool GetEnPassant(int tox, int toy, GameObject obj)
    {
        Game game = controller.GetComponent<Game>();
        bool y = game.EnPassant;
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
        else if (y)
        {
            GameObject objj = game.positions[tox, toy];
            if (objj != null && game.EPassant == objj)
                return true;
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
            return true;
        return false;
    }
    public GameObject GetPosition(int x, int y)
    {
        game = GetComponent<Game>();
        return game.positions[x, y];
    }
    public int PlayerColour(GameObject piece)
    {

        switch (piece.name)
        {
            case "white_king":
            case "white_queen":
            case "white_knight":
            case "white_bishop":
            case "white_rook":
            case "white_pawn":
                return 1;
            default:
                return 0;
        }
    }
    public void PointMovePlate(int x, int y, int a, int b)
    {
        if (Q == 0)
        {
            Game gm = controller.GetComponent<Game>();

            if (gm.PositionOnBoard(x, y))
            {
                GameObject chp = gm.GetPosition(x, y);
                if (chp == null)
                {
                    MovePlateSpawn(gm.GetPosition(a, b), x, y, a, b);
                }

                else if (PlayerColour(gm.GetPosition(x, y)) != PlayerColour(gm.GetPosition(a, b)))
                {
                    MovePlateAttackSpawn(gm.GetPosition(a, b), x, y, a, b);

                }


            }
        }
    }
    public void MovePlateSpawn(GameObject piece,int matrixX, int matrixY, int i, int j)
    {
        GameObject Piece = piece;
        LegalMovesManager =controller.GetComponent<LegalMovesManager>();
       if (Q==0 && LegalMovesManager.IsLegal(Piece, i, j, matrixX,matrixY))
        {
          
            MoveThePlate mpScript = controller.GetComponent<MoveThePlate>();
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
            /*
            if (castling)
            {
                mpScript.castling = true;
                mpScript.Rook = Rook;
                mpScript.xr = XR;
                mpScript.yr = YR;
                mpScript.txr = TXR;
                mpScript.tyr = TYR;
            }
            */
            mpScript.IX = i;
            mpScript.IY = j;
            mpScript.SetReference(game.GetPosition(i,j));
            mpScript.SetCoords(matrixX, matrixY);
            // castling = false;
            controller.GetComponent<MoveThePlate>().MakeMove();
            controller.GetComponent<Game>().currentPlayer = "white";
            Debug.Log(matrixX + " " + matrixY);
            controller.GetComponent<ChessBot>().Q = 1;
            
            return;
        }
      
    }
    public void MovePlateAttackSpawn(GameObject piece,int matrixX, int matrixY, int i, int j)
    {
        GameObject Piece = piece;
        LegalMovesManager = controller.GetComponent<LegalMovesManager>();
        if (Q==0 && LegalMovesManager.IsLegal(Piece, i, j, matrixX, matrixY))
        {
            MoveThePlate mpScript = controller.GetComponent<MoveThePlate>();
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
            /*
            if (castling)
            {
                mpScript.castling = true;
                mpScript.Rook = Rook;
                mpScript.xr = XR;
                mpScript.yr = YR;
                mpScript.txr = TXR;
                mpScript.tyr = TYR;
            }
            */
            mpScript.IX = i;
            mpScript.IY = j;
            mpScript.SetReference(game.GetPosition(i, j));
            Debug.Log(matrixX + " " + matrixY);
            mpScript.SetCoords(matrixX, matrixY);
            // castling = false;
            controller.GetComponent<MoveThePlate>().MakeMove();
            controller.GetComponent<ChessBot>().Q = 1;
            return;
        }
    }

}
