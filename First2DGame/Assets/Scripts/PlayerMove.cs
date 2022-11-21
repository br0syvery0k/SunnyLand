using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;//刚体组件，用来调整速度
    private Animator anim;//动画组件
    public float speed;//速度
    public float JumpForce;//跳跃的力量
    public float ClimbSpeed;//爬的速度
    public Transform DeathLine;//角色死亡线
    public LayerMask Ground;//获得图层的信息
    public GameObject DeathTextPanel;//死亡Text
    public Transform PlayerHead;//角色的头部坐标
    public Transform PlayerBottom;//角色脚底坐标
    private CircleCollider2D Coll;//角色碰撞组件
    private BoxCollider2D BoxColl;//角色BoxCollider2D组件
    public int Count;//计数
    private Text Score;//分数
    private Text FinalScore;//最终分数
    private bool isHurt = false;//是否受伤
    private bool isDeathing = false;//是否死亡
    private float DeathLine_y;//死亡线的Y坐标
    private int CollectionIndex = 0;//收集物编号,1为Cherry,2为Diamond,0就是没有
    private bool isGround;//是否在地面上
    private bool CanDoubleJump = true;//是否可以二段跳
    private bool isInLadder = false;//是否在爬梯子

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        Coll = this.GetComponent<CircleCollider2D>();
        BoxColl = this.GetComponent<BoxCollider2D>();
        Score = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        DeathLine_y = DeathLine.position.y;
        Destroy(DeathLine.gameObject);
        speed = 4.5f;
        JumpForce = 5;
        ClimbSpeed = 2;
        Count = PlayerPrefs.GetInt("Score");
        Score.text = Count.ToString();
    }

    void Update()
    {
        if (!isHurt)
            Movement();
        ExchangeAnim();
        if (!isDeathing)
            PlayerDeath();
        Collect();
        isGround = Physics2D.OverlapCircle(PlayerBottom.position, 0.1f, Ground);
        //IsPressJumpKey();
        Climb();
    }

    //角色移动
    private void Movement()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float FaceDirection = Input.GetAxisRaw("Horizontal"); //这是直接获取0 -1 1 不会是中间的小数
        //角色左右移动
        if (HorizontalMove != 0)
        {
            rb.velocity = new Vector2(HorizontalMove * speed, rb.velocity.y);
            anim.SetFloat("run", Mathf.Abs(HorizontalMove));
        }
        //角色转向
        if (FaceDirection != 0)
        {
            this.transform.localScale = new Vector3(FaceDirection, 1, 1);
        }
        //角色跳跃
        PlayerJump();
        /*if (Input.GetButtonDown("Jump") && Coll.IsTouchingLayers(Ground))
          {
              rb.velocity = new Vector2(rb.velocity.x, JumpForce);
              //JumpAudio.Play();
              AudioManager.instance.JumpAudioSource();
              anim.SetBool("Jump", true);
          }*/
        Crouch();
    }

    //角色跳跃
    private void PlayerJump()
    {
        if (isGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            AudioManager.instance.JumpAudioSource();
            anim.SetBool("Jump", true);
            //isPressJumpKey = false;
            anim.SetBool("Fall", false);
            CanDoubleJump = true;
        }
        else
        {
            if (CanDoubleJump && Input.GetButtonDown("Jump"))
            {
                rb.velocity = Vector2.up * JumpForce;
                AudioManager.instance.JumpAudioSource();
                anim.SetBool("Jump", true);
                CanDoubleJump = false;
                anim.SetBool("Fall", false);
            }
        }
    }

    //动画切换
    private void ExchangeAnim()
    {
        if (rb.velocity.y < 0 && !isInLadder)
            anim.SetBool("Fall", true);
        //上升与下落的动画转换
        if (anim.GetBool("Jump") && !isHurt)
        {
            if (rb.velocity.y < 0 && !isInLadder)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
            }
        }
        else if(isHurt)
        {
            //如果受伤了
            anim.SetBool("Hurt", true);
            anim.SetBool("Jump", false);
            anim.SetFloat("run", 0);//在受伤后会有速度，此时如果不调0，会直接进入run的动画而不是idel的动画
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//如果受伤的时候x速度变为0，那么就将isHurt置零(其实这里不要isHurt也行，不过问题不大)
            {
                isHurt = false;
                anim.SetBool("Hurt", false);//动画重新回到idel状态
            }
        }
        else if (Coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("Fall", false);
        }
    }

    //是否碰到物体或者碰到梯子
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //这里collision是局部变量，如果我们碰到了一个物体的Collider2D组件，并且这个组件是Trigger，那么就看这个组件的tag是否是Collections
        //Is Trigger 就是是否是触发器的意思，要判断这个就必须要打开 Is Trigger
        if(collision.tag == "Cherry")
        {
            CollectionIndex = 1;
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "Diamond")
        {
            CollectionIndex = 2;
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Ladder")
        {
            isInLadder = true;
        }
    }

    //角色离开梯子
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            isInLadder = false;
        }
    }

    //物品收集(分数的叠加)
    private void Collect()
    {
        if (CollectionIndex == 1)
        {
            AudioManager.instance.CollectAudioSource();
            //CollectAudio.Play();
            Count++;
            Score.text = Count.ToString();
        }
        else if (CollectionIndex == 2)
        {
            AudioManager.instance.CollectAudioSource();
            //CollectAudio.Play();
            Count += 5;
            Score.text = Count.ToString();
        }
        //重置 CollectionIndex 编号
        CollectionIndex = 0;
    }

    //撞击敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            /*
            ? 这里的 GetComponent<Enemy>() 让我有点疑惑，应该是我对这个 GetComponent 方法的理解产生了偏差，但是我现在想了很久也不明白为什么
            等到时候学了泛型的时候再回来看一下吧。现在就姑且理解为现在这个 collision 对象的 Enemy 组件，然后调用Enemy组件内部的方法(Enemy明明应该是其父类
            ，就像Transform这些类一样，但是每次都可以在这个对象里面拿到父类的东西，难道这就是泛型的作用吗?)
                                                                                                            2022-04-19
            */
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("Fall"))
            {
                enemy.Death();
                rb.velocity = new Vector2(rb.velocity.x, JumpForce - 1);
                anim.SetBool("Jump", true);
                anim.SetBool("Fall", false);
                Count += 3;
                Score.text = Count.ToString();
            }
            else
            {
                isHurt = true;
                //碰到敌人时扣分
                if (Count >= 1)
                {
                    Count -= 1;
                    Score.text = Count.ToString();
                }
                //受伤反弹
                if(this.transform.position.x < collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(-3, rb.velocity.y + 0.5f);
                }
                else if(this.transform.position.x > collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(3, rb.velocity.y + 0.5f);
                }
            }
        }
    }
    
    //角色死亡
    private void PlayerDeath()
    {
        if (this.transform.position.y <= DeathLine_y)
        {
            isDeathing = true;
            anim.SetTrigger("Death");
            //GetComponent<AudioSource>().enabled = false;
            //DeathAudio.enabled = true;
            //DeathAudio.Play();
            AudioManager.instance.DeathAudioSource();
            DeathTextPanel.SetActive(true);
            //显示最终分数
            FinalScore = GameObject.FindGameObjectWithTag("FinalScore").GetComponent<Text>();
            FinalScore.text = Count.ToString();
            Invoke("ReStart", 3.5f);
            MaxScore();
        }
    }

    //角色死亡后的最大分数
    //这个地方是需要改进的,如果是这样就只有Player死亡时才会进行数据存储,需要放在一个更好的地方
    private void MaxScore()
    {
        //初始化MaxScore
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
        int MaxScore = PlayerPrefs.GetInt("MaxScore");
        //如果现在的分数大于最大分数
        if (Count > MaxScore)
            PlayerPrefs.SetInt("MaxScore", Count);
        //数据清零
        PlayerPrefs.SetInt("Score", 0);
    }

    //角色下蹲
    private void Crouch()
    {
        if (!Physics2D.OverlapCircle(PlayerHead.position, 0.2f, Ground))
        {
            if (Input.GetButton("Crouch") && !isInLadder)
            {
                BoxColl.enabled = false;
                anim.SetBool("Crouch", true);
            }
            else
            {
                BoxColl.enabled = true;
                anim.SetBool("Crouch", false);
            }
        }
    }
    
    //死亡后重新回到 InHouse 场景
    private void ReStart()
    {
        SceneManager.LoadScene("InHouse");
    }
    
    //角色爬梯子
    private void Climb()
    {
        if (isInLadder)
        {
            float VerticalMove = Input.GetAxis("Vertical");
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetFloat("run", 0);
            rb.gravityScale = 0;
            if (VerticalMove != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, VerticalMove * ClimbSpeed);
                anim.SetBool("ClimbStay", false);
                anim.SetBool("Climb", true);
                anim.SetBool("Crouch", false);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                anim.SetBool("ClimbStay", true);
                anim.SetBool("Climb", false);
            }
        }
        else if(!isInLadder)
        {
            rb.gravityScale = 1;
            anim.SetBool("ClimbStay", false);
            anim.SetBool("Climb", false);
        }
    }
}
