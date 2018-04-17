using Disk.factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{

    public int score;


    // Use this for initialization  
    void Start()
    {
        score = 0;
    }

    public void add_score(GameObject disk)
    {
        score += disk.GetComponent<DiskData>().score;
    }

    public void Reset()
    {
        score = 0;
    }
}
