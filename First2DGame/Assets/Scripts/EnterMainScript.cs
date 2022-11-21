using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterMainScript : MonoBehaviour
{
    public GameObject EnterDialog;
    private bool PlayerIsHere = false;
    private PlayerMove Player;
    private void Update()
    {
        if (PlayerIsHere && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("Score", Player.Count);
            SceneManager.LoadScene("SampleScene");
        }
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnterDialog.SetActive(true);
            Player = collision.gameObject.GetComponent<PlayerMove>();
        }
        PlayerIsHere = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnterDialog.SetActive(false);
            //Player = collision.gameObject.GetComponent<PlayerMove>();
        }
        PlayerIsHere = false;
    }
}
