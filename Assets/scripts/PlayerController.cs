using UnityEngine;

    public abstract class PlayerController : MonoBehaviour {

        protected Vector2 verticalDirection;
        protected Vector2 horizontalDirection;
        protected bool jumpState = false;
        protected bool shootState = false;
        private Vector2 lastHorizontalDirection;
    
        
        protected abstract void updateMovementParameters();

        
        public void updatePlayerAction()
        {
            if (horizontalDirection != Vector2.zero)
            {
                lastHorizontalDirection = horizontalDirection;
            }
            updateMovementParameters();
        }

        
        public Vector2 getLookingDirection()
        {
            if (horizontalDirection == Vector2.zero && verticalDirection == Vector2.zero)
            {
                return lastHorizontalDirection;
            }
            return horizontalDirection + verticalDirection;
        }

        
        public Vector2 getMovementDirection()
        {
            return horizontalDirection;
        }
        
        
        public bool isJumping()
        {
            return jumpState;
        }
        
        
        public bool isShooting()
        {
            return shootState;
        }
    }
