using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Video;


public enum MoveState
{
    stay,
    move,
    rush
}
public class PlayerControl : MonoBehaviour
{

    public Transform target; // 设置目标对象

    public float moveSpeed = 5f; 
    public float forceMultiplier = 30f; 

    private float moveSpeedRation;//移动速度衰减
    private float turnSpeedRation;//转向速度衰减
    
    //控制动画
    private float setRushtime = 0.9f;
    private float rushtime;
    
    private string m_Horizontal;
    private string m_Vertical;

    public float horizontalInput;
    public float verticalInput;
    
    public MoveState moveState;

    private Function _Function;
    private Rigidbody2D rb2D;

    public GameObject Sprite;

    public Animator animator;

    public Vector2 LastMoveDirection=new Vector2();
    public bool canMove;
    
    
    

    private void Awake()
    {        
        _Function = GetComponent<Function>();
        rb2D = GetComponent<Rigidbody2D>();

        moveSpeedRation = 1;
        turnSpeedRation = 1;
        rushtime = setRushtime;

        canMove = true;
    }

    void Start()
    {
        m_Horizontal = "Horizontal" + _Function.Playernumber;
        m_Vertical = "Vertical" + _Function.Playernumber;
    }

  
    private void FixedUpdate()
    {
        switch (moveState)
        {
            case MoveState.stay:
                animator.SetBool("ifMove",false);
                animator.SetBool("ifRush",false);
                break;
            case MoveState.move:
                animator.SetBool("ifMove",true);
                break;
            case MoveState.rush:
                animator.SetBool("ifRush",true);
                break;
            default:
                break;
        }

        if (animator.GetBool("ifRush"))
        {
            if (rushtime>0)
            {
                rushtime -= Time.fixedDeltaTime;
            }
            else
            {
                rushtime = setRushtime;
                animator.SetBool("ifRush",false);
            }
        }
        
        
        
        horizontalInput = Input.GetAxis(m_Horizontal);
        verticalInput = Input.GetAxis(m_Vertical);

        int temp = (int)(rb2D.mass) / 3;
        moveSpeedRation = 1.0f / (( temp+ 1)*1.0f);
        turnSpeedRation = 1.0f / ((temp + 1) * 1.0f);
        
        if (verticalInput!=0||horizontalInput!=0)
        {
            moveState = MoveState.move;
        }
        else
        {
            moveState = MoveState.stay;
        }
        
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize(); // 防止对角线移动时速度变快
        if (moveState == MoveState.move)
        { 
            LastMoveDirection = movement;
        }
        // 使用AddForce方法
        _Function.rb.AddForce(movement * forceMultiplier);
        //_Function.rb.velocity=(movement * moveSpeed);

        Sprite.transform.up = Vector3.up;
    }

    


    private void Update()
    {
        /*
        horizontalInput = Input.GetAxis(m_Horizontal);
        verticalInput = Input.GetAxis(m_Vertical);

        int temp = (int)(rb2D.mass) / 3;
        moveSpeedRation = 1.0f / (( temp+ 1)*1.0f);
        turnSpeedRation = 1.0f / ((temp + 1) * 1.0f);
        
        if (verticalInput!=0||horizontalInput!=0)
        {
            moveState = MoveState.move;
        }
        else
        {
            moveState = MoveState.stay;
        }
        
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize(); // 防止对角线移动时速度变快
        if (moveState == MoveState.move)
        { 
            LastMoveDirection = movement;
        }
        // 使用AddForce方法
        _Function.rb.AddForce(movement * forceMultiplier);
        //_Function.rb.velocity=(movement * moveSpeed);

        Sprite.transform.up = Vector3.up;

        */
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // 1. 计算方向向量
            Vector2 direction = target.position - transform.position;

            // 2. 计算角度
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 3. 旋转玩家
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}