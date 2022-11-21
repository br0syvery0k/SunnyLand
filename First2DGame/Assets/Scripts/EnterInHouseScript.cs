using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterInHouseScript : MonoBehaviour
{
    public GameObject EnterEDialog;
    private bool PlayerIsHere = false;
    private void Update()
    {
        if (PlayerIsHere && Input.GetKeyDown(KeyCode.E))
            SceneManager.LoadScene("InHouse");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            EnterEDialog.SetActive(true);
        PlayerIsHere = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            EnterEDialog.SetActive(false);
        PlayerIsHere = false;
    }
}
