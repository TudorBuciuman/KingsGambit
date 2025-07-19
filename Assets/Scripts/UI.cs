using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject logo;
    public static bool pressed=false;
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
        }
    }
    public IEnumerator Wait()
    {
        while (true)
        {
            if (Input.touchCount!=0 || Input.anyKeyDown)
            {
                break;
            }
            yield return null;
        }
        pressed = true;
        logo.SetActive(false);
    }
}
