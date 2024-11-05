using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Settings : MonoBehaviour
{
    public AudioMixer audioSource; 
    public InputField Username;
    public Button SetUsername;
    public Button Close;
    public Button OpenSettings;
    public Button Theme;
    public Toggle SetSound;
    public string Score;
    public string SetTheme;
    public Text SoundOn;
    public GameObject UI;
    public GameObject Themesc;
    public GameObject user;
    public string Master = "MusicVolume";


    public void Open()
    {

        if (PlayerPrefs.GetString("musicOn") == null || PlayerPrefs.GetString("musicOn") == "1") { 
        audioSource.SetFloat(Master, 1f);
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
        UI.gameObject.SetActive(true);
        Themesc.gameObject.SetActive(true);
        SetSound.gameObject.SetActive(true);
        SetUsername.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("username")==null)
        PlayerPrefs.SetString("username", "Tudor");
        Username.text = PlayerPrefs.GetString("username");
    }
    public void SetTheUsername()
    {
        PlayerPrefs.SetString("username", Username.text);
    }
    public void ChangeSound()
    {
        if (PlayerPrefs.GetString("musicOn") == "1")
        {
            audioSource.SetFloat(Master, -80f);
            PlayerPrefs.SetString("musicOn", "0");
            SetSound.isOn = false;
        }
        else
        {
            audioSource.SetFloat(Master, 1f);
            PlayerPrefs.SetString("musicOn", "1");
            SetSound.isOn = true;
        }
    }
    public void CloseSettings()
    {
        SoundOn.gameObject.SetActive(false);
        Username.gameObject.SetActive(false);
        SetSound.gameObject.SetActive(false);
        SetUsername.gameObject.SetActive(false);
        user.gameObject.SetActive(false);
        UI.gameObject.SetActive(false);
        Themesc.gameObject.SetActive(false);
        Close.gameObject.SetActive(false);
    }
}
