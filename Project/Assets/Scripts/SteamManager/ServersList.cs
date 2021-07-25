using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Transports.SteamP2P;
using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServersList : MonoBehaviour
{
    protected Callback<GameOverlayActivated_t> Callback_gameOverlay;
    protected Callback<LobbyMatchList_t> Callback_lobbyList;
    protected Callback<LobbyDataUpdate_t> Callback_lobbyInfo;
    protected Callback<LobbyEnter_t> Callback_lobbyEnter;
    protected Callback<GameLobbyJoinRequested_t> Callback_gameLobbyJoinRequested;
    protected Callback<LobbyCreated_t> Callback_LobbyCreate;
    protected Callback<AvatarImageLoaded_t> Callback_avatarImageLoaded;
    protected Callback<LobbyChatUpdate_t> Callback_lobbyChatUpdate;

    private NetworkManager networkManager;
    private SteamP2PTransport steamP2P;
    public InteractableMenuButtons interactableMenuButtons;

    ulong current_lobbyID;
    List<CSteamID> lobbyIDS;

    [Header("Instantiate lobbies")]
    [SerializeField] private Transform content;
    [SerializeField] private RoomListing roomListing;

    [Header("Instantiate Players Lobby Info")]
    [SerializeField] private Transform contentPlayers;
    [SerializeField] private PlayerLobbyListing playerLobbyListing;

    [Header("Create Lobby")]
    [SerializeField] private TMP_Text nameLobbyCreate;
    [SerializeField] private TMP_Text maxLobbyPlayers;

    [Header("MenuChanges")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject multiplayerMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    //[SerializeField] private GameObject creditsMenuPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text checkHostText;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text roomPlayers;

    private void Start()
    {
        lobbyIDS = new List<CSteamID>();
        Callback_gameOverlay = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
        Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
        Callback_lobbyInfo = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyInfo);
        Callback_lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        Callback_gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        Callback_LobbyCreate = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        Callback_avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
        Callback_lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);

        networkManager = FindObjectOfType<NetworkManager>();
        steamP2P = FindObjectOfType<SteamP2PTransport>();

        if (SteamAPI.Init())
        {
            Debug.Log("Steam API init -- SUCCESS!");
        }
        else
        {
            Debug.Log("Steam API init -- failure ...");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CheckPlayersLobby();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RefreshButton();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(SteamMatchmaking.GetLobbyOwner((CSteamID)current_lobbyID));
            Debug.Log(networkManager.IsHost);
        }

        SteamAPI.RunCallbacks();
    }
    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }
    void OnGetLobbiesList(LobbyMatchList_t result)
    {
        Debug.Log("Found " + result.m_nLobbiesMatching + " lobbies!");
        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDS.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
            Instantiate(roomListing, content);


            //+++++++++++++++++++++++++++++

            string roomNameInfo = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[i].m_SteamID, "name");

            int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyIDS[i]);
            int numLimPlayers = SteamMatchmaking.GetLobbyMemberLimit((CSteamID)lobbyIDS[i]);

            roomListing.serverNr.text = i.ToString();
            if (roomNameInfo.Length == 0)
            {
                roomListing.serverName.text = "NoName";
            }
            else
            {
                roomListing.serverName.text = roomNameInfo;
            }
            roomListing.serverPlayers.text = numPlayers.ToString() + "/" + numLimPlayers.ToString();
        }
    }
    void OnGetLobbyInfo(LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDS.Count; i++)
        {
            if (lobbyIDS[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                Debug.Log("Lobby " + i + " :: " + SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[i].m_SteamID, "name"));
                return;
            }
        }
    }
    public void LobbyEnter(int lobbyNr)
    {
        //connectingText.text = "Connecting to " + "'" + SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[lobbyNr].m_SteamID, "name") + "'" + " !";

        Debug.Log("Connecting to " + "'" + SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[lobbyNr].m_SteamID, "name") + "'" + " !");
        SteamAPICall_t try_joinLobby = SteamMatchmaking.JoinLobby((CSteamID)lobbyIDS[lobbyNr]);
    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t result)
    {
        SteamAPICall_t try_joinLobby = SteamMatchmaking.JoinLobby(result.m_steamIDLobby);
        mainMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        //creditsMenuPanel.SetActive(false);

        multiplayerMenuPanel.SetActive(true);
    }
    void OnLobbyEntered(LobbyEnter_t result)
    {
        if (result.m_EChatRoomEnterResponse == 1)
        {
            //connectingText.text = "Connected!";
            current_lobbyID = result.m_ulSteamIDLobby;
            PlayerPrefs.SetString("current_lobbyID", current_lobbyID.ToString());
            Debug.Log("Lobby joined!");
            CheckPlayersLobby();
            lobbyPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Failed to join lobby.");
            //connectingText.text = "Failed to connect!";
        }


        if (networkManager.IsHost)
        {
            //TODO nu stiu daca imi mai trebuie asta hmm..
        }
        else
        {
            steamP2P.ConnectToSteamID = (ulong)SteamMatchmaking.GetLobbyOwner((CSteamID)current_lobbyID);
            Debug.Log("New Steam Id Owner: " + steamP2P.ConnectToSteamID);

            networkManager.StartClient();
        }
    }
    public void CreateLobby()
    {
        //connectingText.text = "Trying to create lobby ...";
        Debug.Log("Trying to create lobby ...");

        int maxPlayers;

        if (interactableMenuButtons.playersInputField.text == "")
        {
            maxPlayers = 2;
        }
        else
        {
            maxPlayers = int.Parse(interactableMenuButtons.playersInputField.text);
        }


        SteamAPICall_t try_toHost = SteamMatchmaking.CreateLobby((ELobbyType)interactableMenuButtons.lobbyType, maxPlayers);

    }
    private void OnLobbyCreated(LobbyCreated_t pCallback)
    {
        if (pCallback.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log("Lobby created -- SUCCESS!");
            lobbyPanel.SetActive(true);
            settingsPanel.SetActive(false);

            CheckPlayersLobby();
        }
        else
        {
            Debug.Log("Lobby created -- failure ...");

        }
        networkManager.StartHost();
        //string personalName = SteamFriends.GetPersonaName();

        string lobbyName = nameLobbyCreate.text;
        SteamMatchmaking.SetLobbyData((CSteamID)pCallback.m_ulSteamIDLobby, "name", lobbyName);
    }
    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID != current_lobbyID) { return; }

        playerLobbyListing.playerImage.texture = GetSteamImage(callback.m_iImage);
    }
    private Texture2D GetSteamImage(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        return texture;
    }
    void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
    {
        Debug.Log("[" + LobbyChatUpdate_t.k_iCallback + " - LobbyChatUpdate] - " + pCallback.m_ulSteamIDLobby + " -- " + pCallback.m_ulSteamIDUserChanged + " -- " + pCallback.m_ulSteamIDMakingChange + " -- " + pCallback.m_rgfChatMemberStateChange);
        if (current_lobbyID != 0)
        {
            CheckPlayersLobby();
        }
    }
    public void CheckPlayersLobby()
    {
        foreach (Transform child in contentPlayers)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Destroyed all players Info");

        int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID);

        Debug.Log("\t Number of players currently in lobby : " + numPlayers);
        for (int i = 0; i < numPlayers; i++)
        {
            Debug.Log("\t Player(" + i + ") == " + SteamFriends.GetFriendPersonaName(SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i)));

            Instantiate(playerLobbyListing, contentPlayers);

            int imageId = SteamFriends.GetLargeFriendAvatar((SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i)));

            playerLobbyListing.playerImage.texture = GetSteamImage(imageId);
            playerLobbyListing.playerName.text = SteamFriends.GetFriendPersonaName(SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i)).ToString();
            //playerLobbyListing.playerNrinLobby.text = (i + 1).ToString();
        }
        Debug.Log("Show up players info");
        CheckRoomInfo();
    }
    private void CheckRoomInfo()
    {
        if (networkManager.IsHost)
        {
            checkHostText.text = "You are the HOST!!!";

            startGameButton.interactable = true;
        }
        else
        {
            checkHostText.text = "You are not a HOST";
        }

        string roomNameInfo = SteamMatchmaking.GetLobbyData((CSteamID)current_lobbyID, "name");

        int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID);
        int numLimPlayers = SteamMatchmaking.GetLobbyMemberLimit((CSteamID)current_lobbyID);

        if (roomNameInfo.Length == 0)
        {
            roomName.text = "NoName";
        }
        else
        {
            roomName.text = roomNameInfo;
        }
        roomPlayers.text = numPlayers.ToString() + "/" + numLimPlayers.ToString();

    }
    public void RefreshButton()
    {
        Debug.Log("Trying to get list of available lobbies ...");
        SteamAPICall_t try_getList = SteamMatchmaking.RequestLobbyList();

        DestroyLobbiesButton();
    }
    public void StartGame()
    {
        if (networkManager.IsHost)
        {
            Debug.Log("Start game!");
            NetworkSceneManager.SwitchScene("MultiplayerMap");
        }
    }
    public void DestroyLobbiesButton()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Destroyed all lobbies");
    }
    public void ButtonLeave()
    {
        Debug.Log("Leaving Lobby!");
        //connectingText.text = "Leaving!";

        SteamMatchmaking.LeaveLobby((CSteamID)current_lobbyID);
        if (networkManager.IsHost)
        {
            networkManager.StopHost();
        }
        else if (networkManager.IsServer)
        {
            networkManager.StopServer();
        }
        else if (networkManager.IsClient)
        {
            networkManager.StopClient();
        }
        current_lobbyID = 0;
        PlayerPrefs.SetString("current_lobbyID", current_lobbyID.ToString());
        Debug.Log("You're not in the lobby anymore!");

        settingsPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        startGameButton.interactable = false;

        foreach (Transform child in contentPlayers)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Destroyed all players Info");
    }


}
