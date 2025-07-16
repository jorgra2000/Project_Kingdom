using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> dayMusicList;
    [SerializeField] private List<AudioClip> nightMusicList;
    [SerializeField] private float crossfadeDuration = 3f;
    [SerializeField] private GameManager gameManager;

    private AudioSource activeSource;
    private AudioSource fadingSource;
    private bool isNightCached;

    void Awake()
    {
        activeSource = gameObject.AddComponent<AudioSource>();
        fadingSource = gameObject.AddComponent<AudioSource>();

        activeSource.loop = true;
        fadingSource.loop = true;
    }

    void Start()
    {
        isNightCached = gameManager.IsNight;
        PlayRandomMusic(isNightCached);
    }

    void Update()
    {
        if (gameManager.IsNight != isNightCached)
        {
            isNightCached = gameManager.IsNight;
            StartCoroutine(CrossfadeToNewTrack(isNightCached));
        }
    }

    void PlayRandomMusic(bool night)
    {
        List<AudioClip> playlist = night ? nightMusicList : dayMusicList;
        if (playlist.Count == 0) return;

        AudioClip clip = playlist[Random.Range(0, playlist.Count)];
        activeSource.clip = clip;
        activeSource.volume = 1f;
        activeSource.Play();
    }

    IEnumerator CrossfadeToNewTrack(bool toNight)
    {
        List<AudioClip> playlist = toNight ? nightMusicList : dayMusicList;
        if (playlist.Count == 0) yield break;

        AudioClip newClip = playlist[Random.Range(0, playlist.Count)];

        // Swap sources
        AudioSource temp = activeSource;
        activeSource = fadingSource;
        fadingSource = temp;

        activeSource.clip = newClip;
        activeSource.volume = 0f;
        activeSource.Play();

        float time = 0f;
        while (time < crossfadeDuration)
        {
            time += Time.deltaTime;
            float t = time / crossfadeDuration;
            activeSource.volume = Mathf.Lerp(0f, 1f, t);
            fadingSource.volume = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        fadingSource.Stop();
    }
}
