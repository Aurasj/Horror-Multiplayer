using UnityEngine;

public class InteractableMenuButtons : MonoBehaviour
{
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
        Debug.Log("No Settings available at the moment");
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
