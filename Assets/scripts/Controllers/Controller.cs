using UnityEngine;

namespace Controllers
{
    // IMPORTANT: notice this class has Start & Update functions.
    // These are not automatically called from the child so you have to manually call them.
    // Call base.Start before the child's start.
    // Call base.Update in the child after the child's Update.
    
    public abstract class Controller : MonoBehaviour
    {
        public Vector2 movingDirection, aimDirection, lastNonZeroDirection;

        // abstract methods
        protected abstract float update_moving_direction();
        
        protected abstract Vector2 update_aim_direction();
        
        public abstract bool jump();

        public abstract bool shoot();
        
        
        protected virtual void Start()
        {
            lastNonZeroDirection = GetComponent<Rigidbody2D>().position.x < 0 ? Vector2.right : Vector2.left;
        }

        protected virtual void Update()
        {
            // saving last x direction that is not zero. used for shooting.
            if (aimDirection != Vector2.zero)
            {
                lastNonZeroDirection = aimDirection;
            }
            aimDirection = update_aim_direction();
            movingDirection.x = update_moving_direction();
            movingDirection = moving_direction().normalized;
        }

        public Vector2 moving_direction()
        {
            return movingDirection;
        }

        public Vector2 aim_direction()
        {
            return aimDirection == Vector2.zero ? lastNonZeroDirection : aimDirection;
        }
    }
}