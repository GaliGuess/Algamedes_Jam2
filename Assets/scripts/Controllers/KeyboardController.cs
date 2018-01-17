using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
    

    public class KeyboardController : Controller
    {   
        public String HorizontalAxis = "Horizontal",
                      VerticalAxis = "Vertical", 
                      JumpAxis = "Jump", 
                      ShootAxis = "Fire1";
     
        private bool isHorizontal, isVertical, isJumping, isShooting;
        
        void Update()
        {
            isHorizontal = Input.GetButton(HorizontalAxis);
            isVertical = Input.GetButton(VerticalAxis);
            isJumping = Input.GetButtonDown(JumpAxis);
            isShooting = Input.GetButtonDown(ShootAxis);
        }
        
        public override bool look_up()
        {
            return isVertical && Input.GetAxis(VerticalAxis) > 0;
        }

        public override bool look_down()
        {
            return isVertical && Input.GetAxis(VerticalAxis) < 0;
        }

        public override bool turn_left()
        {
            return isHorizontal && Input.GetAxis(HorizontalAxis) < 0;
        }

        public override bool turn_right()
        {
            return isHorizontal && Input.GetAxis(HorizontalAxis) > 0;
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
