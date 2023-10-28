using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
   
   [SerializeField] private Button hostButton;
   [SerializeField] private Button clientButton;
   [SerializeField] private Button resetButton;
   [SerializeField] private TextMeshProUGUI playersCounterText;
   private NetworkVariable<int> playersNum = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone);
    public GameObject gameObject;
    public Unity.Netcode.NetworkManager gameControllerObject;
    
    void Start()
    {
        gameObject = GameObject.FindWithTag("NetWorkManager");
        clientButton.enabled = false;
    }
  

    private void Awake(){
        hostButton.onClick.AddListener(()=>
        {
            hostButton.enabled = false;
            clientButton.enabled = true;
            NetworkManager.Singleton.StartHost();
            
        });
        clientButton.onClick.AddListener(()=>
        {
            NetworkManager.Singleton.StartClient();
        });
        resetButton.onClick.AddListener(() =>
        {
            List<NetworkClient> clientsToRemove = new List<NetworkClient>();
            NetworkClient hostClient = null;

            // Iterate over the clients and add the ones to remove to the clientsToRemove list
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                if (client.ClientId == NetworkManager.ServerClientId)
                {
                    // This is the host client, handle it separately (e.g., don't disconnect)
                    hostClient = client;
                   
                }
                else
                {
                    // Other clients can be removed
                    clientsToRemove.Add(client);
                }
            }

            // Disconnect the other clients
            foreach (var client in clientsToRemove)
            {
                NetworkManager.Singleton.DisconnectClient(client.ClientId);
            }

            // If there's a host client, you can perform any specific handling here
            if (hostClient != null)
            {
                // For example, you can send a message to the host client
                // or perform any other necessary operations to keep it connected
            }
        });
    }
    private void Update(){ 
         playersCounterText.text = "Players:" + playersNum.Value.ToString();
        if(!IsServer) return;
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
      
    }
}
