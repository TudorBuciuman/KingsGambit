using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Image logo, TTG;
    public Image Wlogo, WTTG;
    public Text Text;
    public AudioClip boomsound;
    public AudioSource AudioSource;
    public Sprite corrupted;
    bool win = false;
    private void Awake()
    {
#if UNITY_STANDALONE_WIN
        win = true;
#endif
        if(PlayerPrefs.GetInt("darkMode") == 1)
        {
            TTG.sprite = corrupted;
            WTTG.sprite = corrupted;
        }
    }
    void Start()
    {
        StartCoroutine(PlayOnSight());
        if (PlayerPrefs.GetInt("Intro") == 1)
        {
            StartCoroutine(Waitting());

        }
        else if (PlayerPrefs.GetInt("entered")!=0)
        {
            StartCoroutine(BootUp());
        }
    }
    private IEnumerator BootUp()
    {
        yield return new WaitForSeconds(6.3f);
        SceneManager.LoadScene("Game UI");
    }
    private IEnumerator Waitting()
    {
        yield return new WaitForSeconds(6.3f);
        SceneManager.LoadScene("Waiting");
    }
    private IEnumerator PlayOnSight()
    {
        yield return Waiting(2.3f);
        if (!win)
            TTG.gameObject.SetActive(true);
        else
            WTTG.gameObject.SetActive(true);
        yield return Waiting(2f);
        TTG.gameObject.SetActive(false);
        WTTG.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlaySound();
        yield return Waiting(2.6f);
        Text.text = "presents";
        Text.gameObject.SetActive(true);
        yield return Waiting(2f);
        Text.gameObject.SetActive(false);
        yield return Waiting(2.5f);
        if (!win)
            logo.gameObject.SetActive(true);
        else
            Wlogo.gameObject.SetActive(true);
        yield return Waiting(4f);
        logo.gameObject.SetActive(false);
        Wlogo.gameObject.SetActive(false);
        PlaySound();
        yield return Waiting(2f);
        Text.text = "made by B.Tudor";
        Text.gameObject.SetActive(true);
        yield return Waiting(2.5f);
        Text.gameObject.SetActive(false);
        PlaySound();
        PlayerPrefs.SetInt("entered", 1);
        PlayerPrefs.Save();
        yield return StartCoroutine(BootUp());
    }

    private IEnumerator Waiting(float n)
    {
        yield return new WaitForSeconds(n);
    }

    public void PlaySound()
    {
        AudioSource.clip = boomsound;
        AudioSource.Play();
    }
}
