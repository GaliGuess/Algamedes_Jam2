using UnityEngine;

    public abstract class Controller : MonoBehaviour
    {
        public abstract bool look_up();
        
        public abstract bool look_down();
        
        public abstract bool turn_left();

        public abstract bool turn_right();
        
        public abstract bool jump();
        
        public abstract bool shoot();
    }
