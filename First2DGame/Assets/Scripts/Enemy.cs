using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource DeathAudio;
    //����һ���������ԣ���ҪʹEnemy�������ڼ䲻Ҫ�����κζ���
    protected bool isDeathing;
    protected virtual void Start()
    {
        anim = this.GetComponent<Animator>();
        DeathAudio = this.GetComponent<AudioSource>();
        isDeathing = false;
    }
    public void Death()
    {
        DeathAudio.Play();
        anim.SetTrigger("death");
        isDeathing = true;
    }
    public void DeathAnim()
    {
        Destroy(anim.gameObject);
    }
}
