using MLAPI;
using UnityEngine;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab = null;
    [SerializeField] private Transform[] spawnPoints;
    private int index = 0;

    private bool spawnAsPlayerObject = true;

    private void OnEnable()
    {
        //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void Start()
    {
        if (IsHost)
        {
            SpawnPlayer(OwnerClientId);
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

    }
    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            SpawnPlayer(clientId);
        }
    }

    public void SpawnPlayer(ulong clientId)
    {
        NetworkObject player = Instantiate(playerPrefab, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);
        player.name = playerPrefab.name + "(" + clientId + ")";

        if (spawnAsPlayerObject)
        {
            player.SpawnAsPlayerObject(clientId);
            Debug.Log("clientId = " + clientId);
        }
        else
        {
            player.SpawnWithOwnership(clientId);
            Debug.Log("clientId = " + clientId);
        }

        index++;
    }
}
