using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controllers;

namespace Controllers
{
    public class KeyboardController : Controller
    {
        public String HorizontalAxis = "B1_Horizontal",
            VerticalAxis = "B1_Vertical",
            JumpAxis = "B1_Jump",
            ShootAxis = "B1_Fire";

        public Vector2 direction;
        private bool isJumping, isShooting, isGettingDown;
        public bool autoFire, autoJump, GettingDownEnabled = true;

        protected override void Update()
        {
            direction.x = Input.GetAxis(HorizontalAxis);
            direction.y = Input.GetAxis(VerticalAxis);
            isShooting = autoFire ? Input.GetButton(ShootAxis) : Input.GetButtonDown(ShootAxis);

            if (GettingDownEnabled && direction.y < 0)
            {
                isGettingDown = autoJump ? Input.GetButton(JumpAxis) : Input.GetButtonDown(JumpAxis);
            }
            else
            {
                isJumping = autoJump ? Input.GetButton(JumpAxis) : Input.GetButtonDown(JumpAxis);
            }
            
            base.Update();
        }

        protected override float update_moving_direction()
        {
            return direction.x;
        }

        protected override Vector2 update_aim_direction()
        {
            return direction;
        }

        public override bool jump()
        {
            return isJumping;
        }

        public override bool shoot()
        {
            return isShooting;
        }

        public override bool getDown()
        {
            return isGettingDown;
        }
    }
}
