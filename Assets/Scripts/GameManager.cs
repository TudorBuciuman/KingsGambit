using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitApplication()
    {
        Application.Quit();
        Debug.Log("s-a inchis");
    }
}