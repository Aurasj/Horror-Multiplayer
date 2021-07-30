using MLAPI;
using MLAPI.Configuration;
using MLAPI.Messaging;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviour
{
    [SerializeField] private NetworkPrefab playerPrefab = null;
    [SerializeField] private Transform[] spawnPoints;
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerServerRpc;

        if (NetworkManager.Singleton.IsHost)
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }


    private Transform GetSpawnPoint()
    {
        // Stop if there are no spawn points in the seen
        if (spawnPoints.Length == 0) { return null; }

        // get number of players
        if (NetworkManager.Singleton.IsHost)
        {
            return spawnPoints[0].transform;
        }
        else
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            int count = (int)(id - 1);
            return spawnPoints[count].transform;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { return; }

        // Get Spawn.  Stop if there are no spawn points in the seen
        Transform spawn = GetSpawnPoint();
        if (spawn == null) { Debug.Log("No Spawn Points in Scene!"); return; }

        // Spawn on Client
        GameObject player = Instantiate(playerPrefab.Prefab, spawn.position, spawn.rotation);


        Debug.Log("clientId = " + clientId);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

}
