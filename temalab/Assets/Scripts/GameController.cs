using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int maxHits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(maxHits == 0)
        {

        }
    }

    public void Hit()
    {
        maxHits--;
        Debug.Log("Hit decreased");
    }
}
