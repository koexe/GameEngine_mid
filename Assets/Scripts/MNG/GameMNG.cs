using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    private Dictionary<int,string> m_SceneDic;
    public TextAsset ScoreCSV;
    public int g_iScore = 0;
    public List<Score> ScoreList;


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

        InitCsvData(ScoreCSV);
        m_SceneDic = new Dictionary<int, string>();
        m_SceneDic.Add(1, "StartScene");
        m_SceneDic.Add(2, "ExplainScene");
        m_SceneDic.Add(3, "LevelScene");
        m_SceneDic.Add(4, "EndScene");

    }

    public void ChangeScene(int SceneNum)
    {
        SceneManager.LoadScene(m_SceneDic[SceneNum]);
    }
    private void InitCsvData(TextAsset ScoreCSV)
    {
        ScoreList = new List<Score>();

        string[] lines = ScoreCSV.text.Split('\n');
        Debug.Log(lines.Length);
        for (int i = 0; i < lines.Length; i++)
        {
            string Name = "";
            int ScoreNum = 0;

            string line = lines[i];
            string[] fields = line.Split(',');
            Debug.Log(fields.Length);

            if (fields.Length >= 2)
            {
                Name = fields[0].Trim();
                Debug.Log(Name);
                ScoreNum = int.Parse(fields[1].Trim());
                Debug.Log(ScoreNum);
            }
            Score temp = new Score(Name, ScoreNum);

            ScoreList.Add(temp);

        }
        Debug.Log("ASDFa");
        List<Score> newScoreList = ScoreList.OrderBy(s => s.g_iScoreNum).ToList();

    }
    
    public class Score
    {
        public string g_sName;
        public int g_iScoreNum;

        public Score(string name, int score)
        {
            g_sName = name;
            g_iScoreNum = score;
        }
    }
}
