using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NumButtonScript : MonoBehaviour
{
    public int ButtonNum;
    private Coroutine m_crDestroyFunc;

    private void Start()
    {
        if(ButtonNum == 13)
        {
            m_crDestroyFunc = StartCoroutine(DestroyFunc());
        }
    }

    private void OnMouseDown()
    {
        LevelMNG levelMNG = gameObject.transform.parent.GetComponent<LevelMNG>();
        if (levelMNG == null)
            Debug.Log("GameMNG Not Found");
        if (ButtonNum < 10)
        {
            levelMNG.g_sAnswer += ButtonNum;
        }
        else if(ButtonNum == 11)
        {
            levelMNG.g_sAnswer = "";
        }
        else if (ButtonNum == 10)
        {
            levelMNG.CompareAnswer();
        }
        else
        {
            Debug.Log("NouButton");
        }
        //Debug.Log(ButtonNum);
    }


    private IEnumerator DestroyFunc()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
        StopCoroutine(m_crDestroyFunc);
    }
}
