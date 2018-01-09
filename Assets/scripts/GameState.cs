using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
	[HideInInspector] public List<GameObject> shots;
	[HideInInspector] public GameObject[] platforms;
	[HideInInspector] public GameObject[] players;
	[HideInInspector] public ScoreKeeper scoreKeeper;

	
	private void Awake()
	{
		platforms = GameObject.FindGameObjectsWithTag("platform");
		shots = new List<GameObject>();
		players = GameObject.FindGameObjectsWithTag("player");
		scoreKeeper = GameObject.Find("scores").GetComponent<ScoreKeeper>();
		
		if (!scoreKeeper.scoresExist()) initializeScores();
	}

	private void initializeScores()
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
