using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip JumpAudioClip;//��Ծ��Ч
    [SerializeField]
    private AudioClip CollectAudioClip;//�ռ���Ч
    [SerializeField]
    private AudioClip DeathAudioClip;//������Ч
    private void Awake()
    {
        //�����ڵĶ��� ��ֵ���ڴ����д����� ��̬����
        instance = this;
    }
    //��Ծ����
    public void JumpAudioSource()
    {
        audioSource.clip = JumpAudioClip;
        audioSource.Play();
    }
    //�ռ�������
    public void CollectAudioSource()
    {
        audioSource.clip = CollectAudioClip;
        audioSource.Play();
    }
    //��������
    public void DeathAudioSource()
    {
        GetComponent<AudioSource>().enabled = false;
        audioSource.clip = DeathAudioClip;
        audioSource.Play();
    }
}
