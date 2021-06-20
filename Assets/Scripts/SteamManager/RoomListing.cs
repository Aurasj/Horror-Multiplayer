using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField] public TMP_Text serverNr = null;
    [SerializeField] public TMP_Text serverName = null;
    [SerializeField] public TMP_Text serverPlayers = null;
    [SerializeField] public Button joinButton = null;
    [SerializeField] public int lobbyNr;

    ServersList serversList;
    private void Start()
    {
        serversList = FindObjectOfType<ServersList>();

        lobbyNr = int.Parse(serverNr.text);

        joinButton.onClick.AddListener(delegate () { serversList.LobbyEnter(lobbyNr); });
    }
}
