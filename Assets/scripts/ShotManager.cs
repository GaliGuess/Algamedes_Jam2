using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Utils;

namespace Game {
	public class ShotManager : MonoBehaviour {

		[SerializeField] private float speed = 10f;

		private ShotState shot_state;

		private ShotView shot_view;

		void Awake() {
			shot_state = GetComponent<ShotState>();
			shot_view = GetComponent<ShotView>();
		}

		// Use this for initialization
		void Start () {
			
		}
		
		public void Activate(Vector2 position, Vector2 startVelocity, float rotation, GameColor gameColor)
		{
			shot_state.Position = position;
			shot_state.Rotation = rotation;
			
			Vector2 dir = Quaternion.Euler(0,0,rotation) * Vector2.right;
			Vector2 velocity = startVelocity + dir * speed;
			shot_state.Velocity = velocity;
			shot_view.SetVelocity(velocity);
			SetColor(gameColor);
		}

		private void SetColor(GameColor shot_color) {
			shot_state.shot_color = shot_color;
			switch (shot_color) {
			case GameColor.BLACK :
				shot_view.ShotColor = Color.black;
				break;
			case GameColor.WHITE :
				shot_view.ShotColor = Color.white;
				break;
			default :
				shot_view.ShotColor = Color.gray;
				break;
			}
		}

		// Update is called once per frame
		void Update () {

		}


	}
}

