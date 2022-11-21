using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    private GameObject Camera;
    private Transform CameraTransform;
    void Start()
    {   //拿到角色的 GameObject 对象，再拿到 Transform 组件
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraTransform = Camera.GetComponent<Transform>();
    }
    void Update()
    {
        //transform.position 是当前对象的 Transform 组件的位置
        this.transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y, 0);
    }
}
