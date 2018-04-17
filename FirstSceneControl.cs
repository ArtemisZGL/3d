using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Disk.action;
using Disk.factory;

namespace Disk.controller
{
    public interface ISceneControl
    {
        void LoadResources();
    }

    public interface IUserAction
    {
        GameState get_state();
        void set_state(GameState gs);
        int get_score();
        void click(Vector3 pos);
        int get_round();
        void reset_score();
    }

    public class Director : System.Object
    {

        public ISceneControl currentSceneController ;

        private static Director director;

        private Director()
        {

        }

        public static Director getInstance()
        {
            if (director == null)
            {
                director = new Director();
            }
            return director;
        }
    }

    public class FirstSceneControl : MonoBehaviour, ISceneControl, IUserAction
    {
        public CCActionManager ac_manager;
        public ScoreRecorder score_recorder;
        public Queue<GameObject> disks = new Queue<GameObject>();
        private int disk_number;
        private int current_round = -1;
        public int round = 4;
        private float time = 0;     //计时
        private float roundtime = 0;    //每个回合隔多久飞一次飞碟
        private GameState gameState = GameState.START;
        UserGUI user_gui;

        void Awake()
        {
            Director director = Director.getInstance();
            ac_manager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
            user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
            director.currentSceneController = this;
            disk_number = 5;
            this.gameObject.AddComponent<ScoreRecorder>();
            this.gameObject.AddComponent<DiskFactory>();
            score_recorder = Singleton<ScoreRecorder>.Instance;
            director.currentSceneController.LoadResources();
        }

        private void Update()
        {

            if (gameState == GameState.ROUND_FINISH && current_round == round)
            {
                gameState = GameState.OVER;
                current_round = -1;
            }
            else
            {
                if (ac_manager.disk_number == 0 && gameState == GameState.RUNNING && score_recorder.score < (current_round + 1)* (current_round + 1) * 4)
                {
                    gameState = GameState.OVER;
                    current_round = -1;
                }
                if (ac_manager.disk_number == 0 && gameState == GameState.RUNNING)
                {

                    gameState = GameState.ROUND_FINISH;
                }
                if (ac_manager.disk_number == 0 && gameState == GameState.ROUND_START)
                {
                    current_round++;
                    ac_manager.disk_number = (current_round + 1) * disk_number;
                    roundtime = 1 - (current_round + 1) * 0.15F;     //每一关时间减少0.15秒
                    nextRound();
                    
                    gameState = GameState.RUNNING;
                }
                if (time > roundtime)
                {
                    show_disk();
                    time = 0;
                }
                else
                {
                    time += Time.deltaTime;
                }
            }


        }


        private void nextRound()
        {
            DiskFactory df = Singleton<DiskFactory>.Instance;
            for (int i = 0; i < ac_manager.disk_number; i++)
                 disks.Enqueue(df.GetDisk(current_round));
            ac_manager.loadAction(disks);
        }

        void show_disk()
        {
            if (disks.Count != 0)
                disks.Dequeue().SetActive(true);
        }

        public void LoadResources()
        {
          GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/bg"));
        }
        public void click(Vector3 pos)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);

            RaycastHit[] clicks;
            clicks = Physics.RaycastAll(ray);
            for (int i = 0; i < clicks.Length; i++)
            {
                RaycastHit hit = clicks[i];

                if (hit.collider.gameObject.GetComponent<DiskData>() != null)   //撞击的是飞碟
                {
                    score_recorder.add_score(hit.collider.gameObject);

                    hit.collider.gameObject.transform.position = new Vector3(0, -10, 0);
                }

            }
        }

        public int get_score()
        {
            return score_recorder.score;
        }
        
        public GameState get_state()
        {
            return gameState;
        }
        
        public void reset_score()
        {
            score_recorder.Reset();
        }

        public int get_round()
        {
            return current_round;
        }


        public void set_state(GameState state)
        {
            gameState = state;
        }
    }
}
