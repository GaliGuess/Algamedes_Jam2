using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game{
	public class PlatformManager : MonoBehaviour {

		[SerializeField] private GameColor gameColor = GameColor.GREY;

		[SerializeField] private Vector2[] controlPoints;

		[SerializeField] private float cycle_period;

		[SerializeField] private float speed;

		[SerializeField] private Transform[] points;

		private int target_point_idx = 0;

		private int current_point_idx = 0;

		private bool reverse_dir = false;

		private float initial_lerp_time;

		[SerializeField] private float path_time = 2.0f;

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
			SetColor(gameColor);
			initial_lerp_time = Time.time;
			if (points.Length > 0) {
				transform.position = points[0].position;
				current_point_idx = 0;
			}
			if (points.Length > 1) {
				target_point_idx = 1;
			}
			reverse_dir = (points.Length<=2);
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

		public bool V3Equal(Vector3 a, Vector3 b){
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}

		private float GetPathPercentage() {
			return (Time.time - initial_lerp_time)/path_time;
		}

		protected void FixedUpdate() {
			if (points.Length != 0) {
				float path_percentage = GetPathPercentage();
				if (path_percentage <= 1.0f) {
					Debug.Log("PlatformManager: idxs: " + current_point_idx.ToString() + ", " + target_point_idx.ToString() );
					Vector2 pos = Vector2.Lerp(points[current_point_idx].position, points[target_point_idx].position, path_percentage);
					Debug.Log("PlatformManager: moving towards next point");
					Debug.Log("PlatformManager: path_percentage - " + path_percentage.ToString());
					body.position = pos;
				} 
				else {
					Debug.Log("PlatformManager: updating idx");
					Debug.Log("PlatformManager: idxs before: " + current_point_idx.ToString() + ", " + target_point_idx.ToString() );
					current_point_idx = target_point_idx;
					target_point_idx = reverse_dir ? target_point_idx - 1 : target_point_idx + 1;
					Debug.Log("PlatformManager: idxs after: " + current_point_idx.ToString() + ", " + target_point_idx.ToString() );
					if (target_point_idx == 0 || target_point_idx == points.Length-1) {
						reverse_dir = !reverse_dir;
					}
					initial_lerp_time = Time.time;
				}
			}
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
					this.color = Color.grey;
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
			if (other.gameObject.tag == "player")
			{
//				Debug.Log("PlatformManger: detected player");
			}
		}
	}
}

