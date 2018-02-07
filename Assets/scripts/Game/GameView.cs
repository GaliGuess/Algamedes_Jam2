using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game{
	public class GameView : MonoBehaviour
	{
		
		private GameState _gameState;
		public string[] playerNames;
		public GameObject[] lives_objects;

		private void Awake()
		{
			_gameState = GetComponent<GameState>();
		}

		public void updateScore()
		{
			for (int i = 0; i < playerNames.Length; i++)
			{
				if (lives_objects[i] != null)
				{
					int currentScore = _gameState.getScore(playerNames[i]);
					LivesVisualizer livesVisualizer = lives_objects[i].GetComponent<LivesVisualizer>();

					if (livesVisualizer.getCurrentLives() != currentScore)
					{
						livesVisualizer.setLives(currentScore);
					}
				}
			}
		}
	}
}

