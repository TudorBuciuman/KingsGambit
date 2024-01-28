using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThePlate : MonoBehaviour
{
    public GameObject controller;
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

    public AudioSource source;
    public AudioClip move,moveattack,check;
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
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            
        }

    }
    public void OnMouseUp()
    {
      
        {
            //se activeaza din chessman si dupa start 
            //reprezinta piesa pe care o mut si operatiile de stergere
            controller = GameObject.FindGameObjectWithTag("GameController");
            source.Play();
            if (attack == true)
            {
                //  source.clip=moveattack; 
                // source.Play();
               
                GameObject chp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
                if (enPassant)
                {
                    chp = DMPawn;
                }
                if (chp.name == "white_king")
                {
                    controller.GetComponent<Game>().Winner("Black player");
                }
                else if (chp.name == "black_king")
                {
                    controller.GetComponent<Game>().Winner("White player");
                }
                Destroy(chp);
                attack = false;
            }
            else
            {
               source.clip = move;
                gameObject.SetActive(true);
               source.Play();
            }

            controller.GetComponent<Game>().SetEmptyPosition(reference.GetComponent<Chessman>().GetXBoard(), reference.GetComponent<Chessman>().GetYBoard());

            reference.GetComponent<Chessman>().SetXBoard(matrixX);
            reference.GetComponent<Chessman>().SetYBoard(matrixY);
            reference.GetComponent<Chessman>().SetCoords();

            controller.GetComponent<Game>().SetPosition(reference);
            controller.GetComponent<Game>().NextTurn();
            reference.GetComponent<Chessman>().DestroyMovePlates();
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
            controller.GetComponent<Game>().RecordTheMove(piece, IX, IY, reference.GetComponent<Chessman>().GetXBoard(), reference.GetComponent<Chessman>().GetYBoard());
        }
    }
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference() { 
        return reference; 
    }

   


}