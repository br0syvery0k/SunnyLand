using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumMove : Enemy
{
    private Rigidbody2D opossum;
    public Transform LeftPoint, RightPoint;
    private float LeftPosition, RightPosition;
    private bool isFaceLeft = true;
    private float Speed = 3;

    protected override void Start()
    {
        base.Start();
        opossum = this.GetComponent<Rigidbody2D>();
        LeftPosition = LeftPoint.position.x;
        RightPosition = RightPoint.position.x;
        Destroy(LeftPoint.gameObject);
        Destroy(RightPoint.gameObject);
    }

    void Update()
    {
        MoveMent();
    }

    private void MoveMent()
    {
        if (!this.isDeathing)
        {
            if (isFaceLeft)
            {
                opossum.velocity = new Vector2(-Speed, opossum.velocity.y);
                if (this.transform.position.x <= LeftPosition)
                {
                    this.transform.localScale = new Vector3(-1, 1, 1);
                    isFaceLeft = false;
                }
            }
            else
            {
                opossum.velocity = new Vector2(Speed, opossum.velocity.y);
                if (this.transform.position.x >= RightPosition)
                {
                    this.transform.localScale = new Vector3(1, 1, 1);
                    isFaceLeft = true;
                }
            }
        }
    }
}
