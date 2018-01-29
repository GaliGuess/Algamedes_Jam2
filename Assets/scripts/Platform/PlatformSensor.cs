using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{

	public class PlatformSensor : MonoBehaviour {
		public PlatformView carrier;

		private static float SIDE_THRESH = 10.0f;

		private bool CheckNormalIsSide(Vector2 normal) {
			float angle = Vector3.Angle(normal, Vector3.up);
			return (Mathf.Abs(angle - 90.0f)<SIDE_THRESH);
		}

		void OnTriggerEnter2D(Collider2D ob) {
			if (ob.CompareTag(Values.PLAYER_TAG)){
//				Debug.Log("PlatformSensor: OnTriggerEnter");
				Ray ray = new Ray(ob.transform.position, (transform.position - ob.transform.position));
				bool isSide = CheckNormalIsSide(ray.direction);
				Rigidbody2D rb = ob.GetComponent<Rigidbody2D>();
				if (ob != null && rb != carrier.body && !isSide) {
					carrier.Add(rb);
				}
			}

		}

		void OnTriggerExit2D(Collider2D ob) {
			if (ob.CompareTag(Values.PLAYER_TAG)) {
//				Debug.Log("PlatformSensor: OnTriggerExit");
				Rigidbody2D rb = ob.GetComponent<Rigidbody2D>();
				if (ob != null && rb != carrier.body) {
					carrier.Remove(rb);
				}
			}

		}
	}
}

