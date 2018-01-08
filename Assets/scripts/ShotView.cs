using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game{
	public class ShotView : MonoBehaviour {

		private Color color = Color.gray;

		private Rigidbody2D body;

		private SpriteRenderer sprite_renderer;

		private ShotState shot_state;

		public Color ShotColor{
			get {
				return this.color;
			}
			set {
				this.color = value;
				sprite_renderer.material.color = this.color;
			}
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		protected void Awake() {
			body = GetComponent<Rigidbody2D>();
			sprite_renderer = GetComponent<SpriteRenderer>();
			shot_state = GetComponent<ShotState>();
		}

		public void SetVelocity(Vector2 velocity) {
			body.velocity = velocity;
		}

		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "platform")
			{
				//				Debug.Log("ShotManager: detected platform");
				Framework framework = other.gameObject.GetComponent<PlatformState>().platform_framework;
				if (framework != shot_state.shot_framework) {
					Destroy(gameObject);
				}

			}
		}

		public void SetColor(Framework shot_framework) {
			switch (shot_framework) {
			case Framework.BLACK :
				this.ShotColor = Color.black;
				break;
			case Framework.WHITE :
				this.ShotColor = Color.white;
				break;
			default :
				this.ShotColor = Color.gray;
				break;
			}
		}

	}
}

