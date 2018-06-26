using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();


    // Use this for initialization  
    protected void Start()
    {

    }

    // Update is called once per frame  
    protected void Update()
    {
        foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}

public class PatrolActionManager : ActionManager, ISSActionCallback
{
    public FirstController sceneController;

    public void SSActionEvent(SSAction source,
       SSActionEventType events = SSActionEventType.Competeted,
       int intParam = 0,
       string strParam = null,
       UnityEngine.Object objectParam = null)
    {
        if (source is PatrolFollowAction)
        {
            patrolAction move = patrolAction.GetSSAction(source.gameobject.GetComponent<PatrolData>().originPos);
            this.RunAction(source.gameobject, move, this);
            //玩家逃脱
            Singleton<GameEventManager>.Instance.PlayerEscape();
        }
        if (source is patrolAction)
        {
 
            PatrolFollowAction follow = PatrolFollowAction.GetSSAction(source.gameobject.GetComponent<PatrolData>().player);
            this.RunAction(source.gameobject, follow, this);
        }
    }

    public void loadPatrolAction(GameObject patrol, Vector3 location)
    {
        //patrol.SetActive(true);
        //patrol.transform.position = location;
        patrol.GetComponent<PatrolData>().originPos = location;
        RunAction(patrol, patrolAction.GetSSAction(location), this);
        
    }

    public void loadFollowAction(GameObject patrol, GameObject target)
    {
        RunAction(patrol, PatrolFollowAction.GetSSAction(target), this);
    }
}