using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtonScript : MonoBehaviour
{
    public int g_iSceneNum;
    private AudioClip m_AudioClip;


    private void Start()
    {
        m_AudioClip = Resources.Load<AudioClip>("Sound/Click");
    }

    public void Onclick(int SceneNum)
    {
        SoundMNG.Instance.PlaySound(m_AudioClip,1);
        GameMNG.Instance.ChangeScene(g_iSceneNum);
    }
}
