using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NumButtonScript : MonoBehaviour
{
    public int ButtonNum;

    private void OnMouseDown()
    {
        LevelMNG levelMNG = gameObject.transform.parent.GetComponent<LevelMNG>();
        if (levelMNG == null)
            Debug.Log("GameMNG Not Found");
        if (ButtonNum < 10)
        {
            levelMNG.g_sAnswer += ButtonNum;
        }
        if(ButtonNum == 11)
        {
            levelMNG.g_sAnswer = "";
        }
        if (ButtonNum == 10)
        {
            levelMNG.CompareAnswer();
        }
        //Debug.Log(ButtonNum);
    }
}
