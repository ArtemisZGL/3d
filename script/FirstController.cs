using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirstController : NetworkBehaviour, ISceneControl, IUserAction
{
    public int patrolCount = 9;
    public PatrolFactory patrol_factory;
    public ScoreRecorder recorder;
    public PatrolActionManager action_manager;
    public GameObject player;
    private List<GameObject> patrols;
    //private float player_speed = 5;
    private float rotate_speed = 135f;
    private bool game_over = false;
    private IUserGUI GUI;
    private GameEventManager eventManager;
    public GameObject enemyPrefab;

    public void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        player.AddComponent<PlayerData>().areaIndex = 1;
        action_manager = gameObject.AddComponent<PatrolActionManager>() as PatrolActionManager;
        eventManager = gameObject.AddComponent<GameEventManager>() as GameEventManager;
        LoadResources();
        recorder = gameObject.AddComponent<ScoreRecorder>() as ScoreRecorder;
        GUI = gameObject.AddComponent<IUserGUI>() as IUserGUI;
    }

    public void Update()
    {
        if (!isServer)
            return;
    }


    public override void OnStartServer()
    {
        patrols = new List<GameObject>();


        //patrol_factory = gameObject.AddComponent<PatrolFactory>() as PatrolFactory;
        
        int[] pos_x = { 6, -4, -14 };
        int[] pos_z = { -6, 4, 14 };
        Vector3[] pos = new Vector3[9];
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                pos[index] = new Vector3(pos_x[i], 0, pos_z[j]);
                index++;
            }
        }
        for (int i = 0; i < patrolCount; i++)
        {
            try
            {
                var enemy = (GameObject)Instantiate(enemyPrefab, pos[i], Quaternion.identity);
                enemy.SetActive(true);
                enemy.AddComponent<PatrolData>();
                NetworkServer.Spawn(enemy);
                patrols.Add(enemy);
            }
            catch
            {

            }
        }
        
        for (int i = 0; i < patrols.Count; i++)
        {
            action_manager.loadPatrolAction(patrols[i], pos[i]);
            patrols[i].GetComponent<PatrolData>().currentIndex = i + 1;

        }

    }
    public void LoadResources()
    {
        //player = Instantiate(Resources.Load("prefab/player"), new Vector3(-10,0, 10), Quaternion.Euler(0,0,0)) as GameObject;
        //player.name = "player";
        
        
        
        
    }
    /*
    public void playerMoving(float x, float z)
    {
       //player.transform.Translate(x * player_speed * Time.deltaTime, 0, z * player_speed * Time.deltaTime);
    }

    public void setState(bool state)
    {
        game_over = state;
        for (int i = 0; i < patrolCount; i++)
        {
            patrols[i].GetComponent<Animator>().SetBool("attackIndex", false);
        }
        player.transform.position = new Vector3(-10, 0, 10);
    }
    */

    public bool getState()
    {
        return game_over;
    }

    void OnEnable()
    {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }
    void OnDisable()
    {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }

    void AddScore()
    {
        recorder.add_score(1);
    }
    void Gameover()
    {
        game_over = true;
    }
}

