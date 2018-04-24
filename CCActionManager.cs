using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Disk.factory;
using Disk.controller;


namespace Disk.action
{
   

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
        public virtual void FixedUpdate()
        {
           
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


    public enum SSActionEventType : int { Started, Competeted }

    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source,
            SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0,
            string strParam = null,
            Object objectParam = null);

    }

    public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, START, OVER }

    public class SSActionManager : MonoBehaviour
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
                    ac.FixedUpdate();
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
        /*
        protected void FixedUpdate()
        {
            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destroy)
                {
                    waitingDelete.Add(ac.GetInstanceID());
                }
                else if (ac.enable)
                {
                    
                }
            }
        }*/

        public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
        {
            action.gameobject = gameobject;
            action.transform = gameobject.transform;
            action.callback = manager;
            waitingAdd.Add(action);
            action.Start();
        }
    }

    public class CCFlyAction : SSAction
    {
        float g;

        float begin_speed;

        Vector3 direction; //斜方向的运动方向,只需要是一个单位向量就可以了

        float time;

        public override void Start()
        {
            enable = true;
            g = 9.8F;
            time = 0;
            int index = System.Math.Abs(gameobject.GetComponent<DiskData>().score);

            float random_y = UnityEngine.Random.Range(-2F, 5F);

            begin_speed = index * 1.5F + 2;
            direction = new Vector3(-1, 1, 0);
            Vector3 position = new Vector3(4, random_y, 0);
            this.gameobject.transform.position = position;
        }

        // Update is called once per frame  
        public override void Update()
        {
            if (gameobject.activeSelf)
            {
                time += Time.deltaTime;

                transform.Translate(Vector3.down * g * time * Time.deltaTime); //重力的减速

                transform.Translate(direction * begin_speed * Time.deltaTime); //初速度，包括竖直和水平的运动

                if (this.transform.position.y < -4)
                {
                    this.destroy = true;
                    this.enable = false;
                    this.callback.SSActionEvent(this);
                }
            }

        }
    }
    
    
    public class CCActionManager : SSActionManager, ISSActionCallback, IActionManager
    {

        public FirstSceneControl sceneController;
        public int disk_number = 0;

        public int get_num()
        {
            return disk_number;
        }

        public void set_num(int num)
        {
            disk_number = num;
        }

        protected new void Start()
        {
            sceneController = (FirstSceneControl)Director.getInstance().currentSceneController;
            sceneController.ac_manager = this;
        }

        public void SSActionEvent(SSAction source,
            SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0,
            string strParam = null,
            UnityEngine.Object objectParam = null)
        {
            if (source is CCFlyAction)
            {
                disk_number--;
                DiskFactory df = Singleton<DiskFactory>.Instance;
                df.FreeDisk(source.gameobject);
            }
        }

        public void loadAction(Queue<GameObject> diskQueue)
        {
            foreach (GameObject disk in diskQueue)
            {  
                RunAction(disk, ScriptableObject.CreateInstance<CCFlyAction>(), (ISSActionCallback)this);
            }
        }
    }
    

    public interface IActionManager
    {
      void loadAction(Queue<GameObject> diskQueue);
        void set_num(int num);
        int get_num();
    }

    public class PhysisFlyAction : SSAction
    {
        Rigidbody rigid;
        CapsuleCollider trigger;
        CapsuleCollider coll;
        float begin_speed;
        Vector3 direction;
        ConstantForce f;
        bool first = true;
        

        public override void Start()
        {
            rigid = this.gameobject.AddComponent<Rigidbody>() as Rigidbody;
            trigger = this.gameobject.AddComponent<CapsuleCollider>() as CapsuleCollider;
            trigger.isTrigger = true;
            coll = this.gameobject.AddComponent<CapsuleCollider>() as CapsuleCollider;
            //f = this.gameobject.AddComponent<ConstantForce>() as ConstantForce;
            enable = true;
            first = true;
            int index = System.Math.Abs(this.gameobject.GetComponent<DiskData>().score);
            float random_y = UnityEngine.Random.Range(-2F, 5F);
            begin_speed = index * 1.5F + 2;
            direction = new Vector3(-1 * begin_speed * 80, 250, 0);
            Vector3 position = new Vector3(4, random_y, 0);
            this.transform.position = position;
                                                                                                                   //f.force = direction;

            //rigid.SetDensity(1);
            rigid.useGravity = true;
        }
        
        public override void Update()
        {
            if (this.gameobject.activeSelf && this.transform.position.y < -8)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
                Debug.Log("aaaa" + this.callback);
            }
        }

        /*public void OnCollisionEnter(Collision collider)
        {
            if(collider.gameObject.name == "ground")
            {

            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            
        }*/

        public override void FixedUpdate()
        {
            if(this.gameobject.activeSelf && first)
            {
                rigid.AddForce(direction);
                first = false;
            }
             

        }




    }

    public class PhysisActionManager : SSActionManager, ISSActionCallback, IActionManager
    {

        public FirstSceneControl sceneController;
        public int disk_number = 0;
       
        public int get_num()
        {
            return disk_number;
        }

        public void set_num(int num)
        {
            disk_number = num;
        }

        public void SSActionEvent(SSAction source,
            SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0,
            string strParam = null,
            UnityEngine.Object objectParam = null)
        {
            if (source is PhysisFlyAction)
            {
                disk_number--;
                Destroy(source.gameobject.GetComponent<CapsuleCollider>());
                Destroy(source.gameobject.GetComponent<CapsuleCollider>());
                Destroy(source.gameobject.GetComponent<ConstantForce>());
                Destroy(source.gameobject.GetComponent<Rigidbody>());
                DiskFactory df = Singleton<DiskFactory>.Instance;
                df.FreeDisk(source.gameobject);
            }
        }

        public void loadAction(Queue<GameObject> diskQueue)
        {
            foreach (GameObject disk in diskQueue)
            {
                 RunAction(disk, ScriptableObject.CreateInstance<PhysisFlyAction>(), (ISSActionCallback)this);
            }
        }
    }


}
