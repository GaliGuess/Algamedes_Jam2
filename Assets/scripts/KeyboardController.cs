using UnityEngine;
    
//    [CreateAssetMenu(menuName = "Controller/Keyboard")]
    public class KeyboardController : PlayerController
    {
        public KeyCode UpKey, DownKey, LeftKey, RightKey, JumpKey, ShootKey;
        
        protected override void updateMovementParameters() {
            
            verticalDirection = Input.GetKeyDown(UpKey) || Input.GetKey(UpKey) ? Vector2.up :
                                Input.GetKeyDown(DownKey) || Input.GetKey(DownKey) ? Vector2.down :
                                Vector2.zero;
            
            
            horizontalDirection = Input.GetKeyDown(LeftKey) || Input.GetKey(LeftKey) ? Vector2.left : 
                                  Input.GetKeyDown(RightKey) || Input.GetKey(RightKey) ? Vector2.right : 
                                  Vector2.zero;
            
            jumpState = Input.GetKeyDown(JumpKey) ? true : Input.GetKeyUp(JumpKey) ? false : jumpState;
            
            shootState = Input.GetKeyDown(ShootKey);
        }
    }
