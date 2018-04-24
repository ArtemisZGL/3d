using arrow.action;
using arrow.control;
using arrow.factory;
using arrow.singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow.manager
{
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
        protected void FixedUpdate()
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

        public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
        {
            action.gameobject = gameobject;
            action.transform = gameobject.transform;
            action.callback = manager;
            waitingAdd.Add(action);
            action.Start();
        }
    }

    public class PhysisActionManager : ActionManager, ISSActionCallback
    {
        public firstcontroller sceneController;

        public void SSActionEvent(SSAction source,
           SSActionEventType events = SSActionEventType.Competeted,
           int intParam = 0,
           string strParam = null,
           UnityEngine.Object objectParam = null)
        {
            if (source is arrowAction)
            {
                Destroy(source.gameobject.GetComponent<CapsuleCollider>());
                Destroy(source.gameobject.GetComponent<Rigidbody>());
                ArrowFactory df = Singleton<ArrowFactory>.Instance;
                df.FreeArrow(source.gameobject);
            }
        }

        public void loadAction(GameObject arrow, Vector3 force)
        {
            RunAction(arrow, arrowAction.GetArrowAction(force), (ISSActionCallback)this);
        }
    }



}
