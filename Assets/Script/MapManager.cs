using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Transform mapHolder;
    private Transform roleHolder;

    public GameObject[] outWalls;
    public GameObject[] floors;
    public GameObject[] barriers;
    public GameObject[] enemies;
    public GameObject[] foods;
    public GameObject player;
    public GameObject exit;
    public List<Vector2> barrierVectors;

    public int rows = 10;
    public int column = 10;
    public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitMap()
    {
        mapHolder = new GameObject("Map").transform;
        roleHolder = new GameObject("Role").transform;
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if(x==0||y==0||x==column-1||y==rows-1)
                {
                    RandomInit(outWalls, x, y);
                } else
                {
                    RandomInit(floors, x, y); 
                }
            }
        }

        InitFoodEnemyBarrier();
        
    }


    //在特定坐标中生成gameObjects
    private void RandomInit(GameObject[] gameObjects, float x, float y)
    {
        int index = Random.Range(0, gameObjects.Length);
        GameObject gameObject = GameObject.Instantiate(gameObjects[index], new Vector3(x, y, 0), Quaternion.identity);
        gameObject.transform.SetParent(mapHolder);
    }

    //创建敌人食物障碍物
    private void InitFoodEnemyBarrier()
    {
        barrierVectors.Clear();
        for(int x = 2; x< column - 2; x++)
        {
            for(int y = 2; y < rows - 2; y++)
            {
                barrierVectors.Add(new Vector2(x, y));
            }
        }

        int foodNum = Random.Range(1, 4);
        int enemyNum = Random.Range(1, 4);
        int barrierNum = Random.Range(2, 9);


        RandomBatchInit(barrierVectors, barrierNum, barriers);
        RandomBatchInit(barrierVectors, enemyNum, enemies);
        RandomBatchInit(barrierVectors, foodNum, foods);
    }

    //在坐标中随机生成最大数量为limitNum的gameObjects
    private void RandomBatchInit(List<Vector2> barrierVectorList, int limitNum, GameObject[] gameObjects)
    {
        for(int i = 0; i < limitNum; i++)
        {
            int positionIndex = Random.Range(0, barrierVectorList.ToArray().Length + 1);
            Vector2 vector2 = barrierVectorList[positionIndex];
            barrierVectorList.RemoveAt(positionIndex);
            RandomInit(gameObjects, vector2.x, vector2.y);
        }
    }
}
