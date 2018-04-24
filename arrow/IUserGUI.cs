using arrow.control;
using arrow.director;
using arrow.singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUserGUI : MonoBehaviour {
    private IUserAction action;
    private int score;
    public Camera cam;
    public ScoreRecorder score_recorder;

    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        score_recorder = Singleton<ScoreRecorder>.Instance;
    }

    private void OnGUI()
    {
        float MX = Input.GetAxis("Mouse X");
        float MY = Input.GetAxis("Mouse Y");
        action.pull(MX, MY);
        GUI.Label(new Rect(2 * Screen.width / 3, 10, 400, 400), "score: " + action.get_score().ToString());  
        if (Input.GetButtonDown("Fire1") && action.get_state() == GameState.RUNNING)
        {

            Vector3 pos = Input.mousePosition;
            action.shot();
        }
        if(action.get_state() == GameState.ROUND_FINISH)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 45, 40, 90, 90), "play"))
            {
                action.set_state(GameState.ROUND_START);
            }
        }
        


    }
}
