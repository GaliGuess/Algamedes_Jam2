using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game{
	public class ShotView : MonoBehaviour {

		private Color color = Color.gray;

		private Rigidbody2D body;

		private SpriteRenderer sprite_renderer;

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
		}

		public void SetVelocity(Vector2 velocity) {
			body.velocity = velocity;
		}

		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "platform")
			{
				//				Debug.Log("ShotManager: detected platform");
				Destroy(gameObject);
			}
		}

	}
}

