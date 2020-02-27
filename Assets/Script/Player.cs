using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 targetPosition = new Vector2(1, 1);
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;
    public bool isEnd = false;

    public float smoothing = 9;
    public float sleepTime = 0.5f;
    public float attackSleepTime = 0.5f;

    public float sleepTimer = 0;
    public float attackSleepTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        movePlayer();
    }

    private void movePlayer()
    {
        if (GameManager.food <= 0 || isEnd)
        {
            return;
        }

        //控制对象按照一定速度，向目标位置移动
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));

        //当停顿时间小于等待时间的时候，不去监听按键点击事件
        sleepTimer += Time.deltaTime;
        if (sleepTimer < sleepTime)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //每次只能向一个方向移动，优先向水平方向移动
        //如果不做判断同时按下水平和垂直的按钮，会同时向水平和垂直方向移动
        if (h > 0)
        {
            v = 0;
        }
        //当垂直或水平有按键的时候才去修改目标位置，同时重置按键监控休眠
        if (h != 0 || v != 0)
        {
            Vector2 vector = new Vector2(h, v);

            //防止碰撞到自身
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + vector);
            collider.enabled = true;

            if (hit.transform == null)
            {
                changePosition(targetPosition, vector, false);
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "Wall":
                        animator.SetTrigger("Attack");
                        hit.collider.SendMessage("TakeDamage"); //当碰撞物tag是Wall的时候向其发送消息
                        break;
                    case "OutWall": break;
                    case "Food":
                        GameManager.Instance().IncreaseFood(1, hit.collider.transform.gameObject);
                        targetPosition += vector;
                        break;
                    case "Soda":
                        GameManager.Instance().IncreaseFood(2, hit.collider.transform.gameObject);
                        targetPosition += vector;
                        break;
                    case "Enemy":
                        break;
                    case "Exit":
                        targetPosition += vector;
                        isEnd = true;
                        GameManager.Instance().LevelEnd();
                        break;
                }
            }
            GameManager.Instance().OnPlayerMove();
            sleepTimer = 0;
        }
    }

    private void changePosition(Vector2 targetPostion, Vector2 offset, bool isHitFood)
    {
        targetPosition += offset;
        if (!isHitFood)
        {
            GameManager.Instance().decreaseFood(1);
        }
    }

    public void TakeDamage(int lossFood)
    {
        animator.SetTrigger("Damaged");
        GameManager.Instance().decreaseFood(lossFood);
    }
}
