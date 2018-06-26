using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCollider : MonoBehaviour {
    FirstController first;
    public int index;
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider collider)
    {
        first = Director.getInstance().currentSceneController as FirstController;
        if (collider.gameObject.name == "pp")
        {
           first.player.GetComponent<PlayerData>().areaIndex = index;
        }
    }
}
