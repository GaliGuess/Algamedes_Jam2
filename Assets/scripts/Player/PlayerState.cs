using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerState : MonoBehaviour {

		[SerializeField] public Framework player_framework = Framework.BLACK;

		//	[HideInInspector]
		public GameObject currentPlatform;

		// TODO: add all the relative information!
	}

}
