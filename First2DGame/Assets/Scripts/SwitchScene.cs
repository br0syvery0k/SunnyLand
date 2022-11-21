using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SwitchScene : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject SureToExitPanel;
    public AudioMixer audioMixer;
    public bool IsInPauseMenu = false;
    private void Update()
    {
        ESCToGetPauseMenu();
    }
    //���˵�������Ϸ
    public void PlayGame()
    {
        //������Ϸʱ��������
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //���˵��˳���Ϸ
    public void QuitGame()
    {
        Application.Quit();
    }
    //���˵���ת��MadeBybr0sy����
    public void MadeBybr0sy()
    {
        SceneManager.LoadScene("brosy");
    }
    //���˵�����Introduce����
    public void Introduce()
    {
        SceneManager.LoadScene("Introduce");
    }
    //��Ϸʱ��ͣ
    public void PauseMenu()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    //��Ϸʱ�˳���ͣ
    public void ExitPause()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    //�Ƿ��˳����浯��
    public void SureToExit()
    {
        PausePanel.SetActive(false);
        SureToExitPanel.SetActive(true);
    }
    //��ȷ���˳�
    public void NotSureButton()
    {
        SureToExitPanel.SetActive(false);
        PausePanel.SetActive(true);
    }
    //�������˵�
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    //��Ϸ��������
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }
    //��ESC������Pause�˵�
    public void ESCToGetPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsInPauseMenu)
        {
            PauseMenu();
            IsInPauseMenu = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && IsInPauseMenu)
        {
            ExitPause();
            IsInPauseMenu = false;
        }
    }

}
