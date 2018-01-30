﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Controllers
{
	public class PS4Controller : Controller
	{
		[SerializeField]
		public int JoystickNumber = 1;  // for 2 joystick support

		[SerializeField, Tooltip("Allows for jumping with up DPad + Left Analog Stick\nin Addition to other jump controls.")] 
		public bool JumpUsingVerticalMovement = false;

		public bool AimWithMovement;
		
		public bool AutoFire = false;
		public bool AutoJumping = false;
		
		public String[] HorizontalMovementControls = {"J1_PS4_LeftStick_Horizontal", "J1_PS4_DPad_Horizontal"},
						VerticalMovementControls = {"J1_PS4_LeftStick_Vertical", "J1_PS4_DPad_Vertical"};

		public String[] HorizontalAimControls = {"J1_PS4_RightStick_Horizontal"},
						VerticalAimControls = {"J1_PS4_RightStick_Vertical"};

		public String[] JumpControls = {"J1_PS4_X", "J1_PS4_L1", "J1_PS4_L2", "J1_PS4_L3"},
						ShootControls = {"J1_PS4_Square", "J1_PS4_R1", "J1_PS4_R2", "J1_PS4_R3"};

		private float ANALOG_MOVE_THRESHOLD = 0.3f;
		private float ANALOG_AIM_THRESHOLD = 0.3f;
		private float ANALOG_JUMP_THRESHOLD = 0.6f;
		
		private bool isJumping, isShooting;
		private float _moving_direction;
		private Vector2 _aim_direction;

		// Will be used when implementing 2nd controller
		private void Awake()
		{
			addJoystickNumber();

//			var names = Input.GetJoystickNames();
//			for (int i = 0; i < names.Length; i++)
//			{
//				Debug.Log(names[i]);
//			}
		}

		protected override void Update()
		{	
			// Updating Aiming direction
			Vector2 tempAimDirection = _aim_direction;
			foreach (var key in HorizontalAimControls)
			{
				tempAimDirection.x = Input.GetAxis(key);
			}
			foreach (var key in VerticalAimControls)
			{
				tempAimDirection.y = Input.GetAxis(key);
			}
			if (tempAimDirection.magnitude > ANALOG_AIM_THRESHOLD)
			{
				_aim_direction = tempAimDirection.normalized;
			}
			
			// Updating moving direction
			bool moveDirectionChanged = false;
			foreach (var key in HorizontalMovementControls)
			{
				float tempMoveDirection = Input.GetAxis(key);
				if (Mathf.Abs(tempMoveDirection) > ANALOG_MOVE_THRESHOLD)
				{
					_moving_direction = Mathf.Sign(tempMoveDirection);
					moveDirectionChanged = true;
				}
			}
			if (!moveDirectionChanged) _moving_direction = 0f;
			
			// updating aim with movement input if needed
			if (AimWithMovement && tempAimDirection == Vector2.zero)
			{
				foreach (var key in VerticalMovementControls)
				{
					float tempYDirection = -Input.GetAxis(key);
					if (Mathf.Abs(tempYDirection) > ANALOG_MOVE_THRESHOLD)
					{
						tempAimDirection.y = Mathf.Sign(tempYDirection);
					}
				}
				tempAimDirection.x = _moving_direction;
				
				if (tempAimDirection.magnitude > ANALOG_AIM_THRESHOLD)
				{
					_aim_direction = tempAimDirection.normalized;
				}
			}

			// Updating jumping
			isJumping = false;
			foreach (var key in JumpControls)
			{
				var keyPress = AutoJumping ? Input.GetButton(key) : Input.GetButtonDown(key);
				isJumping = isJumping || keyPress;
			}
			if (JumpUsingVerticalMovement)
			{
				foreach (var key in VerticalMovementControls)
				{
					isJumping = isJumping || Input.GetAxis(key) < -ANALOG_JUMP_THRESHOLD;
				}
			}
			
			// Updating Shooting
			isShooting = false;
			foreach (var key in ShootControls)
			{
				var keyPress = AutoFire ? Input.GetButton(key) : Input.GetButtonDown(key);
				isShooting = isShooting || keyPress;
			}

			base.Update();
		}

		protected override float update_moving_direction()
		{
			return _moving_direction;
		}

		protected override Vector2 update_aim_direction()
		{
			return _aim_direction;
		}

		public override bool jump()
		{
			return isJumping;
		}

		public override bool shoot()
		{
			return isShooting;
		}


		// Will be used for assigning each player his own controller
		private void addJoystickNumber()
		{
			String toAdd = "J" + JoystickNumber + "_";

			for (int i = 0; i < HorizontalMovementControls.Length; i++)
			{
				HorizontalMovementControls[i] = toAdd + HorizontalMovementControls[i];
			}

			for (int i = 0; i < VerticalMovementControls.Length; i++)
			{
				VerticalMovementControls[i] = toAdd + VerticalMovementControls[i];
			}

			for (int i = 0; i < HorizontalAimControls.Length; i++)
			{
				HorizontalAimControls[i] = toAdd + HorizontalAimControls[i];
			}

			for (int i = 0; i < VerticalAimControls.Length; i++)
			{
				VerticalAimControls[i] = toAdd + VerticalAimControls[i];
			}

			for (int i = 0; i < JumpControls.Length; i++)
			{
				JumpControls[i] = toAdd + JumpControls[i];
			}

			for (int i = 0; i < ShootControls.Length; i++)
			{
				ShootControls[i] = toAdd + ShootControls[i];
			}
		}
	}
}