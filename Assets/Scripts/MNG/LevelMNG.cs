using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelMNG : MonoBehaviour
{
    public GameObject g_CubePrefab;
    private Coroutine m_crSpawnWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        bool[][][] CubePos = SetCubePos(12);
        SpawnCubes(CubePos);

        Debug.Log("Done!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool[][][] SetCubePos(int CubeAmount)
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
            while(true)
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
    public void SpawnCubes(bool[][][] CubePosArr)
    {
        GameObject Cube = g_CubePrefab;
        GameObject CubeSpawnField = GameObject.Find("CubeSpawnField");

        for (int z = 0;z < CubePosArr.Length;z++)
        {
            for (int y = 0; y<CubePosArr[z].Length;y++)
            {
                for (int x = 0; x < CubePosArr[y].Length;x++)
                {
                    if (CubePosArr[x][y][z] == true)
                    {
                        m_crSpawnWaitTime = StartCoroutine(WaitFunc());
                        Vector3 Pos = Cube.transform.localPosition;
                        Pos.x = x; Pos.y = y; Pos.z = z;
                        Cube.transform.localPosition = Pos;
                        Instantiate(Cube,CubeSpawnField.transform);
                    }                    
                }
            }
        }



    }
    IEnumerator WaitFunc()
    {
         yield return new WaitForSeconds(1.0f);
    }
}
