using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
	public class PlatformFactory : MonoBehaviour {

		static GameObject platformPrefab;

		static public GameObject MakeObject(){
			GameObject clone = (GameObject)Instantiate(platformPrefab);
			return clone;
		}

	}

}
