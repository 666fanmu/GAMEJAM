using System;
using Timers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Function : MonoBehaviour
{ 
    [HideInInspector]
    public Rigidbody2D rb;//刚体
    private PlayerControl _playerControl;

    public int Playernumber;//玩家编号
    public bool canThrow;//是否能投掷

    public GameObject grabPosition;//抓取位置
    public GameObject FirePosition;//投掷方向

    public bool hasCollection;//是否抓取掉落物

    private Transform Child;

    private bool isDashing;
    [FormerlySerializedAs("DashCD")] public float RashCD;//冲刺CD
    public float setRushCD;
    [HideInInspector]
    public bool canDash;//是否能冲刺
    private float DashForce;//冲刺力度

    [FormerlySerializedAs("Dash")] public GameObject DashImage;

    public Sprite Dashimage;
    public Sprite DashCDimage;
    
    private string RushInput;
    private string EatInput;
    private string ThrowInput;

    public float AddForce;

    public AudioClip EatAudio;
    public AudioClip RushAudio;
    public AudioClip ThrowAudio;

    private void Awake()
    {
        _playerControl = GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        RashCD = setRushCD;
        RushInput = "Rush" + Playernumber;
        EatInput = "Eat" + Playernumber;
        ThrowInput = "Throw" + Playernumber;
        //maxAllCollection = 100;
        canThrow = true;
        hasCollection = false;
        canDash = true;
        DashForce = 800f;

    }

    // Update is called once per frame
    void Update()
    {
       
      
        
        //投掷
        if (Input.GetButtonDown(ThrowInput))
        {
            if (hasCollection)
            {
                Child = grabPosition.transform.GetChild(0);
                //调整子物体位置并断开连接
                Child.transform.parent = null;
                Child.transform.position = grabPosition.transform.position;
                    
                //启动Collection中的投掷方法
                Child.GetComponent<Collection>().fire(FirePosition.transform);
                hasCollection = false;
                AudioManager.instance.AudioPlay(ThrowAudio);
            }
        }

        //吃
        if (Input.GetButtonDown(EatInput))
        {
            if (hasCollection)
            {
                //增加质量
                rb.mass+=3;
                Bigger();
                Child = grabPosition.transform.GetChild(0);
                RashCD += setRushCD/2;
                //销毁抓取的掉落物
                Child.GetComponent<Collection>().Destroy();
                hasCollection = false;
                _playerControl.forceMultiplier += AddForce;
                AudioManager.instance.AudioPlay(EatAudio);
            }
        }
            
        //冲刺
        if (Input.GetButtonDown(RushInput))
        {
            if (canDash)
            {
                DashForce = 1200f + (rb.mass-1) * 500;
            
                //rb.AddForce(rb.velocity.normalized * DashForce * rb.mass/*, ForceMode2D.Impulse*/);s

                Vector3 dir = _playerControl.LastMoveDirection;
                rb.AddForce(dir*DashForce);
                //rb.AddForce(this.gameObject.transform.right*DashForce);
                AudioManager.instance.AudioPlay(RushAudio);
                resetCanDash();
                setCanMove();
                _playerControl.moveState = MoveState.rush;
                TimersManager.SetTimer(this, RashCD, resetCanDash);
                TimersManager.SetTimer(this, 1f, setCanMove);
               
            }
        }
        
        
        if (hasCollection)
        {
            Child = grabPosition.transform.GetChild(0);
            Child.transform.position = grabPosition.transform.position;
        }
    }

    private void FixedUpdate()
    {
        
        
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collection"))
        {
            var flag = other.GetComponent<Collection>();
            //如果收集物未被拾取且玩家未拾取
            if (!flag.isHeld&&!hasCollection)
            {
                other.GetComponent<Collection>().isHeld = true;
                hasCollection = true;
                
                //将收集物的位置绑定到抓取位置
                other.transform.position = grabPosition.transform.position;
                other.transform.parent = grabPosition.transform;
            }
            else if(flag.Throw&&flag.isHeld)
            {
                rb.mass+=3;
                _playerControl.forceMultiplier += AddForce;
                Bigger();
                RashCD += setRushCD/2;
                other.GetComponent<Collection>().Destroy();
            }
        }
    }

    

    private void resetCanDash()
    {
        if (canDash)
        {
            canDash = false;
            DashImage.GetComponent<Image>().sprite = DashCDimage;
        }
        else
        {
            canDash = true;
            DashImage.GetComponent<Image>().sprite = Dashimage;
        }
    }

    private void Bigger()
    {
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
    }

    private void setCanMove()
    {
        if (_playerControl.canMove)
        {
            _playerControl.canMove = false;
        }
        else
        {
            _playerControl.canMove = true;
        }
    }
    
}