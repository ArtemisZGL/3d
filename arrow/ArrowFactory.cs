using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow.factory
{
    public class ArrowData : MonoBehaviour
    {
        public int speed;
        public bool shootable;
        public bool recycle;
    }
    public class ArrowFactory : MonoBehaviour
    {

        public GameObject first_arrow;

        private List<ArrowData> used = new List<ArrowData>();
        private List<ArrowData> free = new List<ArrowData>();

        private void Awake()
        {
            first_arrow = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Elven Long Bow/Elven Long Bow Arrow"), Vector3.zero, Quaternion.identity);
            first_arrow.SetActive(false);
        }

        public GameObject GetArrow()
        {
            GameObject new_arrow = null;
            if (free.Count > 0)
            {
                new_arrow = free[0].gameObject;
                free.Remove(free[0]);
            }
            else
            {
                new_arrow = GameObject.Instantiate<GameObject>(first_arrow, Vector3.zero, Quaternion.identity);
                new_arrow.AddComponent<ArrowData>();
            }


            new_arrow.GetComponent<ArrowData>().speed = 1;
            new_arrow.GetComponent<ArrowData>().shootable = false;
            new_arrow.GetComponent<ArrowData>().recycle = false;

            used.Add(new_arrow.GetComponent<ArrowData>());
            new_arrow.name = new_arrow.GetInstanceID().ToString();
            return new_arrow;
        }

        public void FreeArrow(GameObject arrow)
        {
            ArrowData target = null;
            for (int i = 0; i < used.Count; i++)
            {
                if (arrow.GetInstanceID() == used[i].gameObject.GetInstanceID())
                {
                    target = used[i];
                }
            }
            if (target != null)
            {
                free.Add(target);
                used.Remove(target);
            }
        }

    }
}

