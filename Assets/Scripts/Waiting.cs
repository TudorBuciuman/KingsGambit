using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waiting : MonoBehaviour
{
    public GameObject waitObj;
    public AudioClip clip;
    public AudioSource source;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        source.clip = clip;
        StartCoroutine(Firstt());
    }

    public IEnumerator Firstt()
    {
        //I'm out of names
        yield return new WaitForSeconds(5);
        source.Play();
        yield return new WaitForSeconds(105);
        yield return StartCoroutine(Fade(8));
        waitObj.SetActive(true);

        yield return new WaitForSeconds(15);

        waitObj.SetActive(false);
        PlayerPrefs.SetInt("Intro", 0);
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("Intro");
    }
    public IEnumerator Fade(float time)
    {
        AudioSource a = FindFirstObjectByType<AudioSource>();
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            a.volume = Mathf.Lerp(startAlpha, endAlpha, elapsed / time);
            yield return null;
        }

        a.volume = 0;
    }
}
