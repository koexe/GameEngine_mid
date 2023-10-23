using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSceneMNG : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    // Start is called before the first frame update
    void Start()
    {
        string ScoreText = "";
        for(int i = 0; i< GameMNG.Instance.ScoreList.Count; i++)
        {
            string tempString = "";
            tempString += GameMNG.Instance.ScoreList[i].g_sName;
            tempString += " ";
            tempString += GameMNG.Instance.ScoreList[i].g_iScoreNum;
            tempString += "\n";
            ScoreText += tempString;
        }

        HighScoreText.text = ScoreText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
