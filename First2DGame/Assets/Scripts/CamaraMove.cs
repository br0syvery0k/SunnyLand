using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMove : MonoBehaviour
{
    private GameObject Player;
    private Transform PT;
    void Start()
    {   //拿到角色的 GameObject 对象，再拿到 Transform 组件
        Player = GameObject.FindGameObjectWithTag("Player");
        PT = Player.GetComponent<Transform>();
    }
    void Update()
    {
        //transform.position 是当前对象的 Transform 组件的位置
        this.transform.position = new Vector3(PT.position.x, PT.position.y, -10);
    }
}
