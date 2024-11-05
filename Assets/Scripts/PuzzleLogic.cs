using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;


public class PuzzleLogic : MonoBehaviour
{
    public void OpenRandom()
    {
        string puzzlefile = "Puzzles"; 
        TextAsset textAsset = Resources.Load<TextAsset>(puzzlefile);
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');

            System.Random systemRandomm = new System.Random();
            int u = systemRandomm.Next(0, lines.Length); 

            string fen = lines[u].Trim();
            PlayerPrefs.SetString("Continue", "yes");
            PlayerPrefs.SetString("LastScene", fen);
            PlayerPrefs.SetString("Multiplayer", "no");
            SceneManager.LoadScene("Puzzle");
        }
        
        
    }
}
