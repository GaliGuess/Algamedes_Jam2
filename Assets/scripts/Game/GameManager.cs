using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//using System;

namespace Game {

	public enum Framework {GREY, BLACK, WHITE}

	public class GameManager : MonoBehaviour {

		private GameView _gameView;
		private GameState _gameState;
		private ShotFactory _shotFactory;

		private int white_platforms_layer, black_platforms_layer, grey_platforms_layer,
					white_player_layer, black_player_layer;

		[SerializeField]
		public string gameSceneName;
		
		[SerializeField]
		public float secondsToNewRound = 3f;

		[SerializeField]
		public int BPM = 140;
		
		void Awake ()
		{
			_gameState = GetComponent<GameState>();
			_gameView = GetComponent<GameView>();
			_shotFactory = GetComponent<ShotFactory>();
			
			UpdateLayerNames();	// must happen in Awake otherwise platforms are set to Default layer
		}

		private void Start()
		{
				
		}

		public void MockPlatformsAtBeat(int beat_num) {
			GameObject[] platforms = GameObject.FindGameObjectsWithTag(Values.PLATFORM_TAG); //TODO: change tags of platformBody from "platform" to "platformBody" and add tag "platform" to platform
			foreach (GameObject platform in platforms) {
				PlatformManager platform_manager = platform.GetComponentInParent<PlatformManager>();
				platform_manager.SetPosition(((float)beat_num/(float)platform_manager.beats_per_cycle)%1);
			}
		}

		public void SpawnShot(Vector2 position, Vector2 startVelocity, float rotation, Framework framework) {
			GameObject shot = _shotFactory.MakeObject(position, startVelocity,rotation,framework);
		}
		
		
		private void UpdateLayerNames()
		{
			white_platforms_layer = LayerMask.NameToLayer(Values.WHITE_PLATFORM_LAYER);
			black_platforms_layer = LayerMask.NameToLayer(Values.BLACK_PLATFORM_LAYER);
			grey_platforms_layer = LayerMask.NameToLayer(Values.GREY_PLATFORM_LAYER);
			white_player_layer = LayerMask.NameToLayer(Values.WHITE_PLAYER_LAYER);
			black_player_layer = LayerMask.NameToLayer(Values.BLACK_PLAYER_LAYER);
		}

		public void ChangeLayer(GameObject obj, Framework framework)
		{
//			Debug.Log("ChangeLayer: " + obj.tag);
			if (obj.CompareTag(Values.PLATFORM_BODY_TAG)) {
				SetLayerRecursively(obj, framework);
			}
			
			else if (obj.CompareTag(Values.PLAYER_TAG)) obj.layer = framework == Framework.BLACK ? black_player_layer : 
														   white_player_layer;
		}

		// Recursion is probably not a good idea
		public void SetLayerRecursively(GameObject obj, Framework framework )
		{
			obj.layer = framework == Framework.BLACK ? black_platforms_layer : 
				framework == Framework.GREY ? grey_platforms_layer : 
				white_platforms_layer;
			foreach ( Transform child in obj.transform )
			{
				SetLayerRecursively( child.gameObject, framework );
			}
		}

		public void PlayerKilled(GameObject killedPlayer)
		{
			foreach (var player in _gameState.players)
			{
				if (player.name != killedPlayer.name)
				{
					_gameState.incrementScore(player.name);
				}
			}
			_gameView.updateScore();
			reloadGame();
		}



		private void reloadGame()
		{
			StartCoroutine(waitThenReloadGame());
		}

		IEnumerator waitThenReloadGame()
		{
			yield return new WaitForSeconds(secondsToNewRound);
			SceneManager.LoadScene(gameSceneName);
		}
	}
}
