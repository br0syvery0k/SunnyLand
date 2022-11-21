using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip JumpAudioClip;//跳跃音效
    [SerializeField]
    private AudioClip CollectAudioClip;//收集音效
    [SerializeField]
    private AudioClip DeathAudioClip;//死亡音效
    private void Awake()
    {
        //把现在的对象 赋值给在此类中创建的 静态对象
        instance = this;
    }
    //跳跃声音
    public void JumpAudioSource()
    {
        audioSource.clip = JumpAudioClip;
        audioSource.Play();
    }
    //收集物声音
    public void CollectAudioSource()
    {
        audioSource.clip = CollectAudioClip;
        audioSource.Play();
    }
    //死亡声音
    public void DeathAudioSource()
    {
        GetComponent<AudioSource>().enabled = false;
        audioSource.clip = DeathAudioClip;
        audioSource.Play();
    }
}
