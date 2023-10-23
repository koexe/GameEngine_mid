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
    
    // Start is called before the first frame update
    void Start()
    {
        ResultScoreText.text = "Your Score : " + GameMNG.Instance.g_iScore;
    }

    // Update is called once per frame

    public void WriteCSV(string name)
    {
        Debug.Log(name);
        string tempString = name+","+GameMNG.Instance.g_iScore + "\n";

        File.AppendAllText("Assets/Resources/CSV/Score.csv", tempString);

        
    }
}
