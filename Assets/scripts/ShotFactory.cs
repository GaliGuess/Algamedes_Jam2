using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class ShotFactory : MonoBehaviour {

		[SerializeField] GameObject shotPrefab;

		public GameObject MakeObject(Vector2 position, float rotation, GameColor gameColor){
			GameObject clone = (GameObject)Instantiate(shotPrefab);
			clone.GetComponent<ShotManager>().Activate(position, rotation, gameColor);
			return clone;
		}
	}

}
