using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelMNG : MonoBehaviour
{
    public GameObject g_CubePrefab;
    public GameObject g_ButtonPrefab;
    public TextMeshProUGUI m_AnswerText;
    public TextMeshProUGUI m_LevelText;
    public TextMeshProUGUI m_TimeText;
    public TextMeshProUGUI m_ScoreText;
    private Coroutine m_crSpawn;
    private Sprite[] m_ButtonSprites;
    public string g_sAnswer = "";
    private int m_iCubeAmount;
    private int m_iLevel = 0;
    private int m_iTime = 0;
    private int m_iScore = 0;


    // Start is called before the first frame update
    void Start()
    {
        InitLevel(m_iLevel);
        InitButton();
        InitText();
        


        Debug.Log("Init Task Done!");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_iTime++;
        m_AnswerText.text = g_sAnswer;
        m_TimeText.text = "Time:" + m_iTime;
    }


    public void CompareAnswer()
    {
        if (int.Parse(g_sAnswer) == m_iCubeAmount)
        {
            Debug.Log("Correct");
            if (m_iTime < 200)
                m_iScore += 3000;
            else if (m_iTime > 200 && m_iTime < 400)
                m_iScore += 5000 - (m_iTime * 10);
            else
                m_iScore += 1000;
            m_iLevel++;
            m_LevelText.text = "Level:" + (m_iLevel + 1);
            m_ScoreText.text = "Score:" + m_iScore;
            InitLevel(m_iLevel);
            if(m_iLevel == 11)
            {
                SceneManager.LoadScene("EndScene");
            }
        }
        else
        {
            if(m_iScore > 1000)
                m_iScore -= 1000;
            m_ScoreText.text = "Score:" + m_iScore;
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
        ButtonTemp.transform.GetComponent<SpriteRenderer>().sprite = m_ButtonSprites[0];
        ButtonTemp.transform.GetComponent<NumButtonScript>().ButtonNum = 0;
        ButtonTemp.transform.position = new Vector3(0 , -0.75f, 0);
        Instantiate(ButtonTemp, gameObject.transform);

    }
    private void InitLevel(int Level)
    {
        m_iTime = 0;
        GameObject[] CubeDestroy = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < CubeDestroy.Length; i++)
            Destroy(CubeDestroy[i]);

        m_iCubeAmount = m_iLevel + UnityEngine.Random.Range(1, 5);
        bool[][][] CubePos = SetCubePos(m_iCubeAmount);
        SpawnCubes(CubePos);
        
    }
    private void InitText()
    {
        GameObject AnserText = GameObject.Find("AnswerText");
        GameObject LevelText = GameObject.Find("LevelText");
        GameObject ScoreText = GameObject.Find("ScoreText");
        GameObject TimeText = GameObject.Find("TimeText");

        m_AnswerText = AnserText.transform.GetComponent<TextMeshProUGUI>();
        m_LevelText = LevelText.transform.GetComponent<TextMeshProUGUI>();
        m_ScoreText = ScoreText.transform.GetComponent<TextMeshProUGUI>();
        m_TimeText = TimeText.transform.GetComponent<TextMeshProUGUI>();

        m_LevelText.text = "Level:" + (m_iLevel + 1);
        m_ScoreText.text = "Score:" + m_iScore;
        m_TimeText.text = "Time:" + m_iTime;
    }
}
