using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyByContact : NetworkBehaviour
{
    public GameObject Car;
    public GameObject cam;
    public GameObject material;
    private GameController gameController;
    private int maxDecrement;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            maxDecrement = gameController.maxHits;
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find GameController script");
        }
    }

     void Update()
    {
        if(gameController.maxHits == 0)
        {
            List<NetworkClient> clients = new List<NetworkClient>();
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                if (client.ClientId != NetworkManager.ServerClientId)
                {
                    clients.Add(client);
                    // client.tag = "asd";
                }
                else
                {
                    Car.transform.localScale = new Vector3(1,1,1);
                }

            }
            foreach (var client in clients)
            {
                NetworkManager.Singleton.DisconnectClient(client.ClientId);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(gameController.maxHits != 0)
        {
            if (OwnerClientId == NetworkManager.ServerClientId)
            {
                Debug.Log("Collision on host car");
                if (other.tag == "+")
                {
                    Vector3 scale = Car.transform.localScale;
                    scale.x *= 0.66f;
                    scale.y *= 0.66f;
                    scale.z *= 0.66f;

                    Car.transform.localScale = scale;
                    Car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    cam.GetComponent<Follow_Player>().locationOffset *= scale.x;
                    gameController.Hit();
                }
            }
            else {
                Car.tag = "+";
            }
        }
        else
        {
            Debug.Log("Already reached max hits");
        }

        if(other.tag == "PowerUp" && Car.tag == "+" && OwnerClientId != NetworkManager.ServerClientId && maxDecrement > 0)
        {
            Vector3 scale = Car.transform.localScale;
            scale.x *= 0.66f;
            scale.y *= 0.66f;
            scale.z *= 0.66f;

            Car.transform.localScale = scale;
            maxDecrement--;
            Destroy(other.gameObject);
        }
    }
}
