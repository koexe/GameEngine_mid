using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    private Dictionary<int,string> m_SceneDic;
    public string m_sScoreFilePath;
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
        string filename = "Score.csv";
        m_sScoreFilePath = Path.Combine(Application.persistentDataPath, filename);
        


        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        InitCsvData_mobile(m_sScoreFilePath);
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

    public void SaveCSV(Score score)
    {

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

            if (fields.Length >= 2)
            {
                Name = fields[0].Trim();
                ScoreNum = int.Parse(fields[1].Trim());
            }
            Score temp = new Score(Name, ScoreNum);

            ScoreList.Add(temp);

        }

        List<Score> newScoreList = ScoreList.OrderBy(s => s.g_iScoreNum).ToList();

    }

    public void InitCsvData_mobile(string filePath)
    {
        ScoreList = new List<Score>();
        if (!File.Exists(filePath))
        {
            string tempString = "HighScore,0\n";
            File.WriteAllText(filePath, tempString);
        }


        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length >= 0)
            {
                string Name = "";
                int ScoreNum = 0;

                for (int i = 0;i<lines.Length;i++)
                {
                    string[] data = lines[i].Split(',');
                    if (data.Length >= 2)
                    {
                        Name = data[0].Trim();
                        ScoreNum = int.Parse(data[1].Trim());
                    }
                    Score temp = new Score(Name, ScoreNum);
                    ScoreList.Add(temp);
                }
            }
            List<Score> newScoreList = ScoreList.OrderByDescending(s => s.g_iScoreNum).ToList();
            ScoreList = newScoreList;
        }

    }

    public void SaveCsvData_mobile(string name)
    {
        Debug.Log(m_sScoreFilePath);
        string tempString = name + "," + g_iScore + "\n";

        File.AppendAllText(m_sScoreFilePath, tempString);
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
