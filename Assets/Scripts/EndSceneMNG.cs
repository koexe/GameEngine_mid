using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System.Text;

public class EndSceneMNG : MonoBehaviour
{
    public TextMeshProUGUI ResultScoreText;
    public string g_sInputName;
    public int g_iSceneNum = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        ResultScoreText.text = "Your Score : " + GameMNG.Instance.g_iScore;
    }

    // Update is called once per frame

    public void WriteCSV(string name)
    {
        GameMNG.Instance.SaveCsvData_mobile(name);

        GameMNG.Instance.InitCsvData_mobile(GameMNG.Instance.m_sScoreFilePath);
        GameMNG.Instance.ChangeScene(g_iSceneNum);


    }
    public void ShowKeyBoard()
    {
        TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open("");
    }
}
