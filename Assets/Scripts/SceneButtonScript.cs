using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtonScript : MonoBehaviour
{
    public int g_iSceneNum;
    public void Onclick(int SceneNum)
    {
        GameMNG.Instance.ChangeScene(g_iSceneNum);
    }
}
