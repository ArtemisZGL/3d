using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUserGUI : MonoBehaviour {

    private IUserAction act;
	// Use this for initialization
	void Start () {
        act = Director.getInstance().currentSceneController as IUserAction;
	}
	
	// Update is called once per frame
    /*
	void Update () {
        if(!act.getState())
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            act.playerMoving(x, z);
        }
    }
    void OnGUI()
    {
        if(act.getState())
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 400, 400), "GAMEOVER" + " your score is " + Singleton<ScoreRecorder>.Instance.score.ToString());
            if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 - 45, 90, 90), "Restart"))
            {
                act.setState(false);
                Singleton<ScoreRecorder>.Instance.Reset();
            }
        }
        else
        {
            GUI.Label(new Rect(2 * Screen.width / 3, 0, 400, 400), "score: " + Singleton<ScoreRecorder>.Instance.score.ToString());
        }
    }
    */
}
