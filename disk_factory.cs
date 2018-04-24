using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Disk.factory
{
    public class DiskData : MonoBehaviour
    {
        public int score;
        public bool clickable;
    }

    public class DiskFactory : MonoBehaviour
    {

        public GameObject first_disk;

        private List<DiskData> used = new List<DiskData>();
        private List<DiskData> free = new List<DiskData>();

        private void Awake()
        {
            first_disk = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"), Vector3.zero, Quaternion.identity);
            first_disk.SetActive(false);
        }

        public GameObject GetDisk(int current_round)
        {
            GameObject new_disk = null;
            if (free.Count > 0)
            {
                new_disk = free[0].gameObject;
                free.Remove(free[0]);
            }
            else
            {
                new_disk = GameObject.Instantiate<GameObject>(first_disk, Vector3.zero, Quaternion.identity);
                new_disk.AddComponent<DiskData>();
                //new_disk.AddComponent<rotate>();
            }

            int color_index = 0;
            if(current_round == 0)
            {
                color_index = 0;
            }
            else if (current_round == 1)
            {
                float red_rate = 0.7F;
                int random = Random.Range(0, 10);
                if (random < red_rate * 10)
                    color_index = 1;
                else
                    color_index = 0;
            }
            else if (current_round == 2)
            {
                float black_rate = 0.5F;
                float red_rate = 0.3F;
                int random = Random.Range(0, 10);
                if (random < black_rate * 10)
                    color_index = 2;
                else if (random - black_rate * 10 < red_rate * 10)
                    color_index = 1;
                else
                    color_index = 0;
            }

            else if(current_round == 3)
            {
                float black_rate = 0.5F;
                float red_rate = 0.2F;
                float purple_rate = 0.1F;
                int random = Random.Range(0, 10);
                if (random < black_rate * 10)
                    color_index = 2;
                else if (random - black_rate * 10 < red_rate * 10)
                    color_index = 1;
                else if (random - black_rate * 10 - red_rate * 10 < purple_rate * 10)
                    color_index = 3;
                else
                    color_index = 0;
            }

            if(color_index == 0)
            {
                new_disk.GetComponent<DiskData>().score = 1;
                new_disk.GetComponent<Renderer>().material.color = Color.yellow;
                

            }
            else if(color_index == 1)
            {
                new_disk.GetComponent<DiskData>().score = 2;
                new_disk.GetComponent<Renderer>().material.color = Color.red;
            }
            else if(color_index == 2)
            {
                new_disk.GetComponent<DiskData>().score = 3;
                new_disk.GetComponent<Renderer>().material.color = Color.black;
            }
            else if(color_index == 3)
            {
                new_disk.GetComponent<DiskData>().score = -3;
                new_disk.GetComponent<Renderer>().material.color = Color.magenta;
            }
            new_disk.GetComponent<DiskData>().clickable = true;
            used.Add(new_disk.GetComponent<DiskData>());
            new_disk.name = new_disk.GetInstanceID().ToString();
            return new_disk;
        }

        public void FreeDisk(GameObject disk)
        {
            DiskData target = null;
            for(int i = 0; i < used.Count; i++)
            {
                if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
                {
                    target = used[i];
                }
            }
            if (target != null)
            {
                target.gameObject.SetActive(false);
                free.Add(target);
                used.Remove(target);
            }
        }

    }
}