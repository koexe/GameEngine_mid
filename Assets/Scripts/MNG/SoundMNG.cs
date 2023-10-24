using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundMNG : MonoBehaviour
{
    private static SoundMNG _instance;
    private AudioSource m_AudioPlayer;
    private AudioSource[] m_AudioPlayers;
    private AudioClip m_BackGroundMusic;

    public enum Sound
    {
        BackGroundMusic,
        SoundEffect,
        MaxCount,
    }


    public static SoundMNG Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SoundMNG)) as SoundMNG;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //m_AudioPlayer = gameObject.transform.AddComponent<AudioSource>();
        m_BackGroundMusic = Resources.Load<AudioClip>("Sound/BGM");
        InitSoundPlayer();
        PlaySound(m_BackGroundMusic, 0);



        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


    }

    public void PlaySound(AudioClip audio, int SoundPlayerNum)
    {
        //m_AudioPlayer.clip = audio;
        //m_AudioPlayer.Play();

        m_AudioPlayers[SoundPlayerNum].clip = audio;
        m_AudioPlayers[SoundPlayerNum].Play();

    }

    private void InitSoundPlayer()
    {
        string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
        m_AudioPlayers = new AudioSource[soundNames.Length - 1];
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject(soundNames[i]);
            go.AddComponent<AudioSource>();
            go.transform.parent = gameObject.transform;
            m_AudioPlayers[i] = go.transform.GetComponent<AudioSource>();

        }
        m_AudioPlayers[(int)Sound.BackGroundMusic].loop = true;

    }
}
