using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaraFollow : MonoBehaviour {
    Transform first;
	// Use this for initialization
	void Start () {
        first = this.transform.parent.Find("player");

    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(first.position.x, first.position.y + 5, first.position.z - 5);
	}
}
