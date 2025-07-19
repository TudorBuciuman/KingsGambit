using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Image logo, TTG;
    public Text Text;
    public AudioClip boomsound;
    public AudioSource AudioSource;
    void Start()
    {
        StartCoroutine(PlayOnSight());
        if (PlayerPrefs.GetInt("entered")!=0)
        {
            StartCoroutine(BootUp());
        }
    }
    private IEnumerator BootUp()
    {
        yield return new WaitForSeconds(6.3f);
        SceneManager.LoadScene("Game UI");
    }
    private IEnumerator PlayOnSight()
    {
        yield return Waiting(2.3f);
        TTG.gameObject.SetActive(true);
        yield return Waiting(2f);
        TTG.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlaySound();
        yield return Waiting(2.6f);
        Text.text = "presents";
        Text.gameObject.SetActive(true);
        yield return Waiting(2f);
        Text.gameObject.SetActive(false);
        yield return Waiting(2.5f);
        logo.gameObject.SetActive(true);
        yield return Waiting(4f);
        logo.gameObject.SetActive(false);
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
