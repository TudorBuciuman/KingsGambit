using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
public class Game : MonoBehaviour
{
    public GameObject chesspiece;
    public GameObject[,] positions = new GameObject[8, 8];
    public GameObject[] playerBlack = new GameObject[16];
    public GameObject[] playerWhite = new GameObject[16];
    public string currentPlayer = "white";
    public bool gameOver = false;
    public string winner;
    public bool WCastling = true;
    public bool BCastling = true;
    public List<Move> MoveHistory = new List<Move>();

    public void Start()
    {

        playerWhite = new GameObject[]{
              Create("white_rook",0,0),Create("white_knight",1,0),Create("white_bishop",2,0),Create("white_queen",3,0),
              Create("white_king",4,0),Create("white_bishop",5,0),Create("white_knight",6,0),Create("white_rook",7,0),
              Create("white_pawn",0,1),Create("white_pawn",1,1),Create("white_pawn",2,1),Create("white_pawn",3,1),
              Create("white_pawn",4,1),Create("white_pawn",5,1),Create("white_pawn",6,1),Create("white_pawn",7,1)
      };

        playerBlack = new GameObject[]{
              Create("black_rook",0,7),Create("black_knight",1,7),Create("black_bishop",2,7),Create("black_queen",3,7),
              Create("black_king",4,7),Create("black_bishop",5,7),Create("black_knight",6,7),Create("black_rook",7,7),
              Create("black_pawn",0,6),Create("black_pawn",1,6),Create("black_pawn",2,6),Create("black_pawn",3,6),
              Create("black_pawn",4,6),Create("black_pawn",5,6),Create("black_pawn",6,6),Create("black_pawn",7,6)};
        for (int i = 0; i < 16; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);

        }
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 2; j <= 5; j++)
            {
                positions[i, j] = null;
            }
        }

    }
    GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
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

    public void Winner(string playerW)
    {
        winner = playerW;
        gameOver = true;
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
            chessman.wr00Castling = false;
        }
        else if ((move.piece.name == "white_rook" && fromX == 7 && fromY == 0) || (toX == 7 && toY == 0))
        {
            chessman.wr70Castling = false;
        }
        else if ((move.piece.name == "black_rook" && fromX == 0 && fromY == 7) || (toX == 0 && toY == 7))
        {
            chessman.br07Castling = false;
        }
        else if ((move.piece.name == "black_rook" && fromX == 7 && fromY == 7) || (toX == 7 && toY == 7))
        {
            chessman.br77Castling = false;
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
            enemyPieces.AddRange(playerBlack);
        }
        else
        {
            enemyPieces.AddRange(playerWhite);
        }

        return enemyPieces;
    }
    public List<GameObject> GetPlayerPieces()
    {
        List<GameObject> playerPieces = new List<GameObject>();
        if(currentPlayer == "white")
        {
            playerPieces.AddRange(playerWhite);
        }
        else
        {
            playerPieces.AddRange(playerBlack);
        }

        return playerPieces;
    }
    
}