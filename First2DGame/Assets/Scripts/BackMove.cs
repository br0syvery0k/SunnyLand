using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    private GameObject Camera;
    private Transform CameraTransform;
    void Start()
    {   //�õ���ɫ�� GameObject �������õ� Transform ���
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraTransform = Camera.GetComponent<Transform>();
    }
    void Update()
    {
        //transform.position �ǵ�ǰ����� Transform �����λ��
        this.transform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y, 0);
    }
}
