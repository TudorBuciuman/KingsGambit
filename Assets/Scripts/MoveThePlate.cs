using UnityEngine;


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
    public bool enpass =false;
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
    public AudioClip move, moveattack, check, enpassant;
    public float a, b;
    public void Start()
    {
       
        if (attack)
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
           
            GameObject chp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            if (chp != null)
            {
                ColorUtility.TryParseHtmlString("#BF2529", out Color myColor); 
                gameObject.GetComponent<SpriteRenderer>().color = myColor; 
            }
            else if (enPassant && chp==null)
            {
                ColorUtility.TryParseHtmlString("#FFB000", out Color mycol);
                gameObject.GetComponent<SpriteRenderer>().color = mycol;
            }
        }
    }

    public void PlaySound(AudioClip Clip)
    {
        GameObject surce = GameObject.FindGameObjectWithTag("source");
        source.enabled=true;
        if (surce.GetComponent<AudioSource>().clip != enpassant || !surce.GetComponent<AudioSource>().isPlaying)
        {
            surce.GetComponent<AudioSource>().clip = Clip;
            surce.GetComponent<AudioSource>().Play();
        }
    }

    public void OnMouseUp()
    {
        MakeMove();
        GameObject[] MM = GameObject.FindGameObjectsWithTag("tag2");
        if (MM.Length>1 || PlayerPrefs.GetString("Multiplayer")=="yes")
            controller.GetComponent<MultiplayerManager>().SendMove(matrixX, matrixY, IX, IY);
        else if(!legalMovesManager.GetComponent<Game>().IsGameOver())
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<ChessBot>().BotTurn();
        }

    }
    public void MakeMove() {
            controller = GameObject.FindGameObjectWithTag("GameController");
         
            if (attack == true)
            {
                GameObject chp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
                if (enPassant)
                {
                    chp = DMPawn;
                enabled = false;
                enpass = true;
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
           if (PwnTQn)
            {
                piece.GetComponent<Chessman>().PawnToQueen(piece);
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
        float boardSize = (2048f * Game.multiplier) / 100f;
        float squareSize = boardSize / 8f;
        float halfBoard = (boardSize / 2f) - (squareSize / 2f);

        if (legalMovesManager.IsKingInCheck())
            {
            a = controller.GetComponent<LegalMovesManager>().Xkposition; 
            b= controller.GetComponent<LegalMovesManager>().Ykposition;
            

            a = (a * squareSize) - halfBoard;
            b = (b * squareSize) - halfBoard;
            if (legalMovesManager.IsCheckmate())
                {
                controller.GetComponent<Game>().Winner();
                    legalMovesManager.SetGameOver();
                }
            checkl = true;
            PlaySound(check);
            }
         
        
         else if (legalMovesManager.GetComponent<LegalMovesManager>().IsStalemate() || controller.GetComponent<Game>().GetFiftyMoveRule())
            {

                legalMovesManager.SetGameOver();
                controller.GetComponent<Game>().Draw();
            }

            float x = (matrixX * squareSize) - halfBoard;
            float y = (matrixY * squareSize) - halfBoard; ;
            float X = (IX * squareSize) - halfBoard;
            float Y = (IY * squareSize) - halfBoard;

            GameObject[] lastMovePlat = GameObject.FindGameObjectsWithTag("lastMovePlate");
            for(int i=0; i<lastMovePlat.Length; i++)
            {
                Destroy(lastMovePlat[i]);
            }
            GameObject LMP=Instantiate(lastMovePlate, new Vector3(x, y, 80.5f), Quaternion.identity);
            LMP.transform.localScale = new Vector2(LMP.transform.localScale.x * Game.multiplier / 2.4f, LMP.transform.localScale.y * Game.multiplier / 2.4f);
            GameObject mp =Instantiate(lastMovePlate, new Vector3(X, Y, 80.5f), Quaternion.identity);
            mp.transform.localScale = new Vector2(mp.transform.localScale.x * Game.multiplier / 2.4f, mp.transform.localScale.y * Game.multiplier / 2.4f);
            if (checkl)
            {
                GameObject kng = Instantiate(lastMovePlate, new Vector3(a, b, 81), Quaternion.identity);
                kng.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0.0f, 1.0f);
                kng.transform.localScale = new Vector2(kng.transform.localScale.x * Game.multiplier / 2.4f, kng.transform.localScale.y * Game.multiplier / 2.4f);

            checkl = false;
            }
            mp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
            if (enpass)
            {
                PlaySound(enpassant);
                enpass = false;
            }
            else if (attack) {
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
        float boardSize = (2048f * Game.multiplier) / 100f; 
        float squareSize = boardSize / 8f;       
        float halfBoard = (boardSize / 2f) - (squareSize / 2f); 
        float x = (X * squareSize) - halfBoard;
        float y = (Y * squareSize) - halfBoard;

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
                if (controller.GetComponent<Game>().positions[a, b] == null)
                {

                    DMPawn = controller.GetComponent<Game>().positions[a, Y];

                    attack = true;
                    enPassant = true;
                }

            }
            else if (piece.name == "black_pawn" && Y == 3 && b == 2)
            {
                if (controller.GetComponent<Game>().positions[a, b] == null)
                {
                    DMPawn = controller.GetComponent<Game>().positions[a, Y];
                    attack = true;
                    enPassant = true;
                }
            }
        }

        else if (piece.name == "white_king" || piece.name == "black_king")
        {
            if (piece.name == "white_king" && X == 4 && Y == 0)
            {
                if (a == 2 && b == 0)
                {
                    Rook = controller.GetComponent<Game>().GetPosition(0, 0);
                    castling = true;
                    xr = 0;
                    yr = 0;
                    txr = 3;
                    tyr = 0;
                }
                else if (a == 6 && b == 0)
                {
                    Rook = controller.GetComponent<Game>().GetPosition(7, 0);
                    castling = true;
                    xr = 7;
                    yr = 0;
                    txr = 5;
                    tyr = 0;
                }
            }
            else if (piece.name == "black_king" && X == 4 && Y == 7)
            {
                if (a == 2 && b == 7)
                {                    Rook = controller.GetComponent<Game>().GetPosition(0, 7);
                    castling = true;
                    xr = 0;
                    yr = 7;
                    txr = 3;
                    tyr = 7;
                }
                else if (a == 6 && b == 7)
                {
                    Rook = controller.GetComponent<Game>().GetPosition(7, 7);
                    castling = true;
                    xr = 7;
                    yr = 7;
                    txr = 5;
                    tyr = 7;
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