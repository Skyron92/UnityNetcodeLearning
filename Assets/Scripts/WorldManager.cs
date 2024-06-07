using System;
using Unity.Netcode;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            DisplayButton();
        }
        else {
            StatusLabel();

            SubmitNewPosition();
        }
        
        GUILayout.EndArea();
    }

    void DisplayButton() {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartHost();
    }
    
    private void StatusLabel()
    {
        var status = NetworkManager.Singleton.IsHost ? "Host" :
            NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Transport : " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Status : " + status);
    }
    
    private void SubmitNewPosition()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request new position")) {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient) {
                foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds) {
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<Player>().Move();
                }
            }
            else {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.Move();
            }
        };
    }
}
