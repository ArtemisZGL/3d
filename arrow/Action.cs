using arrow.factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow.action
{
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

    public class arrowAction : SSAction
    {
        Vector3 force;
        Rigidbody rigid;
        CapsuleCollider coll;
        float begin_speed;
        ConstantForce f;
        bool first = true;

        public override void Start()
        {
            rigid = this.gameobject.AddComponent<Rigidbody>() as Rigidbody;
            coll = this.gameobject.AddComponent<CapsuleCollider>() as CapsuleCollider;
            enable = true;
            first = true;
            /*int index = System.Math.Abs(this.gameobject.GetComponent<DiskData>().score);
            float random_y = UnityEngine.Random.Range(-2F, 5F);
            begin_speed = index * 1.5F + 2;
            direction = new Vector3(-1 * begin_speed * 80, 250, 0);
            Vector3 position = new Vector3(4, random_y, 0);
            this.transform.position = position;*/
            rigid.useGravity = true ;
        }

        public override void Update()
        {
            if (this.gameobject.activeSelf && this.gameobject.GetComponent<ArrowData>().recycle && rigid.velocity.x < 5 && rigid.velocity.y < 5 && rigid.velocity.z < 5)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }

        public override void FixedUpdate()
        {
            if (this.gameobject.activeSelf && first && this.gameobject.GetComponent<ArrowData>().shootable)
            {
                Debug.Log("a");
                rigid.AddForce(force, ForceMode.VelocityChange);
                this.gameobject.GetComponent<ArrowData>().recycle = true;
                first = false;
            }
            
        }

        public static arrowAction GetArrowAction(Vector3 force)
        {
            arrowAction action = ScriptableObject.CreateInstance<arrowAction>();
            action.force = force;
            return action;
        }
    }
}
