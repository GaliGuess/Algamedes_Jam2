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
		private LivesVisualizer[] _livesVisualizers;

		private void Awake()
		{
			_gameState = GetComponent<GameState>();
			
			initializeScores();
		}

		public void decreaseScore(string killedPlayer)
		{
			for (int i = 0; i < playerNames.Length; i++)
			{
				if (killedPlayer == playerNames[i])
				{
					_livesVisualizers[i].decreaseLife();
				}
			}
		}

		public void initializeScores() {
			_livesVisualizers = new LivesVisualizer[lives_objects.Length];
			for (int i = 0; i < lives_objects.Length; i++)
			{
				_livesVisualizers[i] = lives_objects[i].GetComponent<LivesVisualizer>();
			}
		}
		
		public void updateScore()
		{
			for (int i = 0; i < playerNames.Length; i++)
			{
				if (lives_objects[i] != null)
				{
					int currentScore = _gameState.getScore(playerNames[i]);
//					LivesVisualizer livesVisualizer = lives_objects[i].GetComponent<LivesVisualizer>();

					if (_livesVisualizers[i].getCurrentLives() != currentScore)
					{
						_livesVisualizers[i].setLives(currentScore);
					}
				}
			}
		}
	}
}

