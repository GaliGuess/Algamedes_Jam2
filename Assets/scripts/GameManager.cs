using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

namespace Game {

	public enum Framework {GREY, BLACK, WHITE}


	public class GameManager : MonoBehaviour {

		private ShotFactory shotFactory;

		// Use this for initialization
		void Start () {
			shotFactory = GetComponentInParent<ShotFactory>();
		}

		// Update is called once per frame
		void Update () {

		}
			
		public void SpawnShot(Vector2 position, Vector2 startVelocity, float rotation, Framework framework) {
			GameObject shot = shotFactory.MakeObject(position, startVelocity,rotation,framework);
		}

		public void ChangeLayer(GameObject obj, Framework framework)
		{
			if (obj.CompareTag("platform")) obj.layer = framework == Framework.BLACK ? 13 : framework == Framework.GREY ? 14 : 15;
			else if (obj.CompareTag("player")) obj.layer = framework == Framework.BLACK ? 17 : 18;
		}
		
	}
}
