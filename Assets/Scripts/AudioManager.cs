using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool Music;
    public AudioSource audioSource;
    public static AudioManager manager;
    void Awake()
    {
        manager = this;
        if (Music && PlayerPrefs.GetInt("musicOn")==0)
        {
            StartCoroutine(LoadSongAsync(Random.Range(1, 3)));
        }
    }

    IEnumerator LoadSongAsync(int songName)
    {
        ResourceRequest request = Resources.LoadAsync<AudioClip>("Music/#" + songName);
        yield return request;

        AudioClip clip = request.asset as AudioClip;
        if (clip != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    public static void PlayGameOver()
    {
        Caller();
    }
    public static void Caller()
    {
        manager.StartCoroutine(manager.LoadSongAsync(66));
    }
}
