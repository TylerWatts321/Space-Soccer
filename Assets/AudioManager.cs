using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioManagerChannels
{ 
    MusicChannel = 0,
    SoundEffectChannel,
    ThrusterChannel,
    SideThrusterChannel,
}


public class AudioManager : MonoBehaviour
{
    [Header ("Variables")]
    public static AudioManager Instance;

    public static float musicChannelVol = 1f;
    public static float soundeffectChannelVol = 1f;

    public AudioSource musicChannel;
    public AudioSource soundeffectChannel;
    public AudioSource thrusterChannel;
    public AudioSource sideThrusterChannel;

    [Header ("Audioclip List")]
    public AudioClip TitleMusic;
    public AudioClip bossMusic;
    public AudioClip mapMusic;
    public AudioClip restMusic;
    public AudioClip starMusic;
    public AudioClip asteroidMusic;
    public AudioClip deadMusic;
    public AudioClip defenseMusic;
    public AudioClip hiveMusic;
    public AudioClip introMusic;

    private HashSet<Collider2D> _collisions = new HashSet<Collider2D>();

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        musicChannel = GetComponents<AudioSource>()[0];
        soundeffectChannel = GetComponents<AudioSource>()[1];

        SceneManager.sceneLoaded += OnSceneLoaded;

        Instance.PlaySound(AudioManagerChannels.MusicChannel, TitleMusic, 1f);
    }

    public void Update()
    {
        _collisions.Clear();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MenuScene":
                Instance.PlaySound(AudioManagerChannels.MusicChannel, TitleMusic, 1f);
                break;
            case "StarSystem":
                {
                    Debug.Log("Play Audio");
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, starMusic, 1f);
                    break;
                }
            case "AsteroidBelt":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, asteroidMusic, 1f);
                    break;
                }
            case "DeadSystem":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, deadMusic, 1f);
                    break;
                }
            case "DefenseStationSystem":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, defenseMusic, 1f);
                    break;
                }
            case "HiveSystem":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, hiveMusic, 1f);
                    break;
                }
            case "MapScene":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, mapMusic, 1f);
                    break;
                }
            case "RestStation":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, restMusic, 1f);
                    break;
                }
            case "IntroScene":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, introMusic, 1f);
                    break;
                }
            case "CreditsScene":
                {
                    Instance.StopSound(AudioManagerChannels.ThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
                    Instance.StopSound(AudioManagerChannels.MusicChannel);
                    Instance.PlaySound(AudioManagerChannels.MusicChannel, introMusic, 1f);
                    break;
                }

        }
    }

    public void BossMusic()
    {
        Instance.StopSound(AudioManagerChannels.ThrusterChannel);
        Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
        Instance.StopSound(AudioManagerChannels.MusicChannel);
        Instance.PlaySound(AudioManagerChannels.MusicChannel, bossMusic, 1f);
    }

    public static void SetChannelVolume(int target, float value)
    {
        switch (target)
        {
            case 0:
                musicChannelVol = value;
                Instance.musicChannel.volume = musicChannelVol;
                break;
            case 1:
                soundeffectChannelVol = value;
                Instance.soundeffectChannel.volume = soundeffectChannelVol;
                break;
            default:
                break;
        }
    }

    public void SetMusicLoop()
    {
        musicChannel.loop = !musicChannel.loop;
    }

    public void PlaySound(AudioManagerChannels target, AudioClip clip)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                musicChannel.clip = clip;
                musicChannel.Play();
                break;
            case AudioManagerChannels.SoundEffectChannel:
                soundeffectChannel.PlayOneShot(clip);
                break;
            case AudioManagerChannels.ThrusterChannel:

                if (!thrusterChannel.isPlaying)
                thrusterChannel.Play();
                break;
            case AudioManagerChannels.SideThrusterChannel:

                if (!sideThrusterChannel.isPlaying)
                    sideThrusterChannel.Play();
                break;
        }
    }

    public void PlaySound(AudioManagerChannels target, AudioClip clip, float pitch)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                musicChannel.clip = clip;
                musicChannel.pitch = pitch;
                musicChannel.Play();
                break;
            case AudioManagerChannels.SoundEffectChannel:
                //soundeffectChannel.Stop();
                //soundeffectChannel.clip = clip;
                soundeffectChannel.pitch = pitch;
                soundeffectChannel.PlayOneShot(clip);
                break;
        }
    }

    public void StopSound(AudioManagerChannels target)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                break;
            case AudioManagerChannels.SoundEffectChannel:
                soundeffectChannel.Stop();
                break;
            case AudioManagerChannels.ThrusterChannel:
                thrusterChannel.Stop();
                break;
            case AudioManagerChannels.SideThrusterChannel:
                sideThrusterChannel.Stop();
                break;
        }
    }

    public void PlayCollisionSound(Collider2D colA, Collider2D colB, AudioClip clip)
    {
        if (_collisions.Contains(colA) && _collisions.Contains(colB))
        {
            return;
        }

        _collisions.Add(colA);
        _collisions.Add(colB);

        Debug.Log("Played");
        PlaySound(AudioManagerChannels.SoundEffectChannel, clip, Random.Range(.8f, 1.2f));
    }

}
