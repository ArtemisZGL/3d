using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class patrolCollission :NetworkBehaviour {
    void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.name == "player(Clone)")
        {
            Singleton<GameEventManager>.Instance.PlayerGameover();
            this.gameObject.GetComponent<Animator>().SetBool("attackIndex", true);
            //this.gameObject.GetComponent<PatrolData>().player.GetComponent<Animator>().SetBool("dead", true);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "pp")
        {
            Cmdenter(collider.gameObject);
        }
    }
    [Command]
    void Cmdenter(GameObject go)
    {
        this.gameObject.GetComponent<PatrolData>().isFollow = true;
        this.gameObject.GetComponent<PatrolData>().player = go;
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "pp")
        {

            Cmdexit(collider.gameObject);
        }
    }
    [Command]
    void Cmdexit(GameObject go)
    {
        this.gameObject.GetComponent<Animator>().SetBool("attackIndex", false);
        this.gameObject.GetComponent<PatrolData>().isFollow = false;
        this.gameObject.GetComponent<PatrolData>().player = go;
    }
}
