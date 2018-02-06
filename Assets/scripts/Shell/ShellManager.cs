using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class ShellManager : MonoBehaviour {

		public float speed = 100.0f;

		private ShellView shell_view;

		void Awake() {
			shell_view = GetComponent<ShellView>();
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void Activate(Vector2 position, Vector2 startVelocity, float rotation, Framework shell_framework, Collider2D shooter_collider)
		{

			shell_view.SetShooterCollider(shooter_collider);

			shell_view.Position = position;

			Vector2 dir = Quaternion.Euler(0,0,rotation) * Vector2.right;
			Vector2 velocity = dir * speed;
			shell_view.SetVelocity(velocity);
			SetFramework(shell_framework);
			gameObject.layer = shell_framework == Framework.BLACK ? Values.SHELL_LAYER_BLACK : Values.SHELL_LAYER_WHITE; // set layer accordingly, this allows for shells to ignore platforms of same framework
		}

		private void SetFramework(Framework shell_framework) {
			shell_view.SetColor(shell_framework);

		}
	}
}

