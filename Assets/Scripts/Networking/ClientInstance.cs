using UnityEngine;
using Mirror;

public class ClientInstance : NetworkBehaviour
{

    public static ClientInstance Instance;

    [SerializeField] private GameObject _playerPrefab = null;
    [SerializeField] private Notification _onPlayerSpawned = null;

    public GameObject Player;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        CmdSpawnPlayer();
    }

    [Command]
    void CmdSpawnPlayer()
    {
        GameObject go = Instantiate(_playerPrefab);
        NetworkServer.Spawn(go, base.connectionToClient);
        Player = go;
        TargetPlayerSpawned(base.connectionToClient, go);
    }

    [TargetRpc]
    void TargetPlayerSpawned(NetworkConnection conn, GameObject go)
    {
        Player = go;
        _onPlayerSpawned.Raise();
    }

    public static ClientInstance GetClientInstance(NetworkConnection conn = null)
    {
        // on server
        if (conn != null)
        {
            if (MyNetworkManager.Players.TryGetValue(conn, out NetworkIdentity ni))
            {
                return ni.GetComponent<ClientInstance>();
            }
            else
            {
                Debug.LogWarning("No CI found for conn: " + conn);
                return null;
            }
        }
        // on client
        else
        {
            return Instance;
        }
    }

}
