using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableMenuButtons : MonoBehaviour
{
    [Header("MainMenu")]
    [SerializeField] private GameObject MainMenu;

    [Header("SettingsPanel")]
    [SerializeField] private GameObject SettingsPanel;

    [Header("MultiplayerPanel")]
    [SerializeField] private GameObject MultiplayerPanel;
    [SerializeField] private GameObject lobbiesPanel;
    [SerializeField] private GameObject createPanel;
    [SerializeField] private Button createbutton;
    [SerializeField] private Button lobbiesButton;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject createPublicButton;
    [SerializeField] private GameObject createFriendsOnlyButton;
    [SerializeField] private GameObject createPrivateButton;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] public TMP_InputField playersInputField;
    [SerializeField] public TMP_Text textPanelInfoLobbyType;

    public int lobbyType = 2;


    //Singleplayer
    public void SingleplayerButton()
    {
        SceneManager.LoadScene("Singleplayer", LoadSceneMode.Single);
    }
    //Multiplayer
    public void MultiplayerButton()
    {
        MainMenu.SetActive(false);
        MultiplayerPanel.SetActive(true);

        createbutton.interactable = true;
        lobbiesButton.interactable = false;

        lobbiesPanel.SetActive(true);

    }
    public void LobbiesButton()
    {
        lobbiesPanel.SetActive(true);
        createPanel.SetActive(false);

        lobbiesButton.interactable = false;
        createbutton.interactable = true;
    }
    public void CreateButton()
    {
        lobbiesPanel.SetActive(false);
        createPanel.SetActive(true);

        lobbiesButton.interactable = true;
        createbutton.interactable = false;
    }
    public void BackButton()
    {
        MultiplayerPanel.SetActive(false);

        lobbiesPanel.SetActive(true);
        createPanel.SetActive(false);

        MainMenu.SetActive(true);
    }
    public void CreatePublicButton()
    {
        lobbyType = 1;
        createFriendsOnlyButton.SetActive(true);
        createPublicButton.SetActive(false);
        textPanelInfoLobbyType.text = "Joinable by friends and invites, but does not show up in the lobby list.";
         Debug.Log("FriendsOnly Lobby!");
    }
    public void CreateFriendsOnly()
    {
        lobbyType = 0;
        createPrivateButton.SetActive(true);
        createFriendsOnlyButton.SetActive(false);
        textPanelInfoLobbyType.text = "The only way to join the lobby is from an invite.";
        Debug.Log("Private Lobby!");
    }
    public void CreatePrivateButton()
    {
        lobbyType = 2;
        createPublicButton.SetActive(true);
        createPrivateButton.SetActive(false);
        textPanelInfoLobbyType.text = "Returned by search and visible to friends.";
        Debug.Log("Public Lobby!");
    }

    public void CheckInputsOnChangeValue()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            startButton.interactable = false;
        }
        else
        {
            startButton.interactable = true;
        }
        nameInputField.characterLimit = 20;
        playersInputField.characterLimit = 1;

        playersInputField.text = Regex.Replace(playersInputField.text, @"[^2-8]", "");
    }
    //Settings
    public void SettingsButton()
    {
        MainMenu.SetActive(false);
        SettingsPanel.SetActive(true);
        //TODO
    }
    //Credits
    public void CreditsButton()
    {
        Debug.Log("No Credits at the moment");
        //TODO
    }
    //Quit
    public void QuitButton()
    {
        Debug.Log("The game closes!");
        Application.Quit();
    }
}
