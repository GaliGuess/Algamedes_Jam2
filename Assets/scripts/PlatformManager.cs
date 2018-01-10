using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game{
	public class PlatformManager : MonoBehaviour {

		[SerializeField] private List<Transform> points;

		[SerializeField] public int beats_per_cycle = 4;

		[SerializeField] private float cycle_period;

		[SerializeField] private float segment_period;

		[SerializeField] static public int init_num_lives = 3;
		
		[SerializeField] public Framework init_platform_framework = Framework.GREY;

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
			InitState();
			UpdateFramework(init_platform_framework);
			UpdateSegmentPeriod();
		}

		public void AddPoint(GameObject point) {
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

		private void InitState() {
			platform_state.num_lives = init_num_lives;
		}

		public void UpdateSegmentPeriod() {
			cycle_period = beats_per_cycle*60.0f/GetComponentInParent<GameManager>().BPM; // seconds per cycle
			int num_of_segments = (points.Count -1)*2; //for the whole cycle
			segment_period = cycle_period/num_of_segments; //each segment will have the same period, regardless of distance of segment
		}

		// Update is called once per frame
		void Update () {
			
		}
			

		public bool V3Equal(Vector3 a, Vector3 b){
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}


		private float GetPathPercentage() {
			return (Time.time - initial_lerp_time)/segment_period;
		}

		// This is used only from editor to mock movement of platform
		public void SetPosition(float cycle_percentage) {
			int source_point_idx, target_point_idx;
			source_point_idx = Mathf.FloorToInt(cycle_percentage*(points.Count-1)*2);
			source_point_idx = source_point_idx < points.Count ? source_point_idx : 2*(points.Count-1) - source_point_idx; 
			target_point_idx = Mathf.CeilToInt(cycle_percentage*(points.Count-1)*2);
			target_point_idx = target_point_idx < points.Count ? target_point_idx : 2*(points.Count-1) - target_point_idx;
//			Debug.Log("PlatformManager idx: " + source_point_idx.ToString() + ", " + target_point_idx.ToString() );
			int num_of_paths = (points.Count-1)*2;
			float path_percentage = cycle_percentage*num_of_paths % 1;
			Vector2 pos = Vector2.Lerp(points[source_point_idx].position, points[target_point_idx].position, path_percentage);
			transform.position = pos;
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

		public void UpdateHit(Framework platform_framework) {
			platform_state.num_lives--;
			if(platform_state.num_lives <= 0) {
				platform_state.num_lives = init_num_lives;
				UpdateFramework(platform_framework);
			}
		}

		// Updates platform state/view
		public void SetFramework(Framework platform_framework) {
			GetComponent<PlatformState>().platform_framework = platform_framework;
			GetComponent<PlatformView>().SetColor(platform_framework);
		}

		// This updates the new framework within game_manager as well as updating platform state/view
		private void UpdateFramework(Framework platform_framework) {
			SetFramework(platform_framework);
			game_manager.ChangeLayer(gameObject, platform_framework);
		}


	}
}

