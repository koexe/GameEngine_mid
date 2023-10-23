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
    public GameObject g_ExpainPrefab;
    public TextMeshProUGUI m_AnswerText;
    public TextMeshProUGUI m_LevelText;
    public TextMeshProUGUI m_TimeText;
    public TextMeshProUGUI m_ScoreText;
    public TextMeshProUGUI m_HelpText;
    public Sprite[] g_AnswerSprites;
    private Coroutine m_crSpawn;
    private Sprite[] m_ButtonSprites;
    public string g_sAnswer = "";
    private int m_iCubeAmount;
    private int m_iLevel = 0;
    private int m_iTime = 0;
    private int m_iScore = 0;
    private bool[][][] CurrentCubePosArr;


    private bool m_GameState = false;

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
        if (m_GameState == true)
        {
            m_iTime++;
            m_AnswerText.text = g_sAnswer;
            m_TimeText.text = "Time:" + m_iTime;
        }
    }
    public void CompareAnswer()
    {
        Debug.Log(int.Parse(g_sAnswer));
        if (int.Parse(g_sAnswer) == m_iCubeAmount)
        {
            if (m_iLevel + 1 == 10)
            {
                GameMNG.Instance.g_iScore = m_iScore;
                SceneManager.LoadScene("EndScene");
            }
            Debug.Log("Correct");
            int plusScore;
            if (m_iTime < 200)
                plusScore = 3000;
            else if (m_iTime > 200 && m_iTime < 400)
                plusScore = 5000 - (m_iTime * 10);
            else
                plusScore = 1000;
            m_iScore += plusScore;

            m_iLevel++;
            m_LevelText.text = "Level:" + (m_iLevel + 1);
            m_ScoreText.text = "Score:" + m_iScore;


            ShowAnswerPopup(true, plusScore);
            //InitLevel(m_iLevel);
        }
        else
        {
            if(m_iScore > 1000)
                m_iScore -= 1000;
            m_ScoreText.text = "Score:" + m_iScore;
            ShowAnswerPopup(false,-1000);
            Debug.Log("No");
        }
        g_sAnswer = "";
    }
    public void ShowExplain()
    {
        GameObject ExplainTemp = g_ExpainPrefab;
        GameObject Temp = GameObject.FindGameObjectWithTag("UI");
        ExplainTemp.transform.position = new Vector3(-4.0f, 1.0f, -7.5f);
        ExplainTemp.transform.GetComponent<IdontWantAddScript>().UI = Temp;

        Instantiate(ExplainTemp,gameObject.transform);
        m_GameState = false;
        Temp.SetActive(false);
    }
    public void ShowHint()
    {
        bool[][][] CubePos = CurrentCubePosArr;
        int hidden = 0;
        for(int y = 0; y < CubePos.Length-1; y++)
            for(int z = 0; z < CubePos.Length-1; z++)
                for (int x = 0; x < CubePos.Length-1; x++)
                {
                    if(CubePos[x][z][y] == true)
                        if (CubePos[x + 1][z + 1][y + 1] == true)
                            hidden++;
                }

        TextMeshProUGUI HelpText =m_HelpText.GetComponent<TextMeshProUGUI>();

        HelpText.text = "숨겨진 큐브는 " + hidden + "개 입니다.";
    }
    public void setGameState()
    {
        m_GameState = true;
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

        //Debug.Log(Cube1stFloor);
        //Debug.Log(CubeOtherFloor);

        
        for (int i = 0; i < Cube1stFloor; i++)
        {
            int x,z;
            while (true)
            {
                x = UnityEngine.Random.Range(0, 4);
                z = UnityEngine.Random.Range(0, 4);
                if (CubePos[x][z][0] == false)
                    break;
            }
            CubePos[x][z][0] = true;
        }
        
        for (int i = 0; i < CubeOtherFloor; i++)
        {
            int x, y ,z;
            while(true)
            {
                x = UnityEngine.Random.Range(0, 4);
                z = UnityEngine.Random.Range(0, 4);
                y = UnityEngine.Random.Range(1, 4);
                if(CubePos[x][z][y - 1] == true && CubePos[x][z][y] == false)
                    break;
            }
            CubePos[x][z][y] = true;
        }

        return CubePos;
    }
    private void SpawnCubes(bool[][][] CubePosArr)
    {
        GameObject Cube = g_CubePrefab;
        GameObject CubeSpawnField = GameObject.Find("CubeSpawnField");
        //CubeScript cubeScript = g_CubePrefab.transform.GetComponent<CubeScript>();
        //0.1초를 기다리며 큐브를 생성하는 코루틴
        IEnumerator SpawnCube()
        {
            for (int y = 0; y < CubePosArr.Length; y++)
            {
                for (int z = 0; z < CubePosArr[y].Length; z++)
                {
                    for (int x = 0; x < CubePosArr[z].Length; x++)
                    {
                        if (CubePosArr[x][z][y] == true)
                        {
                            Vector3 Pos = Cube.transform.localPosition;
                            //cubeScript.g_v3TargetPos = Pos;
                            Pos.x = x; Pos.y = y + 1; Pos.z = z;
                            Cube.transform.localPosition = Pos;
                            Instantiate(Cube, CubeSpawnField.transform);
                            yield return new WaitForSeconds(0.1f);
                        }
                    }
                }
            }
            m_GameState = true;
            StopCoroutine(m_crSpawn);
        }
        m_crSpawn = StartCoroutine(SpawnCube());
    }
    private void InitButton()
    {
        //리소스 폴더에서 sprite load
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

        //0 ~ submit, Clear Button GameObject Instantiate
        GameObject ButtonTemp = g_ButtonPrefab;
        ButtonTemp.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        ButtonTemp.transform.GetChild(0).GetComponent<TextMeshPro>().text = "";

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
        //Level의 큐브를 삭제하고 level에 맞는 양의 큐브를 spawn하는 함수
        m_iTime = 0;
        m_GameState = false;
        GameObject[] CubeDestroy = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < CubeDestroy.Length; i++)
            Destroy(CubeDestroy[i]);

        m_iCubeAmount = m_iLevel + UnityEngine.Random.Range(3, 6);
        bool[][][] CubePos = SetCubePos(m_iCubeAmount);
        CurrentCubePosArr = CubePos;

        SpawnCubes(CubePos);        
    }
    private void InitText()
    {
        //게임 시작 시 정보 텍스트들을 초기화하는 함수
        GameObject AnserText = GameObject.Find("AnswerText");
        GameObject LevelText = GameObject.Find("LevelText");
        GameObject ScoreText = GameObject.Find("ScoreText");
        GameObject TimeText = GameObject.Find("TimeText");

        m_AnswerText = AnserText.transform.GetComponent<TextMeshProUGUI>();
        m_LevelText = LevelText.transform.GetComponent<TextMeshProUGUI>();
        m_ScoreText = ScoreText.transform.GetComponent<TextMeshProUGUI>();
        m_TimeText = TimeText.transform.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI HelpText = m_HelpText.GetComponent<TextMeshProUGUI>();

        m_LevelText.text = "Level:" + (m_iLevel + 1);
        m_ScoreText.text = "Score:" + m_iScore;
        m_TimeText.text = "Time:" + m_iTime;
        HelpText.text = "";
    }
    private void ShowAnswerPopup(bool isAnswer, int Score)
    {
        //정답/오답의 정보와 점수를 표시하는 코루틴
        Coroutine InitCoroutine = null;

        IEnumerator ShowPopup()
        {
            GameObject PopupPrefab = g_ButtonPrefab;

            PopupPrefab.transform.GetComponent<SpriteRenderer>().sprite = isAnswer ? g_AnswerSprites[0] : g_AnswerSprites[1];
            PopupPrefab.transform.GetComponent<NumButtonScript>().ButtonNum = 13;
            PopupPrefab.transform.position = new Vector3(2.0f, 4.5f, 10.0f);
            PopupPrefab.transform.tag = "PopUp";
            PopupPrefab.transform.rotation = Quaternion.Euler(36.0f, 136.0f, 0.0f);
            PopupPrefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = Score.ToString();
            
            Instantiate(PopupPrefab);

            yield return new WaitForSeconds(1.0f);
            if(isAnswer)
                InitLevel(m_iLevel);

            StopCoroutine(InitCoroutine);
        }
        InitCoroutine = StartCoroutine(ShowPopup());
    }
}
