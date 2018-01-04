using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

namespace Game {

	public enum GameColor {GREY, BLACK, WHITE}


	public class GameManager : MonoBehaviour {

		private ShotFactory shotFactory;

		// Use this for initialization
		void Start () {

			shotFactory = GetComponentInParent<ShotFactory>();

		}

		// Update is called once per frame
		void Update () {

		}
			
		public void SpawnShot(Vector2 position, Vector2 startVelocity, float rotation, GameColor gameColor) {
			GameObject shot = shotFactory.MakeObject(position, startVelocity,rotation,gameColor);
		}

		public void ChangeLayer(GameObject obj, GameColor color)
		{
			if (obj.CompareTag("platform")) obj.layer = color == GameColor.BLACK ? 13 : color == GameColor.GREY ? 14 : 15;
			else if (obj.CompareTag("player")) obj.layer = color == GameColor.BLACK ? 17 : 18;
		}
		
	}
}
