using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game{
	public class PlatformManager : MonoBehaviour {

		[SerializeField] private List<Transform> points;

		[SerializeField] private float cycle_period = 2.0f;

		private int target_point_idx = 0;

		private int current_point_idx = 0;

		private bool reverse_dir = false;

		private float initial_lerp_time;

		private GameManager game_manager;

		private PlatformState platform_state;
		
		private PlatformView platform_view;


		void Awake() {
			game_manager = GetComponentInParent<GameManager>();
			platform_state = GetComponent<PlatformState>();
			platform_view = GetComponent<PlatformView>();
		}

		// Use this for initialization
		void Start () {
			InitPath();
		}

		public void AddPoint() {
			GameObject point = new GameObject();
			point.transform.position = transform.position;
			point.transform.parent = transform.parent.Find("PlatformPath");
			point.name = "point" + (points.Count+1).ToString();
			points.Add(point.transform);
		}


		private void InitPath() {
			initial_lerp_time = Time.time;
			if (points.Count > 0) {
				transform.position = points[0].position;
				current_point_idx = 0;
			}
			if (points.Count > 1) {
				target_point_idx = 1;
			}
			reverse_dir = (points.Count<=2);
		}


		// Update is called once per frame
		void Update () {
			
		}
			

		public bool V3Equal(Vector3 a, Vector3 b){
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}


		private float GetPathPercentage() {
			return (Time.time - initial_lerp_time)/cycle_period;
		}


		protected void FixedUpdate() {
			if (points.Count != 0) {
				float path_percentage = GetPathPercentage();
				if (path_percentage <= 1.0f) {
					Vector2 pos = Vector2.Lerp(points[current_point_idx].position, points[target_point_idx].position, path_percentage);
					platform_state.Position = pos;
				} 
				else {
					current_point_idx = target_point_idx;
					target_point_idx = reverse_dir ? target_point_idx - 1 : target_point_idx + 1;
					if (target_point_idx == 0 || target_point_idx == points.Count-1) {
						reverse_dir = !reverse_dir;
					}
					initial_lerp_time = Time.time;
				}
			}
		}


		public void SetColor(GameColor platform_color) {
			game_manager.ChangeLayer(gameObject, platform_color);
			
			platform_state.PlatformColor = platform_color;
			switch (platform_color) {
				case GameColor.BLACK :
					platform_view.PlatformColor = Color.black;
					break;
				case GameColor.WHITE :
					platform_view.PlatformColor = Color.white;
					break;
				default :
					platform_view.PlatformColor = Color.grey;
					break;
			}
		}


	}
}

