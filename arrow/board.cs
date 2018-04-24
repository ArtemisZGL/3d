using arrow.factory;
using arrow.singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour {
    private List<GameObject> arrows;
    public int s;
    // Use this for initialization
    void Start () {
        arrows = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<ArrowData>().recycle = false;
        other.gameObject.GetComponent<Rigidbody>().useGravity = false;
        other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        arrows.Add(other.gameObject);
        Singleton<ScoreRecorder>.Instance.add_score(s);
    }
}
