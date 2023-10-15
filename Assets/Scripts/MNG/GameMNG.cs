using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    private Dictionary<int,string> m_SceneDic;


    public static GameMNG Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameMNG)) as GameMNG;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        m_SceneDic = new Dictionary<int, string>();
        m_SceneDic.Add(1, "StartScene");
        m_SceneDic.Add(2, "ExplainScene");
        m_SceneDic.Add(3, "LevelScene");
        m_SceneDic.Add(4, "EndScene");

    }

    public void ChangeScene(int SceneNum)
    {
        SceneManager.LoadScene(m_SceneDic[SceneNum]);
        Debug.Log("asdf");


    }
}
