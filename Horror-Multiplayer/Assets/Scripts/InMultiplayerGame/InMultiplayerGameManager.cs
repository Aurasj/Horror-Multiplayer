using MLAPI;
using Steamworks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InMultiplayerGameManager : MonoBehaviour
{
    [SerializeField] GameObject tabPanel;
    [SerializeField] GameObject infoMenuPanel;

    private NetworkManager networkManager;

    [SerializeField] private SteamChat steamChat;
    PlayerControllerAction inputActions;

    [HideInInspector] public ulong current_lobbyID;

    [HideInInspector] public bool playerOnEnable = false;

    private void Awake()
    {
        inputActions = new PlayerControllerAction();
        inputActions.Enable();

        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            inputActions.LoadBindingOverridesFromJson(rebinds);

        var chatbinding = inputActions.Menu.Chat.GetBindingDisplayString();
        PlayerPrefs.SetString("chatbinding", chatbinding);
    }
    private void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();

        inputActions.Menu.TabMenu.performed += _ => TabPanel();
        inputActions.Menu.Chat.performed += _ => steamChat.ChatButton();

        string currentIDIn = PlayerPrefs.GetString("current_lobbyID");
        current_lobbyID = Convert.ToUInt64(currentIDIn);
    }
    public void TabPanel()
    {
        if (tabPanel)
        {
            if (!tabPanel.activeSelf)
            {
                tabPanel.SetActive(true);

                playerOnEnable = false;

                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                tabPanel.SetActive(false);

                playerOnEnable = true;

                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public void InfoMenuPanelButton()
    {
        if (!infoMenuPanel.activeSelf)
        {
            infoMenuPanel.SetActive(true);
        }
        else
        {
            infoMenuPanel.SetActive(false);
        }
    }
    public void LeaveLobby()
    {
        Debug.Log("Leaving Lobby!");
        //connectingText.text = "Leaving!";

        SteamMatchmaking.LeaveLobby((CSteamID)current_lobbyID);

        if (networkManager.IsHost)
        {
            networkManager.StopHost();
        }
        else if (networkManager.IsClient)
        {
            networkManager.StopClient();
        }
        else if (networkManager.IsServer)
        {
            networkManager.StopServer();
        }

        SceneManager.LoadScene("Menu");

        Debug.Log("You're not in the lobby anymore!");

    }
}
