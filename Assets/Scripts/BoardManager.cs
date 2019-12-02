using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int i = 1; i < columns - 1; i++)
            for (int j = 1; j < rows - 1; j++)
                gridPositions.Add(new Vector3(i, j, 0f));
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int i = -1; i < columns + 1; i++)
            for (int j = -1; j < rows + 1; j++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (i == -1 || i == columns || j == -1 || j == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoise = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoise, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(tileArray: wallTiles, min: wallCount.minimum, max: wallCount.maximum);
        LayoutObjectAtRandom(tileArray: foodTiles, min: foodCount.minimum, max: foodCount.maximum);
        int enemyCount = (int)Math.Log(level, 2f);
        LayoutObjectAtRandom(tileArray: enemyTiles, min: enemyCount, max: enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
