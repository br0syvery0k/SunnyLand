using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgMove : Enemy
{
    private Rigidbody2D frog;
    //在Unity中拖入两个对象的Transform
    //private Animator frog_anim;
    public Transform Left;
    public Transform Right;
    private CircleCollider2D frog_Coll;
    public LayerMask Ground;
    private float LeftPosition;
    private float RightPosition;
    private bool isFaceLeft = true;
    private float Speed = 3;
    private float JumpForce = 4;
    protected override void Start()
    {
        base.Start();
        anim = this.GetComponent<Animator>();
        frog = this.GetComponent<Rigidbody2D>();
        frog_Coll = this.GetComponent<CircleCollider2D>();
        LeftPosition = Left.position.x;
        RightPosition = Right.position.x;
        Destroy(Left.gameObject);
        Destroy(Right.gameObject);
    }

    void Update()
    {
        //MoveMent();
        ExchangeAnim();
    }
    //角色移动
    private void MoveMent()
    {
        //如果这个角色正在死亡，那就不要动啦！
        if (!this.isDeathing)
        {
            if (isFaceLeft)
            {
                if (this.transform.position.x <= LeftPosition)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    isFaceLeft = false;
                }
                if (frog_Coll.IsTouchingLayers(Ground) && isFaceLeft)
                {
                    frog.velocity = new Vector2(-Speed, JumpForce);
                    anim.SetBool("Jump", true);
                }
            }
            else if (!isFaceLeft)
            {
                if (this.transform.position.x >= RightPosition)
                {
                    this.transform.localScale = new Vector3(1, 1, 1);
                    isFaceLeft = true;
                }
                if (frog_Coll.IsTouchingLayers(Ground) && !isFaceLeft)
                {
                    frog.velocity = new Vector2(Speed, JumpForce);
                    anim.SetBool("Jump", true);
                }
            }
        }
    }
    //切换动画
    private void ExchangeAnim()
    {
        if (anim.GetBool("Jump"))
        {
            if (frog.velocity.y < 0)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
            }
        }
        else if (frog_Coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("Fall", false);
        }
    }
}
