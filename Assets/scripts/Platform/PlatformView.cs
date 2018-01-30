using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class PlatformView : MonoBehaviour {

		public bool useSensorAsTrigger;

		private PlatformManager platform_manager;

		public PlatformState platform_state;

		public Rigidbody2D body;

		private SpriteRenderer sprite_renderer;

		private List<Rigidbody2D> carried_bodies = new List<Rigidbody2D>();

		public Vector2 Position{
			get {
				return body.position;
			}
			set {
				body.position = value;
			}
		}

		public Vector2 Velocity{
			get {
				return body.velocity;
			}
			set {
				body.velocity = value;
			}
		}

		private Vector2 last_position;

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
			last_position = Position;
			body.isKinematic = true;
//			if (useSensorAsTrigger) {
//				foreach(PlatformSensor sensor in GetComponentsInChildren<PlatformSensor>() ) {
//					
//					sensor.platform_view = this;
//				}
//			}

		}

		// Update is called once per frame
		void Update () {

		}

		void LateUpdate() {
			Velocity = Position - last_position;
			foreach(Rigidbody2D body in carried_bodies) {
//				Debug.Log("PlatformView: moving carried body " + body.transform + ", " + Velocity);
				body.transform.Translate(Velocity);
//				body.transform.parent = transform;
			}
			last_position = Position;
		}

		protected void Awake() {
			Init();
		}

		public void Init() {
			body = GetComponent<Rigidbody2D>();
			sprite_renderer = GetComponent<SpriteRenderer>();
			platform_manager = GetComponentInParent<PlatformManager>();
			platform_state = GetComponentInParent<PlatformState>();

			if (transform.Find("PlatformSensor") == null) {
				AddCarrierSensor();
			}

			if (transform.Find("PlatformShotSensor") == null) {
				AddShotSensor();
			}

			PlatformSensor carrier_sensor = GetComponentInChildren<PlatformSensor>();
			carrier_sensor.Init();
			PlatformShotSensor shot_sensor = GetComponentInChildren<PlatformShotSensor>();
			shot_sensor.Init();
		}

		private void AddShotSensor() {
//			Debug.Log("platform body: create shot sensor");
			GameObject shot_sensor_instance = Instantiate(Resources.Load("PlatformShotSensor"), transform.position, transform.rotation) as GameObject;
			shot_sensor_instance.transform.parent = transform;
			shot_sensor_instance.name = "PlatformShotSensor";
			shot_sensor_instance.GetComponent<PlatformShotSensor>().Init();
			BoxCollider2D box = shot_sensor_instance.GetComponent<BoxCollider2D>();
			box.size = transform.localScale;
		}

		private void AddCarrierSensor() {
//			Debug.Log("platform body: create carrier sensor");
			GameObject carrier_sensor_instance = Instantiate(Resources.Load("PlatformSensor"), transform.position, transform.rotation) as GameObject;
			carrier_sensor_instance.transform.parent = transform;
			carrier_sensor_instance.name = "PlatformSensor";
			carrier_sensor_instance.GetComponent<PlatformSensor>().Init();
			EdgeCollider2D collider = carrier_sensor_instance.GetComponent<EdgeCollider2D>();
		}
			
		public void UpdateHit(Framework framework) {
			if (framework != platform_state.platform_framework) {
				platform_manager.UpdateHit(framework);
			}
		}

		void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.tag == Values.SHOT_TAG)
			{
//				Debug.Log("PlatformManager: detected shot");
				Framework framework = other.gameObject.GetComponent<ShotState>().shot_framework;
				UpdateHit(framework);
			}
			if (other.gameObject.tag == Values.PLAYER_TAG)
			{
				if (!useSensorAsTrigger) {
					Rigidbody2D rb = other.collider.GetComponent<Rigidbody2D>();

					if (rb != null) {
						Add(rb);
					}
				}

//				Debug.Log("PlatformManger: detected player");
			}
		}

		void OnCollisionExit2D(Collision2D other) {
			if (other.gameObject.tag == Values.PLAYER_TAG)
			{
				if (!useSensorAsTrigger) {
					Rigidbody2D rb = other.collider.GetComponent<Rigidbody2D>();
					if (rb != null) {
						Remove(rb);
					}
				}
				//				Debug.Log("PlatformManger: detected player");
			}
		}

		public void Add(Rigidbody2D rb) {
			if (!carried_bodies.Contains(rb)) {
				carried_bodies.Add(rb);
			}
		}

		public void Remove(Rigidbody2D rb) {
			if (carried_bodies.Contains(rb)) {
				carried_bodies.Remove(rb);
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

		public void Show() {
			sprite_renderer.enabled = true;
		}

		public void Hide() {
			sprite_renderer.enabled = false;

		}
	}
}

