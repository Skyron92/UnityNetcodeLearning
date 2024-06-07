using Unity.Netcode;
using UnityEngine;

public class RPCTest : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner)
        {
            ServerOnlyRPC(0, NetworkObjectId);
        }
    }

    [Rpc(SendTo.Server)]
    private void ServerOnlyRPC(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Server received the RPC #{value} from NetworkObject #{sourceNetworkObjectId}");
        ClientsAndHostRPC(value, sourceNetworkObjectId);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ClientsAndHostRPC(int value, ulong sourceNetworkObjectId) {
        Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        if(IsOwner) ServerOnlyRPC(value+1, sourceNetworkObjectId);
    }
}
