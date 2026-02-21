using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class Settings : MonoBehaviour
{
    public AudioMixer audioSource; 
    public InputField Username;
    public Button SetUsername;
    public Button Close;
    public Button OpenSettings;
    public Button Theme;
    public Toggle SetSound;
    public Toggle SetTheme;
    public string Score;
    public Text SoundOn;
    public GameObject Ui;
    public GameObject Themesc;
    public GameObject user;
    public string Master = "MusicVolume";
    public bool isOpen = false;
    public bool wasDarkMode = false;
    public GameObject windarkmode;
    public bool canClose = true;
    public Image crownImg;
    public Text challenge;

    public void Open()
    {
        wasDarkMode = windarkmode.activeSelf;
        if (wasDarkMode)
            windarkmode.SetActive(false);
        if (!PlayerPrefs.HasKey("musicOn"))
        {
            PlayerPrefs.SetInt("musicOn", 0);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("musicOn") == 0) { 
        audioSource.SetFloat(Master, 0f);
        SetSound.isOn = true;
        }
        else
        {
            audioSource.SetFloat(Master, -80f);
            SetSound.isOn = false;
        }
        SoundOn.gameObject.SetActive(true);
        Close.gameObject.SetActive(true);
        Username.gameObject.SetActive(true);
        SetUsername.gameObject.SetActive(true);
        user.gameObject.SetActive(true);
        Ui.gameObject.SetActive(true);
        SetSound.gameObject.SetActive(true);
        SetUsername.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("username") == null)
            PlayerPrefs.SetString("username", "Tudor");
        Username.text = PlayerPrefs.GetString("username");
        if (UI.DarkMode)
        Ui.GetComponent<Image>().color = Color.black;
        if (PlayerPrefs.GetInt("darkMode") == 0)
        {
            Themesc.gameObject.SetActive(true);
            SetTheme.gameObject.SetActive(true);
        }
        else
        {
            Username.text = "Pawn";
            Username.interactable = false;
            SetUsername.gameObject.SetActive(false);
            challenge.gameObject.SetActive(true);
            challenge.text = "WAITING\r\nNoun: delay, stillness, anticipation\r\nAdjective: stagnant, unchanging, patient, inevitable\r\n\"The board was set, yet no hand moved.\r\nThe silence itself became the first move.\"";
        }

            isOpen = true;
    }
    public void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isOpen = false;
                CloseSettings();
            }
        }
    }
    public void SetTheUsername()
    {
        PlayerPrefs.SetString("username", Username.text);
        PlayerPrefs.Save();
    }
    public void ChangeSound()
    {
        if (PlayerPrefs.GetInt("musicOn") == 0)
        {
            audioSource.SetFloat(Master, -80f);
            PlayerPrefs.SetInt("musicOn", 1);
            SetSound.isOn = false;

        }
        else
        {
            audioSource.SetFloat(Master, 0f);
            PlayerPrefs.SetInt("musicOn", 0);
            SetSound.isOn = true;
        }
        PlayerPrefs.Save();
    }
    public void Protocol()
    {
        canClose = false;
        SetTheme.isOn = true;
        SetUsername.gameObject.SetActive(false);
        Username.interactable = false;
        PlayerPrefs.SetInt("darkMode", 1);
        PlayerPrefs.SetInt("lv", 1);
        Ui.GetComponent<Image>().color = Color.black;
        GameObject qwop = GameObject.FindGameObjectWithTag("tag2");
        Destroy(qwop);
        StartCoroutine(idk());
    }
    public IEnumerator idk()
    {
        yield return new WaitForSeconds(5);
        challenge.gameObject.SetActive(true);
        UI.pressed = false;
        yield return new WaitForSeconds(2);
        crownImg.gameObject.SetActive(true);
        StartCoroutine(Fade(10));
        StartCoroutine(FadeAlphaImg(crownImg, 6));
        yield return new WaitForSeconds(4);
        challenge.text += "\r\nBut beware—\r\neven waiting has a cost.";
        Username.text = "Pawn";
        PlayerPrefs.SetInt("Intro", 1);
        yield return new WaitForSeconds(5);
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
    public IEnumerator FadeAlphaImg(Image image, float time)
    {
        Color color = image.color;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsed = 0f;

        // Set initial alpha
        color.a = startAlpha;
        image.color = color;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / time);
            color.a = newAlpha;
            image.color = color;
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
    public void CloseSettings()
    {
        if (canClose)
        {
            SoundOn.gameObject.SetActive(false);
            Username.gameObject.SetActive(false);
            SetSound.gameObject.SetActive(false);
            SetTheme.gameObject.SetActive(false);
            SetUsername.gameObject.SetActive(false);
            user.gameObject.SetActive(false);
            Ui.gameObject.SetActive(false);
            Themesc.gameObject.SetActive(false);
            Close.gameObject.SetActive(false);
            if (wasDarkMode)
            {
                windarkmode.SetActive(true);
            }
        }
    }
    public void Reset()
    {
        StartCoroutine(resett());
    }
    public IEnumerator resett()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindAnyObjectByType<AudioSource>().Stop();
        yield return new WaitForSeconds(5);
        PlayerPrefs.SetInt("darkMode",0);
        PlayerPrefs.SetInt("lv",1);
        UI.pressed = false;
        GameObject qwop = GameObject.FindGameObjectWithTag("tag2");
        Destroy(qwop);
        SceneManager.LoadScene("Intro");
    }
}
