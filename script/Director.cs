using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControl
{
    void LoadResources();
}

public interface IUserAction
{
    //void playerMoving(float x, float z);
    //bool getState();
    //void setState(bool state);
}

public class Director : System.Object
{

    public ISceneControl currentSceneController { get; set; }

    private static Director director;

    private Director()
    {
        
    }

    public static Director getInstance()
    {
        if (director == null)
        {
            director = new Director();
        }
        return director;
    }
}
