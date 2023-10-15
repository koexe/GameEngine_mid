using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelMNG : MonoBehaviour
{
    public GameObject g_CubePrefab;
    public GameObject g_ButtonPrefab;
    private TextMeshProUGUI m_AnswerText;
    private Coroutine m_crSpawn;
    private Sprite[] m_ButtonSprites;
    public string g_sAnswer;
    private int m_CubeAmount = 12;


    // Start is called before the first frame update
    void Start()
    {
        g_sAnswer = "";
        bool[][][] CubePosTemp = SetCubePos(12);
        SpawnCubes(CubePosTemp);
        InitButton();
        GameObject AnserText = GameObject.Find("AnswerText");
        if (AnserText == null)
            Debug.Log("NoAnserText");

        m_AnswerText = AnserText.transform.GetComponent<TextMeshProUGUI>();
        if (m_AnswerText == null)
            Debug.Log("NoTMP");

        Debug.Log("Done!");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_AnswerText.text = g_sAnswer;
    }


    public void CompareAnswer()
    {
        if (int.Parse(g_sAnswer) == m_CubeAmount)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("No");
        }
        g_sAnswer = "";
    }

    private bool[][][] SetCubePos(int CubeAmount)
    {
        if(CubeAmount > 48)
        {
            Debug.Log("큐브 개수가 너무 많습니다");
        }

        bool[][][] CubePos = new bool[4][][];
        for (int i = 0; i < 4; i++)
        {
            CubePos[i] = new bool[4][];
            for (int j = 0; j < 4; j++)
                CubePos[i][j] = new bool[4];
        }
        for (int i = 0;i < 4; i++)
            for(int j = 0;j < 4; j++)
                for (int k = 0; k < 4; k++)
                    CubePos[i][j][k] = false;
       
        int CubeOtherFloor = CubeAmount / 3;
        int Cube1stFloor = CubeAmount - CubeOtherFloor;

        Debug.Log(Cube1stFloor);
        Debug.Log(CubeOtherFloor);

        
        for (int i = 0; i < Cube1stFloor; i++)
        {
            int x,y;
            while (true)
            {
                x = UnityEngine.Random.Range(0, 4);
                y = UnityEngine.Random.Range(0, 4);
                if (CubePos[x][y][0] == false)
                    break;
            }
            CubePos[x][y][0] = true;
        }
        
        for (int i = 0; i < CubeOtherFloor; i++)
        {
            int x, y ,z;
            while(true)
            {
                x = UnityEngine.Random.Range(0, 4);
                y = UnityEngine.Random.Range(0, 4);
                z = UnityEngine.Random.Range(1, 4);
                if(CubePos[x][y][z - 1] == true && CubePos[x][y][z] == false)
                    break;
            }
            CubePos[x][y][z] = true;
        }

        return CubePos;
    }
    private void SpawnCubes(bool[][][] CubePosArr)
    {
        GameObject Cube = g_CubePrefab;
        GameObject CubeSpawnField = GameObject.Find("CubeSpawnField");
        IEnumerator SpawnCube()
        {
            for (int z = 0; z < CubePosArr.Length; z++)
            {
                for (int y = 0; y < CubePosArr[z].Length; y++)
                {
                    for (int x = 0; x < CubePosArr[y].Length; x++)
                    {
                        if (CubePosArr[x][y][z] == true)
                        {
                            Vector3 Pos = Cube.transform.localPosition;
                            Pos.x = x; Pos.y = y; Pos.z = z;
                            Cube.transform.localPosition = Pos;
                            Instantiate(Cube, CubeSpawnField.transform);
                            yield return new WaitForSeconds(0.1f);
                        }
                    }
                }
            }
            StopCoroutine(m_crSpawn);
        }

        m_crSpawn = StartCoroutine(SpawnCube());

    }


    private void InitButton()
    {
        m_ButtonSprites = new Sprite[12];
        for (int i = 0; i < m_ButtonSprites.Length; i++)
        {
            Sprite tempSprite = Resources.Load<Sprite>("Image/" + i);
            if (tempSprite != null)
                m_ButtonSprites[i] = tempSprite;
            else
                Debug.Log("Sprite not Found");
        }
        Debug.Log("Sprite Init Done");
        GameObject ButtonTemp = g_ButtonPrefab;
        
        for (int i = 1;i < m_ButtonSprites.Length-2;i++)
        {
            ButtonTemp.transform.GetComponent<SpriteRenderer>().sprite = m_ButtonSprites[i];
            ButtonTemp.transform.GetComponent<NumButtonScript>().ButtonNum = i;
            ButtonTemp.transform.position = new Vector3((-1.5f + (((i-1) % 3)*1.5f)), ((2.5f - ((i-1) / 3))*1.5f),0);

            Instantiate(ButtonTemp, gameObject.transform);
        }
        for(int i = 10; i< m_ButtonSprites.Length; ++i)
        {
            ButtonTemp.transform.GetComponent<SpriteRenderer>().sprite = m_ButtonSprites[i];
            ButtonTemp.transform.GetComponent<NumButtonScript>().ButtonNum = i;
            ButtonTemp.transform.position = new Vector3(-2.0f + (4.0f* (i-10)), -0.75f, 0);
            Instantiate(ButtonTemp, gameObject.transform);
        }
    }
}
