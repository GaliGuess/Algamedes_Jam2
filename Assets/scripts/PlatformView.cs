using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class PlatformView : MonoBehaviour {

		private PlatformManager platform_manager;

		private Rigidbody2D body;

		private SpriteRenderer sprite_renderer;

		[SerializeField] private Color color = Color.gray;

		public Color PlatformColor{
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
			platform_manager = GetComponent<PlatformManager>();
			sprite_renderer.material.color = this.color;
		}

		// Update is called once per frame
		void Update () {

		}

		protected void Awake() {
			body = GetComponent<Rigidbody2D>();
			sprite_renderer = GetComponent<SpriteRenderer>();
		}


		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "shot")
			{
//				Debug.Log("PlatformManager: detected shot");
				GameColor shotColor = other.gameObject.GetComponent<ShotManager>().gameColor;
				platform_manager.SetColor(shotColor);
			}
			if (other.gameObject.tag == "player")
			{
				//				Debug.Log("PlatformManger: detected player");
			}
		}
	}
}

