using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("se incarca scena ");
        PlayerPrefs.SetString("Continue", "no");
        PlayerPrefs.SetString("Multiplayer", "no");
        SceneManager.LoadScene(sceneName);

    }

    public void LoadLastGame(string sceneName)
    {
        PlayerPrefs.SetString("Continue","yes");
        PlayerPrefs.SetString("Multiplayer", "no");
        SceneManager.LoadScene(sceneName);
    }
    public void SaveAndClose(string sceneName)
    {
      GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (PlayerPrefs.GetString("LastScene") != "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            controller.GetComponent<Game>().SaveFenBoard();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Debug.Log("s-a inchis");
        Application.Quit();
        
    }
}