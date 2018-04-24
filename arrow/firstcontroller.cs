
using arrow.factory;
using arrow.manager;
using arrow.singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using arrow.director;

namespace arrow.control
{
    public enum GameState { ROUND_START, ROUND_FINISH, RUNNING }
    public class firstcontroller : MonoBehaviour, ISceneControl, IUserAction
    {
        public PhysisActionManager ac_manager;
        public ScoreRecorder score_recorder;
        public GameObject bow;
        public GameObject arrow;
        public GameObject board;

        public GameState state;
        IUserGUI user_gui;
        // Use this for initialization
        void Awake()
        {
            Director director = Director.getInstance();
            ac_manager = gameObject.AddComponent<PhysisActionManager>() as PhysisActionManager;
            user_gui = gameObject.AddComponent<IUserGUI>() as IUserGUI;
            director.currentSceneController = this;
            this.gameObject.AddComponent<ScoreRecorder>();
            this.gameObject.AddComponent<ArrowFactory>();
            score_recorder = Singleton<ScoreRecorder>.Instance;
            director.currentSceneController.LoadResources();
            state = GameState.ROUND_FINISH;
        }

        // Update is called once per frame
        void Update()
        {
            if (state == GameState.ROUND_START)
            {
                loadArrow();
                state = GameState.RUNNING;
            }
        }

        public void loadArrow()
        {
            arrow = Singleton<ArrowFactory>.Instance.GetArrow();
            arrow.transform.rotation = bow.transform.rotation;
            arrow.transform.position = bow.transform.position;
            arrow.transform.parent = bow.transform;
            
            arrow.SetActive(true);

        }
        public void set_state(GameState s)
        {
            state = s;
        }

        public GameState get_state()
        {
            return state;
        }

        public void pull(float Mx, float My)
        {
            float x = bow.transform.localEulerAngles.y + Mx;
            float y = bow.transform.localEulerAngles.x - My;
            y = Mathf.Clamp(y, 45, 90);
            bow.transform.localEulerAngles = new Vector3(y, x, 0);
        }

        public void LoadResources()
        {
            bow = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Elven Long Bow/Elven Long Bow"), Vector3.zero, Quaternion.Euler(90, 180, 0));
            board = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/board"),new Vector3(0, -10 , 100 ), Quaternion.Euler(90, 180, 0));
        }

        public int get_score()
        {
            return score_recorder.score;
        }

        public void shot()
        {
            ac_manager.loadAction(arrow, arrow.transform.up * -200);
            arrow.transform.parent = null;
            arrow.GetComponent<ArrowData>().shootable = true;
            state = GameState.ROUND_FINISH;
            arrow = null;

        }


        public void reset_score()
        {
            score_recorder.Reset();
        }
    }
}
