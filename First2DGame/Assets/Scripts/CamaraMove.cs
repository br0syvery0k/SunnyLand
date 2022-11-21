using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMove : MonoBehaviour
{
    private GameObject Player;
    private Transform PT;
    void Start()
    {   //�õ���ɫ�� GameObject �������õ� Transform ���
        Player = GameObject.FindGameObjectWithTag("Player");
        PT = Player.GetComponent<Transform>();
    }
    void Update()
    {
        //transform.position �ǵ�ǰ����� Transform �����λ��
        this.transform.position = new Vector3(PT.position.x, PT.position.y, -10);
    }
}
