using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class LandingSensor : MonoBehaviour {

		private PlayerView player_view;
		private PlayerManager player_manager;

		void Awake() {
			player_view = GetComponentInParent<PlayerView>();
			player_manager = GetComponentInParent<PlayerManager>();
		}

		// Use this for initialization
		void Start () {
			
		}

		// Update is called once per frame
		void Update () {

		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(Values.PLATFORM_BODY_TAG))
			{
				player_view.isLanding = true;
			}
		}

		void FixedUpdate()
		{
			if (player_manager.isGrounded) {
				player_view.isLanding = false;
			}
		}


	}

}
