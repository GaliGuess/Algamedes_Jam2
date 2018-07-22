using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game {
	public class SelectOnInput : MonoBehaviour {

		public EventSystem eventSystem;
		public GameObject selectedObject;

		private bool buttonSelected;

		// Use this for initialization
		void Start () {

		}

		void OnEnable() {
//			eventSystem.SetSelectedGameObject(selectedObject);
		}

		// Update is called once per frame
		void Update () {
//			Debug.Log("SelectOnInput: Horizontal: " + Input.GetAxisRaw("Horizontal"));
//			Debug.Log("SelectOnInput: Vertical: " + Input.GetAxisRaw("Vertical"));
			if ((Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Horizontal")) != 0 && buttonSelected == false) {
				Debug.Log("select button!");
//				eventSystem.SetSelectedGameObject(selectedObject);
				buttonSelected = true;
			}
		}

		private void OnDisable() {
			buttonSelected = false;
		}
	}
}

