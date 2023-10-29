using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Timers;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public bool isHeld = false;
    public bool Throw;//是否在投掷状态

    private Rigidbody2D rb;
    private Vector2 direction=new Vector2(1,0);
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void fire(Transform transform)
    {
        //确定投掷方向
        transform.rotation=Quaternion.LookRotation(Vector3.forward,direction);
        direction = transform.position - this.transform.position;
        direction.Normalize();
        
        Throw = true;

        //最多飞行4秒后销毁
        TimersManager.SetTimer(this, 4f, Destroy);

    }

    
    public void FixedUpdate()
    {
        if (Throw)
        {
            var tagPosition = (Vector2)transform.position + direction;
            
            //使用DOMove方法移动物体
            rb.DOMove(tagPosition, 3f).SetSpeedBased();
        }
    }
}
