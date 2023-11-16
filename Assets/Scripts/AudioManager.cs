using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;
    public AudioSource musicPlayer;
    public AudioSource soundEffectPlayer;
    public List<AudioClip> audioClips;

    public GameObject audioMenu;
    public Text nowPlayingText;
    public List<GameObject> SEHistoryEntries;
    public List<AudioClip> SEHistorySEs;
    public List<Text> SEHistoryNames;
    public List<Text> SEHistoryGames;
    public Slider musicVolumeSlider;
    public Slider SEVolumeSlider;

    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetAudioClipByName(string clipName)
    {
        foreach (AudioClip audioClip in audioClips)
        {
            if (audioClip.name == clipName)
            {
                return audioClip;
            }
        }

        return null;
    }

    public void PlayMusic(string clipName, float startTime = 0)
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.Stop();
        }
        musicPlayer.clip = GetAudioClipByName(clipName);
        musicPlayer.time = startTime;
        musicPlayer.Play();
        nowPlayingText.text = clipName;
    }

    public void StopMusic()
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.Stop();
        }
        nowPlayingText.text = "None";
    }

    public void PlaySoundEffect(string clipName)
    {
        if (soundEffectPlayer.isPlaying)
        {
            soundEffectPlayer.Stop();
        }

        AudioClip audioClip = GetAudioClipByName(clipName);

        if (audioClip != null)
        {
            soundEffectPlayer.clip = audioClip;
            soundEffectPlayer.Play();
            AddToSoundEffectHistory(audioClip);
        }
        else
        {
            Debug.LogError(clipName + " could not be found.");
        }
    }

    public void StopSoundEffect(string clipName)
    {
        if (clipName != null)
        {
            if (soundEffectPlayer.clip.name == clipName)
            {
                soundEffectPlayer.Stop();
            }
        }
        else
        {
            if (soundEffectPlayer.isPlaying)
            {
                soundEffectPlayer.Stop();
            }
        }
    }

    public void AddToSoundEffectHistory(AudioClip soundEffect)
    {
        while (SEHistorySEs.Count >= 5)
        {
            SEHistorySEs.RemoveAt(4);
        }

        SEHistorySEs.Insert(0, soundEffect);

        for (int i = 0; i < SEHistorySEs.Count; i++)
        {
            SEHistoryEntries[i].SetActive(true);
            string[] audioClipDetails = SEHistorySEs[i].name.Split('-');
            SEHistoryNames[i].text = audioClipDetails[0];
            SEHistoryGames[i].text = audioClipDetails[1];
        }
    }

    public void ReplaySoundEffect(int idx)
    {
        if (soundEffectPlayer.isPlaying)
        {
            soundEffectPlayer.Stop();
        }

        soundEffectPlayer.clip = SEHistorySEs[idx];
        soundEffectPlayer.Play();
    }

    public void ToggleAudioMenu()
    {
        audioMenu.SetActive(!audioMenu.activeInHierarchy);
    }

    public void AdjustMusicVolume()
    {
        musicPlayer.volume = musicVolumeSlider.value;
    }

    public void AdjustSoundEffectVolume()
    {
        soundEffectPlayer.volume = SEVolumeSlider.value;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<AudioManager>() != null)
            return;

        Instantiate(Resources.Load("AudioManager"));
    }
}
