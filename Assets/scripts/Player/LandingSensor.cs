using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class LandingSensor : MonoBehaviour {

		private PlayerView player_view;
		private PlayerManager player_manager;

		public GameObject player;
		private Rigidbody2D body;

		public bool colliding;

		void Awake() {
			player_view = GetComponentInParent<PlayerView>();
			player_manager = GetComponentInParent<PlayerManager>();
			
			// adding rigid body from a manulally inputed gameobject as otherwise it gets the landing sensor's rigidbody
			// Adding a rigidbody to the landingSensor solved the gettingDown form platform bug.
//			body = GetComponentInParent<Rigidbody2D>();
			body = player.GetComponent<Rigidbody2D>();
	
		}

		// Use this for initialization
		void Start () {
			
		}

		// Update is called once per frame
		void Update ()
		{
			if (body.velocity.y <= -1 && colliding) {
				Debug.Log("LandingSensor: isLanding = true");
				player_view.isLanding = true;
			} else if (player_manager.isGrounded) {
				Debug.Log("LandingSensor: isLanding = false");
				player_view.isLanding = false;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(Values.PLATFORM_BODY_TAG))
			{
//				player_view.isLanding = true;
				colliding = true;
			}
		}

//		private void OnTriggerStay2D(Collider2D other)
//		{
//			if (other.CompareTag(Values.PLATFORM_BODY_TAG))
//			{
//				colliding = true;
//			}
//		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(Values.PLATFORM_BODY_TAG))
			{
				colliding = false;
			}
		}
			

		void FixedUpdate()
		{
//			if (player_manager.isGrounded) {
//				player_view.isLanding = false;
//			}
//			colliding = false;
		}


	}

}
