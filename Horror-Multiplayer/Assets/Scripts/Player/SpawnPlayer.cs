using MLAPI;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] NetworkPlayerSpawner networkPlayerSpawner;

    void Start()
    {
        ulong id = NetworkManager.Singleton.LocalClientId;

        networkPlayerSpawner.SpawnPlayerServerRpc(id);
    }

}
