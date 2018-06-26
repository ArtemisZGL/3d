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

    public void add_score(int s)
    {
        score += s;
    }

    public void Reset()
    {
        score = 0;
    }
}