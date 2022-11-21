using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMove : Enemy
{
    private Rigidbody2D eagle;
    //private CircleCollider2D Coll;
    public Transform UpPoint, DownPoint;
    private float Up, Down;
    private bool isGoUp = true;
    private float Speed = 4;
    protected override void Start()
    {
        base.Start();
        eagle = this.GetComponent<Rigidbody2D>();
        //Coll = this.GetComponent<CircleCollider2D>();
        Up = UpPoint.position.y;
        Down = DownPoint.position.y;
        Destroy(UpPoint.gameObject);
        Destroy(DownPoint.gameObject);
    }

    void Update()
    {
        MoveMent();
    }

    private void MoveMent()
    {
        //如果这个角色正在死亡，就不要动了
        if (!this.isDeathing)
        {
            if (isGoUp)
            {
                eagle.velocity = new Vector2(eagle.velocity.x, Speed);
                if (this.transform.position.y >= Up)
                    isGoUp = false;
            }
            else
            {
                eagle.velocity = new Vector2(eagle.velocity.x, -Speed);
                if (this.transform.position.y <= Down)
                    isGoUp = true;
            }
        }
    }
}
