using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game{
	public class PlatformManager : MonoBehaviour {

		[SerializeField] private GameColor gameColor = GameColor.GREY;

		[SerializeField] private Vector2[] controlPoints;

		[SerializeField] private float cycle_period;

		[SerializeField] private float speed;

		[SerializeField] private static float HEIGHT = 1.0f;

		private static float DEFAULT_WIDTH = 5.0f;

		[SerializeField] private float width = DEFAULT_WIDTH;

		[SerializeField] private Color color = Color.gray;

		private Rigidbody2D body;

		private SpriteRenderer spriteRenderer;

		// added by Gal 3/1/18
		private GameManager gameManager;
		
		public Vector2 Position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		// Use this for initialization
		void Start () {
//			Vector3 scale = new Vector3(width, HEIGHT, 1f );
//			transform.localScale = scale;
			spriteRenderer.material.color = this.color;
			
			// added by Gal 3/1/18
			gameManager = GetComponentInParent<GameManager>();
		}

		protected void Awake() {
			body = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		// Update is called once per frame
		void Update () {

		}

		public void Activate(Vector2 position, float width)
		{
			this.Position = position;
			this.width = width;

		}

		protected void FixedUpdate() {

		}

		private void SetColor(GameColor gameColor) {
			
			// added by Gal 3/1/18
			gameManager.ChangeLayer(gameObject, gameColor);
			
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
		
		
		
		

		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == "shot")
			{
				Debug.Log("PlatformManager: detected shot");
				GameColor shotColor = other.gameObject.GetComponent<ShotManager>().gameColor;
				SetColor(shotColor);
			}
		}
	}
}

