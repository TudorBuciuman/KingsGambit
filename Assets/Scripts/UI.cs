using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject logo,windowsLogo;
    public static bool pressed=false;
    public static bool win = false;
    public static bool DarkMode=false;

    public GameObject camera;

    public Text lv;
    public Text location;
    public Text score;
    public GameObject DarkModeObj;
    public AudioClip clip1,clip2;
    public AudioSource audioSource;
    public void Awake()
    {
#if UNITY_STANDALONE_WIN
        windowsLogo.SetActive(true);
        logo.SetActive(false);
        win = true;
#endif
        //PlayerPrefs.SetInt("darkMode",1);
        DarkMode = PlayerPrefs.GetInt("darkMode")==1;
        camera.GetComponent<Camera>().backgroundColor = Color.black;

        if (DarkMode)
        {
            Protocol();
            audioSource.clip = clip2;
            audioSource.Play();
        }
    }
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (!pressed)
        {
            StartCoroutine(Wait());
        }
        else
        {
            logo.SetActive(false);
            windowsLogo.SetActive(false);
        }
    }
    public IEnumerator Wait()
    {
        while (true)
        {
            if (win && (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)))
            {
                break;
            }
            else if (! win && (Input.touchCount!=0 || Input.anyKeyDown))
            {
                break;
            }
            yield return null;
        }
        pressed = true;
        logo.SetActive(false);
        windowsLogo.SetActive(false);
    }
    public void Protocol()
    {
        DarkModeObj.SetActive(true);
        int Lv = PlayerPrefs.GetInt("lv");
        if (Lv == 0)
        {
            Lv = 1;
            PlayerPrefs.GetInt("lv", 1);
        }
        lv.text = "LV "+Lv;
        score.text = Lv.ToString() + ":0";
        if (Lv < 5)
        {
            location.text = "Fighting";
        }
        else if (Lv < 10)
        {
            location.text = "Rising";
        }
        else if (Lv < 15)
        {
            location.text = "Withering";
        }
        else if (Lv < 20)
        {
            location.text = "The End";
        }
        else
        {
            location.text = "endgame";
        }

    }
}
