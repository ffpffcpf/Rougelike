using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private Vector2 targetPosition;
    private Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;
    public float smoothing = 1f;
    public int lossFood = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        GameManager.Instance().emenies.Add(this);
    }

    private void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
    }

    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if(offset.magnitude < 1.1f)
        {
            //进行攻击
            animator.SetTrigger("Attack");
            player.SendMessage("TakeDamage", lossFood);
        }
        else
        {
            float x = 0, y = 0;
            //根据x,y轴偏移量来控制敌人移动，先移动偏移量较大的
            if(Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                if(offset.y > 0)
                {
                    y = 1;
                } else
                {
                    y = -1;
                }
            }
            else
            {
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }

            //防止碰撞到自身
            Vector2 vector = new Vector2(x, y);
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + vector);
            collider.enabled = true;

            if(hit.transform == null)
            {
                targetPosition += vector;
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "Wall":
                        break;
                    case "OutWall": break;
                    case "Food":
                        targetPosition += vector;
                        break;
                    case "Soda":
                        targetPosition += vector;
                        break;
                    case "Player":
                        break;
                }
            }
        }
    }
}
