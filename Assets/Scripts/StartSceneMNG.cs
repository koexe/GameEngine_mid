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
        if (GameMNG.Instance.ScoreList.Count > 0)
        {
            Debug.Log(GameMNG.Instance.ScoreList.Count);
            for (int i = 0; i < 10; i++)
            {
                string tempString = "";
                tempString += GameMNG.Instance.ScoreList[i].g_sName;
                tempString += " ";
                tempString += GameMNG.Instance.ScoreList[i].g_iScoreNum;
                tempString += "\n";
                ScoreText += tempString;
            }
        }

        HighScoreText.text = ScoreText;
    }
}
