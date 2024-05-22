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
        string puzzlefile = @"Assets/Resources/Puzzles.txt";
        System.Random systemRandom = new System.Random();
        int x=systemRandom.Next(2,18);
        string fen= File.ReadLines(puzzlefile).Skip(x- 1).FirstOrDefault();
        PlayerPrefs.SetString("Continue", "yes");
        PlayerPrefs.SetString("LastScene", fen);
        PlayerPrefs.SetString("Multiplayer", "no");
        SceneManager.LoadScene("Puzzle");
    }
}
