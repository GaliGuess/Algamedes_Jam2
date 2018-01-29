using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game{
	public class GameView : MonoBehaviour
	{
		
		private GameState _gameState;
		public string[] playerNames;
		public Text[] scoreTextObjects;

		private void Awake()
		{
			_gameState = GetComponent<GameState>();
		}

		public void updateScore()
		{
			for (int i = 0; i < playerNames.Length; i++)
			{
				scoreTextObjects[i].text = _gameState.getScore(playerNames[i]).ToString();
			}
		}
	}
}

