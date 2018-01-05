using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Utils;

namespace Game {
	public class ShotManager : MonoBehaviour {

		[SerializeField] public GameColor gameColor;

		[SerializeField] private float speed = 10f;

		private Color color = Color.gray;

		private Rigidbody2D body;

		private SpriteRenderer spriteRenderer;

		[SerializeField] public float Rotation {
			get {
				return transform.eulerAngles.z;
			}
			set {
				var rot = transform.eulerAngles;
				rot.z = value;
				transform.eulerAngles = rot;
			}
		}

		[SerializeField]  public Vector2 Position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		[SerializeField]  public Vector2 Forward {
			get {
				return Vector2.right.Rotate(Rotation * Mathf.Deg2Rad);
			}
			set {
				transform.position = value;
			}
		}

		protected void Awake() {
			body = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		// Use this for initialization
		void Start () {
			body.AddForce(Forward.GetWithMagnitude(speed));
		}

//		public void Activate(Vector2 position, float rotation, GameColor gameColor)
//		{
//			this.Position = position;
//			this.Rotation = rotation;
//			SetColor(gameColor);
//		}
		
		public void Activate(Vector2 position, Vector2 startVelocity, float rotation, GameColor gameColor)
		{
			this.Position = position;
			this.Rotation = rotation;
			
			Vector2 dir = Quaternion.Euler(0,0,rotation) * Vector2.right;
			body.velocity = startVelocity + dir * speed;
			SetColor(gameColor);
		}

		private void SetColor(GameColor gameColor) {
			this.gameColor = gameColor;
			switch (gameColor) {
			case GameColor.BLACK :
				this.color = Color.black;
				break;
			case GameColor.WHITE :
				this.color = Color.white;
				break;
			default :
				this.color = Color.gray;
				break;
			}
			spriteRenderer.material.color = this.color;
		}

		// Update is called once per frame
		void Update () {

		}

		protected void FixedUpdate() {
//			MoveForward(this.speed);
		}

		public void MoveForward(float speed) {
			Vector2 direction = Forward.GetWithMagnitude(speed);
			Position += direction;
		}

		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "platform")
			{
				Debug.Log("ShotManager: detected platform");
				Destroy(gameObject);
			}
		}
	}
}

