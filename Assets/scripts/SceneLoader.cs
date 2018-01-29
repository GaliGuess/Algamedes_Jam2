using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
	public class SceneLoader : MonoBehaviour {

		public void LoadScene(int index) {
			SceneManager.LoadScene(index);
		}
	}
}

