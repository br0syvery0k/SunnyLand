using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;//������������������ٶ�
    private Animator anim;//�������
    public float speed;//�ٶ�
    public float JumpForce;//��Ծ������
    public float ClimbSpeed;//�����ٶ�
    public Transform DeathLine;//��ɫ������
    public LayerMask Ground;//���ͼ�����Ϣ
    public GameObject DeathTextPanel;//����Text
    public Transform PlayerHead;//��ɫ��ͷ������
    public Transform PlayerBottom;//��ɫ�ŵ�����
    private CircleCollider2D Coll;//��ɫ��ײ���
    private BoxCollider2D BoxColl;//��ɫBoxCollider2D���
    public int Count;//����
    private Text Score;//����
    private Text FinalScore;//���շ���
    private bool isHurt = false;//�Ƿ�����
    private bool isDeathing = false;//�Ƿ�����
    private float DeathLine_y;//�����ߵ�Y����
    private int CollectionIndex = 0;//�ռ�����,1ΪCherry,2ΪDiamond,0����û��
    private bool isGround;//�Ƿ��ڵ�����
    private bool CanDoubleJump = true;//�Ƿ���Զ�����
    private bool isInLadder = false;//�Ƿ���������

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

    //��ɫ�ƶ�
    private void Movement()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float FaceDirection = Input.GetAxisRaw("Horizontal"); //����ֱ�ӻ�ȡ0 -1 1 �������м��С��
        //��ɫ�����ƶ�
        if (HorizontalMove != 0)
        {
            rb.velocity = new Vector2(HorizontalMove * speed, rb.velocity.y);
            anim.SetFloat("run", Mathf.Abs(HorizontalMove));
        }
        //��ɫת��
        if (FaceDirection != 0)
        {
            this.transform.localScale = new Vector3(FaceDirection, 1, 1);
        }
        //��ɫ��Ծ
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

    //��ɫ��Ծ
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

    //�����л�
    private void ExchangeAnim()
    {
        if (rb.velocity.y < 0 && !isInLadder)
            anim.SetBool("Fall", true);
        //����������Ķ���ת��
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
            //���������
            anim.SetBool("Hurt", true);
            anim.SetBool("Jump", false);
            anim.SetFloat("run", 0);//�����˺�����ٶȣ���ʱ�������0����ֱ�ӽ���run�Ķ���������idel�Ķ���
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//������˵�ʱ��x�ٶȱ�Ϊ0����ô�ͽ�isHurt����(��ʵ���ﲻҪisHurtҲ�У��������ⲻ��)
            {
                isHurt = false;
                anim.SetBool("Hurt", false);//�������»ص�idel״̬
            }
        }
        else if (Coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("Fall", false);
        }
    }

    //�Ƿ��������������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����collision�Ǿֲ��������������������һ�������Collider2D�����������������Trigger����ô�Ϳ���������tag�Ƿ���Collections
        //Is Trigger �����Ƿ��Ǵ���������˼��Ҫ�ж�����ͱ���Ҫ�� Is Trigger
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

    //��ɫ�뿪����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            isInLadder = false;
        }
    }

    //��Ʒ�ռ�(�����ĵ���)
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
        //���� CollectionIndex ���
        CollectionIndex = 0;
    }

    //ײ������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            /*
            ? ����� GetComponent<Enemy>() �����е��ɻ�Ӧ�����Ҷ���� GetComponent ��������������ƫ��������������˺ܾ�Ҳ������Ϊʲô
            �ȵ�ʱ��ѧ�˷��͵�ʱ���ٻ�����һ�°ɡ����ھ͹������Ϊ������� collision ����� Enemy �����Ȼ�����Enemy����ڲ��ķ���(Enemy����Ӧ�����丸��
            ������Transform��Щ��һ��������ÿ�ζ�������������������õ�����Ķ������ѵ�����Ƿ��͵�������?)
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
                //��������ʱ�۷�
                if (Count >= 1)
                {
                    Count -= 1;
                    Score.text = Count.ToString();
                }
                //���˷���
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
    
    //��ɫ����
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
            //��ʾ���շ���
            FinalScore = GameObject.FindGameObjectWithTag("FinalScore").GetComponent<Text>();
            FinalScore.text = Count.ToString();
            Invoke("ReStart", 3.5f);
            MaxScore();
        }
    }

    //��ɫ�������������
    //����ط�����Ҫ�Ľ���,�����������ֻ��Player����ʱ�Ż�������ݴ洢,��Ҫ����һ�����õĵط�
    private void MaxScore()
    {
        //��ʼ��MaxScore
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
        int MaxScore = PlayerPrefs.GetInt("MaxScore");
        //������ڵķ�������������
        if (Count > MaxScore)
            PlayerPrefs.SetInt("MaxScore", Count);
        //��������
        PlayerPrefs.SetInt("Score", 0);
    }

    //��ɫ�¶�
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
    
    //���������»ص� InHouse ����
    private void ReStart()
    {
        SceneManager.LoadScene("InHouse");
    }
    
    //��ɫ������
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
