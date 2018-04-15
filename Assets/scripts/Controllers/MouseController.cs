using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Controllers
{
	public class MouseController : Controller
	{
		public String HorizontalAxis = "W1_Horizontal",
		VerticalAxis = "W1_Vertical",
		JumpAxis = "Mouse_Jump",
		ShootAxis = "Mouse_Fire";

		public Vector2 _moving_direction;
		public Vector2 _aim_direction;
		private bool isJumping, isShooting, isGettingDown;
		public bool autoFire, autoJump;

		private void Awake()
		{
			Cursor.visible = false;
		}

		protected override void Update()
		{
			_moving_direction.x = Input.GetAxis(HorizontalAxis);
			_moving_direction.y = Input.GetAxis(VerticalAxis);
			isShooting = autoFire ? Input.GetButton(ShootAxis) : Input.GetButtonDown(ShootAxis);

			if (_moving_direction.y < 0)
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
			return _moving_direction.x;
		}

		protected override Vector2 update_aim_direction()
		{
			Vector3 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			Vector2 relativeMousePos = (Vector2)mouseViewportPos - new Vector2(0.5f, 0.5f);
			return relativeMousePos;
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
