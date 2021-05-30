using UnityEngine;

public class InteractableMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsPanel;
    public void SingleplayerButton()
    {
        Debug.Log("No Singleplayer scene available at the moment");
        //TODO
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
