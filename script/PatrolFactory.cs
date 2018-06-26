using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerData : MonoBehaviour
{
    public int areaIndex;
}

public class PatrolData : MonoBehaviour
{
    public int currentIndex;
    public bool isFollow = false;
    public GameObject player; 
    public Vector3 originPos;
}

public class PatrolFactory : MonoBehaviour {

    public GameObject firstPatrol = null;
    private List<PatrolData> used = new List<PatrolData>();
    private List<PatrolData> free = new List<PatrolData>();

    private void Awake()
    {
        firstPatrol = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/patrol"), Vector3.zero, Quaternion.identity);
        firstPatrol.SetActive(false);
    }

    public GameObject GetPatrol()
    {
        GameObject newPatrol = null;
        if (free.Count > 0)
        {
            newPatrol = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            newPatrol = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/patrol"), Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(newPatrol);
            newPatrol.AddComponent<PatrolData>();
        }

        used.Add(newPatrol.GetComponent<PatrolData>());
        newPatrol.name = newPatrol.GetInstanceID().ToString();

        return newPatrol;
    }

    public void FreePatrol(GameObject patrol)
    {
        PatrolData target = null;
        for (int i = 0; i < used.Count; i++)
        {
            if (patrol.GetInstanceID() == used[i].gameObject.GetInstanceID())
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
