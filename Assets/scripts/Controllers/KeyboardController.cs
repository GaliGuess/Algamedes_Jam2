using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controllers;

namespace Controllers
{
    public class KeyboardController : Controller
    {
        public String HorizontalAxis = "Horizontal",
            VerticalAxis = "Vertical",
            JumpAxis = "Jump",
            ShootAxis = "Fire1";

        public Vector2 direction;
        private bool isJumping, isShooting;

        protected override void Update()
        {
            direction.x = Input.GetAxis(HorizontalAxis);
            direction.y = Input.GetAxis(VerticalAxis);
            isJumping = Input.GetButtonDown(JumpAxis);
            isShooting = Input.GetButtonDown(ShootAxis);

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
    }
}
