using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
	[HideInInspector] public List<GameObject> shots;
	[HideInInspector] public GameObject[] platforms;
	[HideInInspector] public GameObject[] players;
	public ScoreKeeper scoreKeeper;
	
	private GameView _gameView;
	
	private void Awake()
	{
		platforms = GameObject.FindGameObjectsWithTag("platform");
		shots = new List<GameObject>();
		players = GameObject.FindGameObjectsWithTag("player");
		
		_gameView = GetComponent<GameView>();
	}

	private void Start()
	{
		scoreKeeper = GameObject.Find("scores").GetComponent<ScoreKeeper>();
		if (!scoreKeeper.scoresExist()) initializeScores();
		_gameView.updateScore();
	}

	public void initializeScores()
	{
		foreach (var player in players)
		{	
			scoreKeeper.setScore(player.name, 0);
		}
	}
	
	public void incrementScore(string playerName)
	{
		scoreKeeper.incrementScore(playerName);
	}

	public int getScore(string playerName)
	{
		return scoreKeeper.getScore(playerName);
	}
}
