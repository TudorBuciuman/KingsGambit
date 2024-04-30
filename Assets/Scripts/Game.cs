using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class Game : MonoBehaviour
{
    public GameObject chesspiece;
    public Chessman csman;
    public string MyTurn = "a";
    public GameObject EPassant = null;
    public GameObject[,] positions = new GameObject[8, 8];
    public GameObject[] playerBlack = new GameObject[16];
    public GameObject[] playerWhite = new GameObject[16];
    public string currentPlayer = "white";
    public bool gameOver = false;
    public string winner;
    public int FullMoves = 0;
    public int MadeMoves = 0;
    public string Fen;
    public bool EnPassant = true;
    public bool WCastling = false;
    public bool BCastling = false;
    public bool wkCastling = false;
    public bool wqCastling = false;
    public bool bkCastling = false;
    public bool bqCastling = false;
    public List<Move> MoveHistory = new List<Move>();

    public float MyTime = 300;
    public float OppTime = 300;
    public Text MyTimeT;
    public Text OppTimeT;
    public Text MyName;
    public Text OppName;
    public Text WinnerT;
    public string OppNamet;
    public string MyNamet;

    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public void MuliplayerThings()
    {
        if (tag == "tag3")
        {
            GameObject[] controller = GameObject.FindGameObjectsWithTag("tag2");
            MyName.text = controller[0].GetComponent<Game>().MyNamet;
            OppName.text = controller[0].GetComponent<Game>().OppNamet;
            MyTimeT.gameObject.SetActive(true);
            OppName.gameObject.SetActive(true);
            MyName.gameObject.SetActive(true);
            OppTimeT.gameObject.SetActive(true);
            StartTheGame();
            StartCoroutine(ClockCoroutine());
        }
    }
    public IEnumerator ClockCoroutine()
    {
        while (IsGameOver())
        {
            yield return new WaitForSeconds(1f);
            if (MyTurn == currentPlayer)
            {
                MyTime -= 1f;
                MyTimeT.text = FormatTime(MyTime);
            }
            else
            {
                OppTime -= 1f;
                OppTimeT.text = FormatTime(OppTime);
            }
            if (MyTime <= 0)
            {
                gameOver = true;

                break;
            }
            else if (OppTime <= 0)
            {
                gameOver = true;

                break;
            }
        }
    }
    public void Awake()
    {
        if(tag=="GameController")
            StartTheGame();
    }
    public void StartTheGame()
    {
       
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    positions[i, j] = null;
                }
            }
            

            LoadThePositionFromFen(fen);


            for (int i = 0; i < 16 && playerBlack[i] != null; i++)
            {
                SetPosition(playerBlack[i]);
            }
            for (int i = 0; i < 16 && playerWhite[i] != null; i++)
            {
                SetPosition(playerWhite[i]);
            }
        }
    
    GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, 80), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetYBoard(y);
        cm.SetXBoard(x);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetEmptyPosition(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x > 7 || y > 7) return false;
        return true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void NextTurn()
    {

        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }

    }

    public void Winner()
    {
        NextTurn();
        winner = currentPlayer;
        WinnerT.text=currentPlayer+" has won";
        WinnerT.gameObject.SetActive(true);
        gameOver = true;
        PlayerPrefs.SetString("LastScene", "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    public void WinnerText(string player)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (OppNamet==player)
        winner = MyNamet;
        else 
        winner = OppNamet;
        WinnerT.text = currentPlayer + " has won";
        WinnerT.gameObject.SetActive(true);
        gameOver = true;
    }

    public void Draw()
    {
        WinnerT.text = "Draw";
        WinnerT.gameObject.SetActive(true);
        gameOver = true;
    }
    public string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", min, sec);
    }
    /*   public void Update()
       {
           if (IsGameOver())
           {
               Debug.Log("checkmate");  
               GameObject.FindGameObjectWithTag("Winner").GetComponent<Text>().enabled = true;
               GameObject.FindGameObjectWithTag("Winner").GetComponent<Text>().text = winner + " has won";
               GameObject.FindGameObjectWithTag("Restart").GetComponent<Text>().enabled = true;
               enabled=false;
           }
           if (IsGameOver() == true && Input.GetMouseButtonDown(0))
           {

               SceneManager.LoadScene("Chess board");
               gameOver = false;
           }
       }
    */
    [System.Serializable]
    public class Move
    {
        public GameObject piece;
        public int fromX;
        public int fromY;
        public int toX;
        public int toY;

        public Move(GameObject piece, int fromX, int fromY, int toX, int toY)
        {
            this.piece = piece;
            this.fromX = fromX;
            this.fromY = fromY;
            this.toX = toX;
            this.toY = toY;
        }
    }

    public void RecordTheMove(GameObject piece, int fromX, int fromY, int toX, int toY)
    {
        Move move = new Move(piece, fromX, fromY, toX, toY);
        Chessman chessman = piece.GetComponent<Chessman>();
        MoveHistory.Add(move);
        if ((move.piece.name == "white_rook" && fromX == 0 && fromY == 0) || (toX == 0 && toY == 0))
        {
            chessman.wqCastling = false;
        }
        else if ((move.piece.name == "white_rook" && fromX == 7 && fromY == 0) || (toX == 7 && toY == 0))
        {
            chessman.wkCastling = false;
        }
        else if ((move.piece.name == "black_rook" && fromX == 0 && fromY == 7) || (toX == 0 && toY == 7))
        {
            chessman.bqCastling = false;
        }
        else if ((move.piece.name == "black_rook" && fromX == 7 && fromY == 7) || (toX == 7 && toY == 7))
        {
            chessman.bkCastling = false;
        }
        else if (move.piece.name == "black_king")
        {
            chessman.bCastling = false;
        }
        else if (move.piece.name == "white_king")
        {
            WCastling = false;
        }
    }

    public Move GetTheLastMove()
    {
        if (MoveHistory.Count > 0)
        {
            return MoveHistory[MoveHistory.Count - 1];
        }
        return null;
    }

    public List<GameObject> GetEnemyPieces()
    {
        List<GameObject> enemyPieces = new List<GameObject>();


        if (currentPlayer == "white")
        {
            foreach (GameObject piece in playerBlack)
            {
                if (piece != null)
                {
                    enemyPieces.Add(piece);
                }
            }
        }
        else
        {
            foreach (GameObject piece in playerWhite)
            {
                if (piece != null)
                {
                    enemyPieces.Add(piece);
                }
            }
        }
        return enemyPieces;
    }
    public List<GameObject> GetPlayerPieces()
    {
        int r = 0;
        List<GameObject> playerPieces = new List<GameObject>();
        if (currentPlayer == "white")
        {
            foreach (GameObject piece in playerWhite)
            {
                if (piece != null)
                {
                    r++;
                    playerPieces.Add(piece);
                }
            }
        }
        else
        {
            foreach (GameObject piece in playerBlack)
            {
                if (piece != null)
                {
                    playerPieces.Add(piece);
                }
            }
        }
        return playerPieces;
    }
    public void LoadThePositionFromFen(string fen)
    {
        GameObject controller = GameObject.FindGameObjectWithTag("tag2");

        if (PlayerPrefs.GetString("Continue") == "yes" && SceneManager.GetActiveScene().name!="Multiplayer lobby")
        {
            fen= PlayerPrefs.GetString("LastScene");
        }
        int y = fen.Length;
        int u = 0;
        int w = 0, b = 0;
        for (int i = 0; i <= 63; i++)
        {
            if (char.IsDigit(fen[u]))
            {
                i += (int)(char.GetNumericValue(fen[u])) - 1;
                u++;

            }
            else if ((int)(fen[u]) == '/')
            {
                u++;
                i--;
            }
            else if ((int)(fen[u]) == 'K')
            {
                playerWhite[w++] = Create("white_king", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'Q')
            {
                playerWhite[w++] = Create("white_queen", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'R')
            {
                playerWhite[w++] = Create("white_rook", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'B')
            {
                playerWhite[w++] = Create("white_bishop", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'N')
            {
                playerWhite[w++] = Create("white_knight", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'P')
            {
                playerWhite[w++] = Create("white_pawn", (i % 8), (7 - (i / 8)));
                u++;
            }

            else if ((int)(fen[u]) == 'k')
            {
                playerBlack[b++] = Create("black_king", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'q')
            {
                playerBlack[b++] = Create("black_queen", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'r')
            {
                playerBlack[b++] = Create("black_rook", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'b')
            {
                playerBlack[b++] = Create("black_bishop", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'n')
            {
                playerBlack[b++] = Create("black_knight", (i % 8), (7 - (i / 8)));
                u++;
            }
            else if ((int)(fen[u]) == 'p')
            { 
                playerBlack[b++] = Create("black_pawn", (i % 8), (7 - (i / 8) ) );
                u++;
            }

        }
        if (u < y)
        {
            if (fen[u] == ' ')
                u++;

            if ((int)(fen[u]) == 'b')
            {
                currentPlayer = "black";
            }
            u += 2;
            int ctl = 0;
            while (u < y)
            {
                if (ctl == 0 && (int)(fen[u]) == '-')
                {
                    ctl = 1;
                    if ((int)(fen[u]) == '-')
                    {
                        WCastling = false;
                        BCastling = false;
                        chesspiece.GetComponent<Chessman>().bCastling = false;
                        chesspiece.GetComponent<Chessman>().wCastling = false;
                    }
                    u++;
                }
                else if ((int)(fen[u]) == 'Q')
                {
                    WCastling = true;
                    chesspiece.GetComponent<Chessman>().wCastling = true;
                    wqCastling = true;
                    chesspiece.GetComponent<Chessman>().wqCastling = true;
                }
                else if ((int)(fen[u]) == 'K')
                {
                    WCastling = true;
                    chesspiece.GetComponent<Chessman>().wCastling = true;
                    wkCastling = true;
                    chesspiece.GetComponent<Chessman>().wkCastling = true;

                }
                else if ((int)(fen[u]) == 'q')
                {
                    BCastling = true;
                    chesspiece.GetComponent<Chessman>().bCastling = true;
                    bqCastling = true;
                    chesspiece.GetComponent<Chessman>().bqCastling = true;
                }
                else if ((int)(fen[u]) == 'k')
                {
                    BCastling = true;
                    chesspiece.GetComponent<Chessman>().bCastling = true;
                    bkCastling = true;
                    chesspiece.GetComponent<Chessman>().bkCastling = true;
                }
                else if ((int)(fen[u]) == ' ' && (int)(fen[u + 1]) == '-')
                {

                    EnPassant = false;
                }
                else if ((int)(fen[u]) == ' ')
                {
                    if (EnPassant)
                    {
                        u++;
                        int p = (int)(fen[u]) - (int)('a');
                        u++;
                        int q = (int)(fen[u]) - (int)('1');
                        EPassant = positions[p, q];

                    }

                    u++;
                    MadeMoves = 0;
                    while (u < y && (int)(fen[u]) != ' ')
                    {
                        MadeMoves = (int)(char.GetNumericValue(fen[u])) + MadeMoves * 10;
                        u++;
                    }
                    u++;
                    while (u < y && (int)(fen[u]) != ' ')
                    {
                        FullMoves = (int)(char.GetNumericValue(fen[u])) + FullMoves * 10;
                        u++;
                    }
                }


                u++;
            }
        }
        else
        {
            WCastling = true;
            BCastling = true;
            wkCastling = true;
            wqCastling = true;
            bkCastling = true;
            bqCastling = true;
        }
    }
    public void SaveFenBoard()
    {
        char[] TheFen = new char[64];
        int u = 0, emptySquareCount = 0;

        for (int j = 7; j >= 0; j--)
        {
            for (int i = 0; i <= 7; i++)
            {
                if (positions[i, j] != null)
                {
                    if (emptySquareCount > 0)
                    {
                        TheFen[u++] = (char)('0' + emptySquareCount);
                        emptySquareCount = 0;
                    }

                    switch (positions[i, j].name)
                    {
                        case "white_king":
                            TheFen[u++] = 'K';
                            break;
                        case "white_queen":
                            TheFen[u++] = 'Q';
                            break;
                        case "white_pawn":
                            TheFen[u++] = 'P';
                            break;
                        case "white_knight":
                            TheFen[u++] = 'N';
                            break;
                        case "white_rook":
                            TheFen[u++] = 'R';
                            break;
                        case "white_bishop":
                            TheFen[u++] = 'B';
                            break;
                        case "black_king":
                            TheFen[u++] = 'k';
                            break;
                        case "black_queen":
                            TheFen[u++] = 'q';
                            break;
                        case "black_pawn":
                            TheFen[u++] = 'p';
                            break;
                        case "black_knight":
                            TheFen[u++] = 'n';
                            break;
                        case "black_rook":
                            TheFen[u++] = 'r';
                            break;
                        case "black_bishop":
                            TheFen[u++] = 'b';
                            break;
                    }
                }
                else
                {
                    emptySquareCount++;
                }
            }

            if (emptySquareCount > 0)
            {
                TheFen[u++] = (char)('0' + emptySquareCount);
                emptySquareCount = 0;
            }

            if (j != 0)
            {
                TheFen[u++] = '/';
            }
        }

        string Fen = new string(TheFen);
        PlayerPrefs.SetString("LastScene",Fen);
        Debug.Log(Fen);
    }


    public bool GetFiftyMoveRule()
    {

        return (FullMoves == 50);

    }


}