using Disk.action;
using Disk.controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    bool start;

    // Use this for initialization  
    void Start()
    {
        action = Director.getInstance().currentSceneController as IUserAction;
        start = true;
    }

    private void OnGUI()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            Vector3 pos = Input.mousePosition;
            action.click(pos);

        }
        GUI.color = Color.red;
        GUI.Label(new Rect(2 * Screen.width / 3, 0, 400, 400), "score: " + action.get_score().ToString());
        if (action.get_state() == GameState.OVER)
        {
            GUI.color = Color.red;
            
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 400, 400), "GAMEOVER" + " your score is " + action.get_score().ToString());
            if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 - 45, 90, 90), "Restart"))
            {
                action.reset_score();
                action.set_state(GameState.ROUND_START);
            }
        }
        else
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(3 * Screen.width / 4 , 0, 400, 400), "round: " + (action.get_round() + 1).ToString());
        }
        if (start && GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 - 45, 90, 90), "Start"))
        {
            start = false;
            action.set_state(GameState.ROUND_START);
        }

       
        if (!start && action.get_state() == GameState.ROUND_FINISH && GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 - 45, 90, 90), "Next Round"))
        {
            action.set_state(GameState.ROUND_START);
        }

        
    }


}