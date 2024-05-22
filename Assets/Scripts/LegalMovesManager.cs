using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LegalMovesManager : MonoBehaviour
{
    public Game legalMovesManage;
    public Game game;
    public bool IsGameOver = false;

    public void Start()
    {
     ;
    }
    public void SetGameOver()
    {
        IsGameOver = true;
    }
    public bool GetGameOver()
    {
        return IsGameOver;
    }
    public bool IsLegal(GameObject piece,int x, int y, int a, int b)
    {
        int t = 0;
        legalMovesManage = GetComponent<Game>();
        List<GameObject> enemyPieces = legalMovesManage.GetEnemyPieces();
        if (GetPosition(a, b) != null && PlayerColour(GetPosition(a, b)) != PlayerColour(GetPosition(x, y)))
        {
            t = 1;
            lostpiece = GetComponent<Game>().positions[a, b];
            legalMovesManage.GetComponent<Game>().positions[a, b] = piece;
            legalMovesManage.GetComponent<Game>().positions[x, y] = null;
        }
        else if(GetPosition(a, b) == null)
        {
            legalMovesManage.GetComponent<Game>().positions[a, b] = piece;
            legalMovesManage.GetComponent<Game>().positions[x, y] = null;
        }
        foreach (GameObject enemyPiece in enemyPieces)
        {
            if (enemyPiece != null && enemyPiece!=lostpiece)
            {

                if (ThreatensKing(enemyPiece))
                {
                    if (t == 1)
                    {
                        legalMovesManage.GetComponent<Game>().positions[a, b] = lostpiece;
                        lostpiece=null;
                    }
                    else
                    {
                        legalMovesManage.GetComponent<Game>().positions[a, b] = null;

                    }
                    legalMovesManage.GetComponent<Game>().positions[x, y] = piece;
                    return false;
                }
               
            }
        }
        if (t == 1)
        {
            legalMovesManage.GetComponent<Game>().positions[a, b] = lostpiece;
        }
        else
            legalMovesManage.GetComponent<Game>().positions[a, b] = null;
        legalMovesManage.GetComponent<Game>().positions[x, y] = piece;
        lostpiece = null;
        return true;
    }
    public bool IsCheckmate()
    {
            return IsStalemate();

    }
    public bool IsStalemate()
    {
        legalMovesManage = GetComponent<Game>();
        List<GameObject> playerPieces = legalMovesManage.GetPlayerPieces();
        foreach (GameObject playerPiece in playerPieces)
        {
            if (playerPiece != null)
            {
                if (!MakeMove(playerPiece))
                {
                    Debug.Log(playerPiece);
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;

    }
    public bool IsKingInCheck()
    {
        legalMovesManage = GetComponent<Game>();
        List<GameObject> enemyPieces = legalMovesManage.GetEnemyPieces();
       
        foreach (GameObject enemyPiece in enemyPieces)
        {
            if (enemyPiece != null)
            {
                if (ThreatensKing(enemyPiece))
                {
                    SetCheckPieceFast(enemyPiece);
                    return true;
                }
            }
        }
        return false;
    }
    public bool ThreatensKing(GameObject piece)
    {
        int a = piece.GetComponent<Chessman>().GetXBoard();
        int b = piece.GetComponent<Chessman>().GetYBoard();
        switch (piece.name)
        {
            case "black_king":
            case "white_king":
                return (GoTo(a,b+1) || GoTo(a-1, b + 1) || GoTo(a, b - 1) || GoTo(a - 1, b - 1) || GoTo(a+1, b + 1) || GoTo(a+1, b) || GoTo(a+1, b -1) || GoTo(a-1, b));
            case "black_queen":
            case "white_queen":
                return (LineMovePlate(1, 0, a, b) ||
                 LineMovePlate(0, 1, a, b) ||
                 LineMovePlate(1, 1, a, b) ||
                 LineMovePlate(-1, 0, a, b) ||
                 LineMovePlate(0, -1, a, b) ||
                 LineMovePlate(-1, -1, a, b) ||
                 LineMovePlate(-1, 1, a, b) ||
                 LineMovePlate(1, -1, a, b));
            case "black_bishop":
            case "white_bishop":
                return (
                    LineMovePlate(1, 1, a, b) ||
                    LineMovePlate(-1, -1, a, b) ||
                    LineMovePlate(-1, 1, a, b) ||
                    LineMovePlate(1, -1, a, b));
            case "black_pawn":
                return (GoTo(a - 1, b - 1) || GoTo(a + 1, b - 1));
            case "white_pawn":
                return (GoTo(a - 1, b + 1) || GoTo(a + 1, b + 1));
            case "black_knight":
            case "white_knight":
                return (GoTo(a + 1, b + 2) || GoTo(a - 1, b + 2) || GoTo(a + 1, b - 2) || GoTo(a + 2, b - 1) || GoTo(a - 1, b - 2) || GoTo(a +2, b +1) || GoTo(a - 2, b + 1) || GoTo(a - 2, b - 1));
            case "black_rook":
                {
                    return (LineMovePlate(1, 0, a, b) ||
                     LineMovePlate(0, 1, a, b) ||
                     LineMovePlate(-1, 0, a, b) ||
                     LineMovePlate(0, -1, a, b));
                }
            case "white_rook":
                {
                    return (LineMovePlate(1, 0, a, b) ||
                    LineMovePlate(0, 1, a, b) ||
                    LineMovePlate(-1, 0, a, b) ||
                    LineMovePlate(0, -1, a, b));
                }
   
        }






        return false;



    }
    public bool LineMovePlate(int xI, int yI, int a, int b)
    {
        int x = a + xI;
        int y = b + yI;
        if (game.GetComponent<Game>().currentPlayer == "black")
        {
            while (PositionOnBoard(x, y) && GetPosition(x, y) == null)
            {
                x += xI;
                y += yI;
            }
            if (PositionOnBoard(x, y) && GetPosition(x, y).GetComponent<Chessman>().player == game.GetComponent<Game>().currentPlayer)
            {
                if (GetPosition(x, y).name == "black_king")
                {
                    Xkposition = x; Ykposition = y;
                    return true;
                }
            }
            return false;
        }
        else
        {
            while (PositionOnBoard(x, y) && GetPosition(x, y) == null)
            {
                x += xI;
                y += yI;

            }
            if (PositionOnBoard(x, y) && GetPosition(x, y).GetComponent<Chessman>().player == game.GetComponent<Game>().currentPlayer)
            {
                if (GetPosition(x, y).name == "white_king")
                {
                    Xkposition = x; Ykposition = y;
                    return true;
                }
            }

            return false;
        }
    }

    public bool GoTo(int x, int y)
    {
        if (PositionOnBoard(x,y) && GetPosition(x, y) != null)
        {
            if (game.GetComponent<Game>().currentPlayer == "black" && GetPosition(x, y).name == "black_king")
            {
                Xkposition = x; Ykposition=y;
                return true;
            }
            else if (game.GetComponent<Game>().currentPlayer == "white" && GetPosition(x, y).name == "white_king")
            {
                Xkposition = x; Ykposition = y;
                return true;
            }
            return false;
        }
        else
            return false;


    }
    public int Xkposition;
    public int Ykposition;
    public bool PositionOnBoard(int x, int y){
        if(x>=0 && x<=7 && y>=0 && y<=7)
        return true;
        return false;
}
    public GameObject GetPosition(int x, int y)
    {
        game=GetComponent<Game>();
        return game.positions[x, y];
    }


    public GameObject CheckPiece;
    public void SetCheckPieceFast(GameObject piece)
    {
       CheckPiece = piece;
    }
    public GameObject GetCheckPieceFast()
    {
        return CheckPiece;
    }
    public bool MakeMove(GameObject piece)
    {
        int a = piece.GetComponent<Chessman>().xBoard;
        int b = piece.GetComponent<Chessman>().yBoard;
        switch (piece.name)
        {
            case "black_king": 
            case "white_king":
                    return (TemporaryUpdate(piece, a, b, a, b - 1) || TemporaryUpdate(piece, a, b, a, b + 1) || TemporaryUpdate(piece, a, b, a - 1, b - 1) || TemporaryUpdate(piece, a, b, a - 1, b) ||
                            TemporaryUpdate(piece, a, b, a - 1, b + 1) || TemporaryUpdate(piece, a, b, a + 1, b - 1) || TemporaryUpdate(piece, a, b, a + 1, b) || TemporaryUpdate(piece, a, b, a + 1, b + 1)); 
            case "black_queen":
            case "white_queen":
                return (LMovePlate(piece,1, 0, a, b) ||
                 LMovePlate(piece, 0, 1, a, b) ||
                 LMovePlate(piece, 1, 1, a, b) ||
                 LMovePlate(piece,-1, 0, a, b) ||
                 LMovePlate(piece, 0, -1, a, b) ||
                 LMovePlate(piece, -1, -1, a, b) ||
                 LMovePlate(piece, -1, 1, a, b) ||
                 LMovePlate(piece, 1, -1, a, b));
            case "black_bishop":
            case "white_bishop":
                return (
                    LMovePlate(piece, 1, 1, a, b) ||
                    LMovePlate(piece, -1, -1, a, b) ||
                    LMovePlate(piece, -1, 1, a, b) ||
                    LMovePlate(piece, 1, -1, a, b));
            case "black_pawn":
                return ((TemporaryUpdate(piece,a,b,a - 1, b - 1) && legalMovesManage.GetComponent<Game>().positions[a-1, b - 1] != null) || (TemporaryUpdate(piece,a,b,a + 1, b - 1) && legalMovesManage.GetComponent<Game>().positions[a+1, b - 1] != null )|| TemporaryUpdate(piece,a,b,a,b-1) || (a==6 && legalMovesManage.GetComponent<Game>().positions[a, b-1]==null && TemporaryUpdate(piece,a,b,a,b-2)));
            case "white_pawn":
                return ((TemporaryUpdate(piece, a, b,a - 1, b + 1) && legalMovesManage.GetComponent<Game>().positions[a-1, b + 1] != null )|| (TemporaryUpdate(piece, a, b, a + 1, b + 1) && legalMovesManage.GetComponent<Game>().positions[a+1, b + 1] != null) || TemporaryUpdate(piece, a, b, a, b+1) || (a == 1 && legalMovesManage.GetComponent<Game>().positions[a,b+1] == null && TemporaryUpdate(piece, a, b, a, b+2)));
            case "black_knight":
            case "white_knight":
                return (TemporaryUpdate(piece, a, b, a + 1, b + 2) || TemporaryUpdate(piece, a, b, a - 1, b + 2) || TemporaryUpdate(piece, a, b, a + 1, b - 2) || TemporaryUpdate(piece, a, b, a + 2, b - 1) || TemporaryUpdate(piece, a, b, a - 1, b + 2) 
                    || TemporaryUpdate(piece, a, b, a - 1, b - 2) || TemporaryUpdate(piece, a, b, a - 2, b + 1) || TemporaryUpdate(piece, a, b, a - 2, b - 1));
            case "black_rook":
                {
                    return (LMovePlate(piece, 1, 0, a, b) ||
                     LMovePlate(piece, 0, 1, a, b) ||
                     LMovePlate(piece, -1, 0, a, b) ||
                     LMovePlate(piece, 0, -1, a, b));
                }
            case "white_rook":
                {
                    return (LMovePlate(piece, 1, 0, a, b) ||
                    LMovePlate(piece, 0, 1, a, b) ||
                    LMovePlate(piece, -1, 0, a, b) ||
                    LMovePlate(piece, 0, -1, a, b));
                }
            default: return false;

        }



    }

    public bool LMovePlate(GameObject piece, int a, int b, int x, int y)
    {
        
        int q = x, r = y;
        while (PositionOnBoard(q+a, r+b) && GetPosition(q+a, r+b) == null)
            {
                q += a;
                r += b;
                if (PositionOnBoard(q, r))
                {
                if (piece.name == "black_bishop")
                {
                    Debug.Log(q + " " + r);
                }
                if (TemporaryUpdate(piece, x, y, q, r))
                    return true;
                }
            }
            if (PositionOnBoard(q+a, r+b) && PlayerColour(GetPosition(q+a, r+b)) != PlayerColour(piece))
            {
            if (TemporaryUpdate(piece, x, y, q, r))
                 return true;
            }
            return false;
    }



    public GameObject lostpiece;
    public bool TemporaryUpdate(GameObject piece ,int x, int y, int a, int b)
    {
        int t=0;
        List<GameObject> enemyPieces = legalMovesManage.GetEnemyPieces();
        if (PositionOnBoard(a, b) && GetPosition(a, b) != null && PlayerColour(GetPosition(a, b)) != PlayerColour(GetPosition(x,y)))
        {
            t = 1;
            lostpiece = GetComponent<Game>().positions[a, b];
            legalMovesManage.GetComponent<Game>().positions[a, b] = piece;
            legalMovesManage.GetComponent<Game>().positions[x, y] = null;
           
        }
        else if (PositionOnBoard(a, b) && GetPosition(a, b) == null)
        {
            t = 2;
            legalMovesManage.GetComponent<Game>().positions[a, b] = piece;
            legalMovesManage.GetComponent<Game>().positions[x, y] = null;
        }
        if (piece.name == "black_bishop")
        {
            Debug.Log(a + " " + b );
        }
        if (PositionOnBoard(a, b) && (t==1 || t==2))
        {
            foreach (GameObject ePiece in enemyPieces)
            {
                if (ePiece != null && (t==2 ||ePiece!=lostpiece))
                {
                    if (ThreatensKing(ePiece))
                    {
                        legalMovesManage.GetComponent<Game>().positions[x, y] = piece;
                        if (t == 1) 
                        { 
                         legalMovesManage.GetComponent<Game>().positions[a, b] = lostpiece;
                        lostpiece = null;

                        }

                         else
                        legalMovesManage.GetComponent<Game>().positions[a, b] = null;
                        
                        return false;
                     }
                   
                   

                }
                
            }
            legalMovesManage.GetComponent<Game>().positions[x, y] = piece;
            if (t == 1)
            {
                legalMovesManage.GetComponent<Game>().positions[a, b] = lostpiece;
                lostpiece = null;
            }
            else
                legalMovesManage.GetComponent<Game>().positions[a, b] = null;
            Debug.Log(piece+" "+x+" "+y+" "+a+" "+b);
            return true;
        }
        else
        {
            return false;
        }
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

            }
        return 0;
    }

}
