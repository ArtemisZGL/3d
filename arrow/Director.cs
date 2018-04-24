using arrow.control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow.director
{
    public interface ISceneControl
    {
        void LoadResources();
    }

    public interface IUserAction
    {
        int get_score();
        void pull(float Mx, float My);
        void shot();
        void reset_score();
        void set_state(GameState s);
        GameState get_state();
        void loadArrow();
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
}
