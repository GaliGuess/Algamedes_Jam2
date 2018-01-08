using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class PlatformState : MonoBehaviour {

		[SerializeField] private Framework framework = Framework.GREY;

		[SerializeField] private static float HEIGHT = 1.0f;

		private static float DEFAULT_WIDTH = 5.0f;

		[SerializeField] private float width = DEFAULT_WIDTH;

		public Vector2 Position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		public Framework PlatformFramework {
			get {
				return this.framework;
			}
			set {
				this.framework = value;
			}
		} 

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
	}
}

