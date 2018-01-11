using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class PlatformView : MonoBehaviour {

		private PlatformManager platform_manager;

		private PlatformState platform_state;

		private Rigidbody2D body;

		private SpriteRenderer sprite_renderer;

		[SerializeField] private Color color = Color.gray;

		public Color PlatformColor{
			get {
				return this.color;
			}
			set {
				this.color = value;
				if (sprite_renderer == null) {
					sprite_renderer = GetComponent<SpriteRenderer>();
				}
				sprite_renderer.material.color = this.color;
			}
		}

		// Use this for initialization
		void Start () {
			sprite_renderer.material.color = this.color;
		}

		// Update is called once per frame
		void Update () {

		}

		protected void Awake() {
			body = GetComponent<Rigidbody2D>();
			sprite_renderer = GetComponent<SpriteRenderer>();
			platform_manager = GetComponent<PlatformManager>();
			platform_state = GetComponent<PlatformState>();
		}


		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "shot")
			{
//				Debug.Log("PlatformManager: detected shot");
				Framework framework = other.gameObject.GetComponent<ShotState>().shot_framework;
				if (framework != platform_state.platform_framework) {
					platform_manager.UpdateHit(framework);
				}
			}
			if (other.gameObject.tag == "player")
			{
				//				Debug.Log("PlatformManger: detected player");
			}
		}

		public void SetColor(Framework platform_framework) {
			switch (platform_framework) {
			case Framework.BLACK :
				this.PlatformColor = Color.black;
				break;
			case Framework.WHITE :
				this.PlatformColor = Color.white;
				break;
			default :
				this.PlatformColor = Color.grey;
				break;
			}
		}
	}
}

