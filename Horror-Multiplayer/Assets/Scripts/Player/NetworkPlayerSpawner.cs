using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using System.Collections;
using UnityEngine;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform[] spawnPoints;

    private Transform GetSpawnPoint()
    {
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
        Transform spawn = GetSpawnPoint();
        if (spawn == null) { Debug.Log("No Spawn Points in Scene!"); return; }

        GameObject go = Instantiate(playerPrefab, spawn.position, spawn.rotation);

        Debug.Log("clientId = " + clientId);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        ulong objectId = go.GetComponent<NetworkObject>().NetworkObjectId;

        SpawnClientRpc(objectId);
    }

    // A ClientRpc can be invoked by the server to be executed on a client
    [ClientRpc]
    private void SpawnClientRpc(ulong objectId)
    {
        NetworkObject newPlayer = NetworkSpawnManager.SpawnedObjects[objectId];
        Debug.Log(newPlayer);
        Debug.Log(objectId);
    }
}
