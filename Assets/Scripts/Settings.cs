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
    public Button SetSound;
    public string Score;
    public string SetTheme;
    public Text SoundOn;
    public GameObject UI;
    public GameObject Themesc;
    public GameObject user;
    public string Master = "MusicVolume";


    public void Open()
    {
        
        if (PlayerPrefs.GetString("musicOn") == null || PlayerPrefs.GetString("musicOn") == "1")
            audioSource.SetFloat(Master, 1f);
        else
            audioSource.SetFloat(Master, -80f);
        
        SoundOn.enabled = true;
        if (PlayerPrefs.GetString("musicOn") == "1" || PlayerPrefs.GetString("musicOn")=="")
            SoundOn.text = "Yes";
        else
            SoundOn.text="No";
       
        Close.gameObject.SetActive(true);
        Username.gameObject.SetActive(true);
        SetUsername.gameObject.SetActive(true);
        user.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        Themesc.gameObject.SetActive(true);
        
       // Debug.Log(SoundOn.text);
        SetSound.gameObject.SetActive(true);
        SetUsername.gameObject.SetActive(true);
        if (PlayerPrefs.GetString("username")==null)
        PlayerPrefs.SetString("username", "Tudor");
        Username.text = PlayerPrefs.GetString("username");
      //  SoundOn.text = PlayerPrefs.GetString("IsSound");
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
        }
        else
        {
            audioSource.SetFloat(Master, 1f);
            PlayerPrefs.SetString("musicOn", "1");
        }
        SoundOn.text = (PlayerPrefs.GetString("musicOn") == "1") ? "Yes" : "No";
    }
    public void CloseSettings()
    {
        Username.gameObject.SetActive(false);
      //  Theme.enabled = false;
        SetSound.gameObject.SetActive(false);
        SetUsername.gameObject.SetActive(false);
        user.gameObject.SetActive(false);
        UI.gameObject.SetActive(false);
        Themesc.gameObject.SetActive(false);
        Close.gameObject.SetActive(false);
        SoundOn.enabled = false;

    }
}
