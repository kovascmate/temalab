using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraActivation : NetworkBehaviour
{

    public GameObject camera;
    public AudioListener listener;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            camera.SetActive(true);
            listener.enabled = true;
        }
        else
        {
            camera.SetActive(false);
            listener.enabled = false;
        }
    }
}
