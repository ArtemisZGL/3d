using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pd;
using pd.manager;
using System;
using System.Threading;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public coast_controller right_coast;
    public coast_controller left_coast;
    public boat_controller boat;
    public pd_controller[] pds;
    user_gui userGui;
    public CCActionManager actionManager;
    bool cnext = true;

    public class status : IEquatable<status>
    {
        public status(int p, int d, bool b)
        {
            pnum = p;
            dnum = d;
            boat = b;
            children = new List<status>();
        }
        public bool isLegal()
        {
            if (dnum > pnum && pnum != 0)
                return false;
            if (3 - dnum > 3 - pnum && pnum != 3)
                return false;
            return true;
        }
        
        public bool equals(status other)
        {
            if (other.pnum == pnum && other.dnum == dnum && other.boat == boat)
                return true;
            else
                return false;
        }

        public bool Equals(status other)
        {
            return equals(other);
        }

        public int pnum;
        public int dnum;
        public bool boat;
        public List<status> children;
    }

    public List<status> Graph = new List<status>();
    public List<status> appendGraph = new List<status>();
    public status rootstatus = new status(3, 3, true);
    public List<List<status>> path = new List<List<status>>();
    List<status> visited = new List<status>();

    

	// Use this for initialization
	void Awake () {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userGui = gameObject.AddComponent(typeof(user_gui)) as user_gui;   
        loadResources();

        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        Graph.Add(rootstatus);
        initGraph();
        initShortestPath();
    }

    public void next()
    {
        status current = findCurrentStatus();
        status nextStatus = path[Graph.IndexOf(current)][0];
        StartCoroutine(moveFromStatus(current, nextStatus));
        cnext = false;
    }

    public bool canNext()
    {
        return cnext;
    }

    private int frame = 70;
    IEnumerator moveFromStatus(status first, status second)
    {
        if(boat.get_pcount() > 0 || boat.get_dcount() > 0)
        {
            int Num = boat.get_pcount() + boat.get_dcount();
            for (int i = 0; i < Num; i++)
            {
                for (int j = 0; j < frame; j++)
                {
                    yield return null;
                }
                movePD(boat.getPD());
            }
            for (int j = 0; j < frame; j++)
            {
                yield return null;
            }
        }
        if(first.boat)
        {
            int pnum = first.pnum - second.pnum;
            int dnum = first.dnum - second.dnum;
            if(pnum > 0)
            {
                for (int i = 0; i < pnum; i++)
                {
                    movePD(right_coast.getOneP());
                }
            }
            if(dnum > 0)
            {
                for (int i = 0; i < dnum; i++)
                {
                    movePD(right_coast.getOneD());
                }
            }
            
        }
        else
        {
            int pnum = second.pnum - first.pnum;
            int dnum = second.dnum - first.dnum;
            if(pnum > 0)
            {
                for (int i = 0; i < pnum; i++)
                {
                    movePD(left_coast.getOneP());
                }
            }
            if (dnum > 0)
            {
                for (int i = 0; i < dnum; i++)
                {
                    movePD(left_coast.getOneD());
                }
            }
            
        }
        for (int i = 0; i < frame; i++)
        {
            yield return null;
        }
        moveBoat();

        int boatPDNum = boat.get_pcount() + boat.get_dcount();
        for (int i = 0; i < boatPDNum; i++)
        {
            for (int j = 0; j < frame; j++)
            {
                yield return null;
            }
            movePD(boat.getPD());
        }
        for (int j = 0; j < frame; j++)
        {
            yield return null;
        }
        cnext = true;
    }

    void initGraph()
    {
        List<status> graph = new List<status>(Graph);
        do
        {
            appendGraph.Clear();
            foreach (status s in graph)
            {
                
                if (s.boat)
                {
                    if (s.pnum >= 2)
                    {
                        status newStatus = new status(s.pnum - 2, s.dnum, false);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if (s.dnum >= 2)
                    {
                        status newStatus = new status(s.pnum, s.dnum - 2, false);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if (s.pnum >= 1)
                    {
                        status newStatus = new status(s.pnum - 1, s.dnum, false);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if (s.dnum >= 1)
                    {
                        status newStatus = new status(s.pnum, s.dnum - 1, false);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if (s.pnum >= 1 && s.dnum >= 1)
                    {
                        status newStatus = new status(s.pnum - 1, s.dnum - 1, false);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                }
                else
                {
                    if(s.pnum <= 1)
                    {
                        status newStatus = new status(s.pnum + 2, s.dnum, true);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if(s.dnum <= 1)
                    {
                        status newStatus = new status(s.pnum, s.dnum + 2, true);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if(s.pnum <= 2)
                    {
                        status newStatus = new status(s.pnum + 1, s.dnum, true);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    if(s.dnum <= 2)
                    {
                        if(s.pnum == 0 && s.dnum == 0)
                        {

                        }
                        else
                        {
                            status newStatus = new status(s.pnum, s.dnum + 1, true);
                            if (newStatus.isLegal())
                            {
                                s.children.Add(newStatus);
                                afterLegal(newStatus);
                            }
                        }
                        
                    }
                    if(s.pnum <= 2 && s.dnum <= 2)
                    {
                        status newStatus = new status(s.pnum + 1, s.dnum + 1, true);
                        if (newStatus.isLegal())
                        {
                            s.children.Add(newStatus);
                            afterLegal(newStatus);
                        }
                    }
                    
                }
            }
            graph = new List<status>(appendGraph);
            Graph.AddRange(appendGraph);
            
        } while (appendGraph.Count != 0);
        /*
        Debug.Log(Graph.Count);
        foreach (status s in Graph)
        {
            Debug.Log(s.pnum);
            Debug.Log(s.dnum);
            Debug.Log(s.boat);
        }
        */
    }

    public void afterLegal(status newStatus)
    {

        bool index = true;
        foreach (status s2 in Graph)
        {
            if (s2.equals(newStatus))
            {
                index = false;
                break;
            }
        }
        foreach(status s2 in appendGraph)
        {
            if(s2.equals(newStatus))
            {
                index = false;
                break;
            }
        }
        if (index)
            appendGraph.Add(newStatus);
    }

    public status findCurrentStatus()
    {
        int pnum = 0;
        int dnum = 0;
        bool b;
        if (!boat.get_side())
        {
            b = false;
            pnum = right_coast.get_pcount();
            dnum = right_coast.get_dcount();
        }
        else
        {
            b = true;
            pnum = right_coast.get_pcount() + boat.get_pcount();
            dnum = right_coast.get_dcount() + boat.get_dcount();
        }
        status currntStatus = new status(pnum, dnum, b);
        return currntStatus;
    }

    public void initShortestPath()
    {
        int[] dis = new int[Graph.Count];
        status src = Graph[Graph.Count - 1];
        for(int i = 0; i < dis.Length; i++)
        {
            dis[i] = 9999999;
            List<status> pa = new List<status>();
            pa.Add(src);
            path.Add(pa);
        }
        dis[Graph.IndexOf(src)] = 0;
        foreach(status s in src.children)
        {
            dis[Graph.IndexOf(s)] = 1;
        }
        
        visited.Add(src);
        while(visited.Count != Graph.Count)
        {
            status shortest = rootstatus;
            int shortdis = 99999999;
            for(int i = 0; i < dis.Length; i++)
            {
                if(shortdis > dis[i] && notVisit(Graph[i]))
                {
                    shortest = Graph[i];
                    shortdis = dis[i];
                }
            }
            
            visited.Add(shortest);
            foreach(status s in shortest.children)
            {
                if(notVisit(s) && dis[Graph.IndexOf(s)] > dis[Graph.IndexOf(shortest)] + 1)
                {
                    dis[Graph.IndexOf(s)] = dis[Graph.IndexOf(shortest)] + 1;
                    path[Graph.IndexOf(s)].Clear();
                    path[Graph.IndexOf(s)].AddRange(path[Graph.IndexOf(shortest)]);
                    path[Graph.IndexOf(s)].Add(shortest);
                }
            }
        }
        
        /*
        for (int i = 0; i < dis.Length; i++)
        {
            string str = "";
            str += Graph[i].pnum.ToString() + "P" + Graph[i].dnum.ToString() + "D" + Graph[i].boat.ToString() + " " + dis[i].ToString();
            Debug.Log(str);
        }
        */
        foreach(List<status> ls in path)
        {
            ls.Reverse();
        }
        /*
        foreach(List<status> ls in path)
        {
            string str = "";
            
            foreach (status s in ls)
            {
                str += s.pnum.ToString() + "P" + s.dnum.ToString() + "D" + s.boat.ToString() + "b ->";
            }
            Debug.Log(str);

        }
        */
    }

    public bool notVisit(status other)
    {
        foreach(status s in visited)
        {
            if (s.equals(other))
                return false;
        }
        return true;
    }

    public void loadResources()
    {
        left_coast = new coast_controller(false);
        right_coast = new coast_controller(true);
        boat = new boat_controller();
        pds = new pd_controller[6];
        for (int i = 0; i < 3; i++)
        {
            pd_controller p = new pd_controller(true);
            Vector3 temp = right_coast.GetEmptyPos();
            p.set_pos(temp);
            if (temp.x < 0)
                p.pos_index = (int)-(temp.x + 6.5);
            else
                p.pos_index = (int)(temp.x - 6.5);
            p.set_coast(right_coast);
            right_coast.insert(p);
            p.set_name(i.ToString() + "priest");
            pds[i] = p;
            pd_controller d = new pd_controller(false);
            temp = right_coast.GetEmptyPos();
            d.set_pos(temp);
            if (temp.x < 0)
                d.pos_index = (int)-(temp.x + 6.5);
            else
                d.pos_index = (int)(temp.x - 6.5);
            d.set_coast(right_coast);
            right_coast.insert(d);
            d.set_name(i.ToString() + "devil");
            pds[i + 3] = d;
        }

    }

    public void moveBoat()
    {
        int side_index;
        if (boat.get_side())
            side_index = 1;
        else
            side_index = -1;
        if (boat.empty())
            return;
        else if (boat.get_boat().transform.position != new Vector3(side_index * 4.5F, 1, 0))
            return;
        else
        {
            actionManager.moveBoat(boat.get_boat(), boat.get_to_pos());
            userGui.result = check_is_win();
        }
    }

    public void movePD(pd_controller pORd)
    {
        if(boat.is_on_boat(pORd))
        {
            coast_controller coast;
            if (boat.get_side())
                coast = right_coast;
            else
                coast = left_coast;

            Vector3 temp = coast.GetEmptyPos();



            Vector3 end_pos = coast.GetEmptyPos();                                         //动作分离版本改变
            Vector3 middle_pos = new Vector3(pORd.get_pd().transform.position.x, end_pos.y, end_pos.z);  //动作分离版本改变
            actionManager.movePD(pORd.get_pd(), middle_pos, end_pos);  //动作分离版本改变
            if (coast.GetEmptyPos().x < 0)
                pORd.pos_index = (int)-(temp.x + 6.5);
            else
                pORd.pos_index = (int)(temp.x - 6.5);
            pORd.set_coast(coast);
            coast.insert(pORd);
            boat.remove(pORd);
            
            
        }
        else
        {
            if(boat.is_full())
            {
                return;
            }
            
            if(left_coast.is_on_coast(pORd))
            {
                if (boat.get_side())
                    return;
                pORd.set_boat(boat);

                Vector3 temp = boat.GetEmptyPos();
                Vector3 end_pos = temp;                                             //动作分离版本改变
                Vector3 middle_pos = new Vector3(end_pos.x, pORd.get_pd().transform.position.y, end_pos.z); //动作分离版本改变
                actionManager.movePD(pORd.get_pd(), middle_pos, end_pos);  //动作分离版本改变
                if (temp.x < 0)
                    pORd.boat_pos_index = (int)-(temp.x + 3.5);
                else
                    pORd.boat_pos_index = (int)(temp.x - 3.5);
                left_coast.remove(pORd);
                boat.insert(pORd);
               
                
            }
            else
            {
                if (!boat.get_side())
                    return;
                
                right_coast.remove(pORd);
                Vector3 temp = boat.GetEmptyPos();
                Vector3 end_pos = temp;                                             //动作分离版本改变
                Vector3 middle_pos = new Vector3(end_pos.x, pORd.get_pd().transform.position.y, end_pos.z); //动作分离版本改变
                actionManager.movePD(pORd.get_pd(), middle_pos, end_pos);  //动作分离版本改变
               
                if (temp.x < 0)
                    pORd.boat_pos_index = (int)-(temp.x + 3.5);
                else
                    pORd.boat_pos_index = (int)(temp.x - 3.5);
                pORd.set_boat(boat);
                boat.insert(pORd);
               
                
                
            }
        }
        userGui.result = check_is_win();
    }

    int check_is_win()
    {
        int count = 0;
        for(int i =0; i <pds.Length; i++)
        {
            if (left_coast.is_on_coast(pds[i]))
                count++;
        }
        if (count == 6) return 1;

        if(!boat.get_side())
        {
            if (left_coast.get_pcount() + boat.get_pcount() < left_coast.get_dcount() + boat.get_dcount() && left_coast.get_pcount() + boat.get_pcount() != 0)
                return 2;
            if (right_coast.get_pcount() < right_coast.get_dcount() && right_coast.get_pcount() != 0)
                return 2;
        }
        else
        {
            if (right_coast.get_pcount() + boat.get_pcount() < right_coast.get_dcount() + boat.get_dcount() && right_coast.get_pcount() + boat.get_pcount() != 0)
                return 2;
            if (left_coast.get_pcount() < left_coast.get_dcount() && left_coast.get_pcount() != 0)
                return 2;
        }
        

        return 0;
    }

    public void reset()
    {
        boat.reset();
        left_coast.reset();
        right_coast.reset();
        for (int i = 0; i < pds.Length; i++)
        {
            pds[i].reset();
        }
        for (int i = 0; i < pds.Length; i++)
        {
            Vector3 temp = right_coast.GetEmptyPos();



            pds[i].set_pos(right_coast.GetEmptyPos());  
            if (right_coast.GetEmptyPos().x < 0)
                pds[i].pos_index = (int)-(temp.x + 6.5);
            else
                pds[i].pos_index = (int)(temp.x - 6.5);
           
            right_coast.insert(pds[i]);
        }
    }

    
}
