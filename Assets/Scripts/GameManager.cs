using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("Continue", "no");
        PlayerPrefs.SetString("Multiplayer", "no");
        if (sceneName == "Game UI")
        {
            GameObject qwop = GameObject.FindGameObjectWithTag("tag2");
            Destroy(qwop);
        }
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
        if(PlayerPrefs.GetString("Multiplayer")=="no")
        controller.GetComponent<Game>().SaveFenBoard();
        if (sceneName == "Game UI")
        {
            GameObject qwop = GameObject.FindGameObjectWithTag("tag2");
            Destroy(qwop);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Debug.Log("s-a inchis");
        Application.Quit();
        
    }
}