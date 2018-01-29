using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class PlatformState : MonoBehaviour {

		public Framework platform_framework = Framework.GREY;

		[SerializeField] private static float HEIGHT = 1.0f;

		private static float DEFAULT_WIDTH = 5.0f;

		public int num_lives;

		[SerializeField] private float width = DEFAULT_WIDTH;

		public Vector2 Position;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
	}
}

