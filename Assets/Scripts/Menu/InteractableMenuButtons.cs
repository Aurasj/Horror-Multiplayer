using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private GameObject createPrivateButton;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField playersInputField;


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
        createPublicButton.SetActive(false);
        createPrivateButton.SetActive(true);
        Debug.Log("Private Lobby!");
    }
    public void CreatePrivateButton()
    {
        createPublicButton.SetActive(true);
        createPrivateButton.SetActive(false);
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
