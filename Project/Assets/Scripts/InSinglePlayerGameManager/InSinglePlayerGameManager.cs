using UnityEngine;
using UnityEngine.SceneManagement;

public class InSinglePlayerGameManager : MonoBehaviour
{
    [HideInInspector] public bool playerOnEnable = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabPanel();
        }
    }
    public void TabPanel()
    {
        if (playerOnEnable)
        {
            playerOnEnable = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            playerOnEnable = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void MainMenu()
    {
        Debug.Log("Leaving singleplayer!");

        SceneManager.LoadScene("Menu");

        Debug.Log("You're not in the singleplayer anymore!");

    }
}
