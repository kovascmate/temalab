using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyByContact : NetworkBehaviour
{
    public GameObject Car;
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
      
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find GameController script");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (OwnerClientId == NetworkManager.ServerClientId)
        {
            Debug.Log("Collision on car");
            if (other.tag == "+")
            {
                Vector3 scale = Car.transform.localScale;
                scale.x *= 0.66f;
                scale.y *= 0.66f;
                scale.z *= 0.66f;

                Car.transform.localScale = scale;
                Car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }
        else {
            Car.tag = "+";
        }
    }
}
