using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource DeathAudio;
    //设置一个死亡属性，主要使Enemy在死亡期间不要进行任何动作
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
