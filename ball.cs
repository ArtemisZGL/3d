using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {
    public GameObject player;
    public float speed = 5;
	// Use this for initialization
	void Start () {
        player = Instantiate(Resources.Load("ball"), new Vector3(0, 10, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        player.transform.Translate(x * Time.deltaTime * speed, y * Time.deltaTime * speed, 0);

        if(player.transform.position.y > 10)
        {
            //player.transform.Find("Particle System").GetComponent<ParticleSystem>().startColor = Color.red;
            player.transform.Find("Particle System (1)").GetComponent<ParticleSystem>().startColor = Color.red;
            player.transform.Find("Particle System (2)").GetComponent<ParticleSystem>().startColor = Color.red;
            player.transform.Find("xingguang").GetComponent<ParticleSystem>().startColor = Color.red;
        }
        else
        {
            //player.transform.Find("Particle System").GetComponent<ParticleSystem>().startColor = Color.yellow;
            player.transform.Find("Particle System (1)").GetComponent<ParticleSystem>().startColor = Color.yellow;
            player.transform.Find("Particle System (2)").GetComponent<ParticleSystem>().startColor = Color.yellow;
            player.transform.Find("xingguang").GetComponent<ParticleSystem>().startColor = Color.yellow;
        }
    }
}
