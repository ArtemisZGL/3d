using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class movePlayer : NetworkBehaviour
{
    private float player_speed = 5;
    // Use this for initialization
    void Start () {
		
	}
    public override void OnStartLocalPlayer()
    {
        this.transform.Find("pp").Find("transformGaren").GetComponent<SkinnedMeshRenderer>().material.color = Color.black;
        this.transform.Find("Main Camera").gameObject.GetComponent<Camera>().depth = 1;
    }
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
            return;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        this.transform.Translate(x * player_speed * Time.deltaTime, 0, z * player_speed * Time.deltaTime);
    }
}
