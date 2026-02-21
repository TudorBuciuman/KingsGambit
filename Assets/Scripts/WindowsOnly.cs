using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsOnly : MonoBehaviour
{
    void Awake()
    {
#if PLATFORM_STANDALONE_WIN
        this.gameObject.SetActive(false);
#endif
    }
}
