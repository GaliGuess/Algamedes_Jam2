using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class ShellFactory : MonoBehaviour {

		[SerializeField] GameObject shellPrefab;

		public GameObject MakeObject(Vector2 position, Vector2 startVelocity, float rotation, Framework shot_framework, Collider2D shooterCollider){
			GameObject clone = (GameObject)Instantiate(shellPrefab);
			clone.GetComponent<ShellManager>().Activate(position, startVelocity, rotation, shot_framework, shooterCollider);
			Debug.Log("created shell at " + position.ToString());
			return clone;
		}
	}

}
