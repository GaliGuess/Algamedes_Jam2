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
			//TODO: remove. for testing purposes
//			InvokeRepeating("SpawnShotWithDefaultParmas",3f,3f);
		}

		// Update is called once per frame
		void Update () {

		}

		//TODO: remove. for testing purposes
		private void SpawnShotWithDefaultParmas() {
			SpawnShot(new Vector2(0,-5), Random.Range(45f,135f),(GameColor)Random.Range(1, 3));
		}

		// was private
		public void SpawnShot(Vector2 position, float rotation, GameColor gameColor) {
			GameObject shot = shotFactory.MakeObject(position,rotation,gameColor);
		}

		public void ChangeLayer(GameObject obj, GameColor color)
		{
			if (obj.CompareTag("platform")) obj.layer = color == GameColor.BLACK ? 13 : color == GameColor.GREY ? 14 : 15;
			else if (obj.CompareTag("player")) obj.layer = color == GameColor.BLACK ? 17 : 18;
		}
		
	}
}
