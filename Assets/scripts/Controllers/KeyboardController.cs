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
        private bool jumpAxisInUse = true, shootAxisInUse = true;
        
        void Update()
        {
            isHorizontal = Input.GetButton(HorizontalAxis);
            isVertical = Input.GetButton(VerticalAxis);
//            isJumping = Input.GetButton(JumpAxis);
//            isShooting = Input.GetButton(ShootAxis);

            // These are made to mimic GetButtonDown's behavior but more responsive
            if (Input.GetButton(JumpAxis))
            {
                if (!jumpAxisInUse)
                {
                    isJumping = true;
                    jumpAxisInUse = true;
                }
                else isJumping = false;
            }
            else
            {
                isJumping = false;
                jumpAxisInUse = false;
            }

            if (Input.GetButton(ShootAxis))
            {
                if (!shootAxisInUse)
                {
                    isShooting = true;
                    shootAxisInUse = true;
                }
                else isShooting = false;
            }
            else
            {
                isShooting = false;
                shootAxisInUse = false;
            }
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
