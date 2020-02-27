using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static int level = 1;
    public static int food = 100;
    public List<Enemy> emenies = new List<Enemy>();
    private Text foodText;
    private Text gameOverText;
    private bool sleepStep = true;

    private Player player;
    private MapManager mapManager;

    public static GameManager Instance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        InitGame();
    }

    void InitGame()
    {
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        gameOverText.enabled = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mapManager = GetComponent<MapManager>();
    }

    private void UpdateFoodText()
    {
        foodText.text = "Food:" + food + " Level:" + level;
    }

    public void IncreaseFood(int count, GameObject needDestoryObject)
    {
        food += count;
        UpdateFoodText();
        Destroy(needDestoryObject);
    }

    public void decreaseFood(int count)
    {
        food -= count;
        UpdateFoodText();
        if (food <= 0)
        {
            gameOverText.enabled = true;
        }
    }

    public void OnPlayerMove()
    {
        if(sleepStep == true)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in emenies)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
    }

    public void LevelEnd()
    {
        //Application.LoadLevel(Application.loadedLevel); //重新加载本关卡
        level++;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        
    }
}
