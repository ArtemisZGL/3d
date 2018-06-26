    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null);

}

public class SSAction : ScriptableObject
{

    public bool enable = false;
    public bool destroy = false;

    public GameObject gameobject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame  
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }

    public void reset()
    {
        enable = false;
        destroy = false;
        gameobject = null;
        transform = null;
        callback = null;
    }
}

public class patrolAction : SSAction
{
    private float len;
    private float patrolSpeed = 1.5f;
    private enum Direction { SOUTH, WEST, NORTH, EAST };
    private float targetX, targetZ;
    private bool turnIndex = false; 
    private Direction direction = Direction.SOUTH; 
    private PatrolData patrolData;
    private PlayerData playerData;

    private patrolAction()
    {
       
    }
    public override void Start()
    {
        patrolData = this.gameobject.GetComponent<PatrolData>();
        this.enable = true;
        
    }
    public static patrolAction GetSSAction(Vector3 location)
    {
        patrolAction action = CreateInstance<patrolAction>();
        action.targetX = location.x;
        action.targetZ = location.z;
        action.len = Random.Range(3, 8);
        return action;
    }
    public override void Update()
    {
        FirstController first = Director.getInstance().currentSceneController as FirstController;
        playerData = first.player.GetComponent<PlayerData>();
        Gopatrol();
        if (patrolData.isFollow && playerData.areaIndex == patrolData.currentIndex)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this, SSActionEventType.Competeted);
        }
    }
 

    void Gopatrol()
    {
        if (!turnIndex)
        {
            if (direction == Direction.SOUTH)
                targetZ -= len;
            else if (direction == Direction.NORTH)
                targetZ += len;
            else if (direction == Direction.WEST)
                targetX += len;
            else
                targetX -= len;
            turnIndex = true;
        }
        this.transform.LookAt(new Vector3(targetX, 0, targetZ));
        float distance = Vector3.Distance(transform.position, new Vector3(targetX, 0, targetZ));
        if (distance > 0.9)
             transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(targetX, 0, targetZ), patrolSpeed * Time.deltaTime);
        else
        {
            direction++;
            if (direction > Direction.EAST)
                direction = Direction.SOUTH;
            turnIndex = false;
        }
    }
}

public class PatrolFollowAction : SSAction
{
    private float followSpeed = 3F;            
    private GameObject player;           
    private PatrolData patrolData;          
    private PlayerData playerData;       

    private PatrolFollowAction() { }
    public override void Start()
    {
        patrolData = this.gameobject.GetComponent<PatrolData>();
        enable = true;
        playerData = player.GetComponent<PlayerData>();
    }

    public static PatrolFollowAction GetSSAction(GameObject player)
    {
        PatrolFollowAction action = CreateInstance<PatrolFollowAction>();
        action.player = player;
        return action;
    }

    public override void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, followSpeed * Time.deltaTime);
        this.transform.LookAt(player.transform.position);
        if (!patrolData.isFollow || playerData.areaIndex != patrolData.currentIndex)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this, SSActionEventType.Competeted);
        }
    }
}