using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndSceneMNG : MonoBehaviour
{
    public TextMeshProUGUI ResultScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        ResultScoreText.text = "Your Score : " + GameMNG.Instance.g_iScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
