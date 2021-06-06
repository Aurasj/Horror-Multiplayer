using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsPanel;

    public void SingleplayerButton()
    {
        SceneManager.LoadScene("Singleplayer", LoadSceneMode.Single);
    }
    public void MultiplayerButton()
    {
        Debug.Log("No Multiplayer scene available at the moment");
        //TODO
    }
    public void SettingsButton()
    {
        MainMenu.SetActive(false);
        SettingsPanel.SetActive(true);
        //TODO
    }
    public void CreditsButton()
    {
        Debug.Log("No Credits at the moment");
        //TODO
    }
    public void QuitButton()
    {
        Debug.Log("The game closes!");
        Application.Quit();
    }
}
