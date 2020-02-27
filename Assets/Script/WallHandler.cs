using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandler : MonoBehaviour
{
    public int hp = 2;

    public Sprite damagedSprite;//当收到攻击时的图片

    //当受到攻击时
    public void TakeDamage()
    {
        hp -= 1;
        GetComponent<SpriteRenderer>().sprite = damagedSprite;
        if(hp<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
