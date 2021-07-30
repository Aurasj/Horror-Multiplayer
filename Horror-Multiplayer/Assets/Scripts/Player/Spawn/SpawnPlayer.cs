using MLAPI;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] NetworkPlayerSpawner networkPlayerSpawner;

    void Start()
    {
        //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
    }
    

}
