using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI; 
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class NetworkUI : NetworkBehaviour
{


public class IPManager
{
    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }
}

public enum ADDRESSFAM
{
    IPv4, IPv6
}

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
    }

    private void Awake(){
        hostButton.onClick.AddListener(()=>
        {
            string ipv4 = IPManager.GetIP(ADDRESSFAM.IPv4);
            NetworkManager.Singleton.StartHost();
            


        });
        clientButton.onClick.AddListener(()=>
        {
            NetworkManager.Singleton.StartClient();
        });
        resetButton.onClick.AddListener(() =>
        {
            List<NetworkClient> clients = new List<NetworkClient>();
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                if (client.ClientId != NetworkManager.ServerClientId)
                {
                    clients.Add(client);
               // client.tag = "asd";
                }

            }
            foreach (var client in clients)
            {
                NetworkManager.Singleton.DisconnectClient(client.ClientId);
            }
            // If there's a host client, you can perform any specific handling here
            
        });
    }
        private void Update(){
            
            playersCounterText.text = "Players:" + playersNum.Value.ToString();
            if(!IsServer) return;
            playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
      
        }
}
