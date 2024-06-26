using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
	public Brick BrickPrefab;
	public int LineCount = 6;
	public Rigidbody Ball;

	public Text ScoreText;
	public Text BestScoreText; // Added BestScoreText reference
	public GameObject GameOverText;

	private bool m_Started = false;
	private int m_Points;
	private bool m_GameOver = false;

	private string playerName; // New variable to store player name
	private int bestScore; // New variable to store best score

	void Start()
	{
		LoadGameData(); // Load player name and best score from PlayerPrefs

		const float step = 0.6f;
		int perLine = Mathf.FloorToInt(4.0f / step);

		int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
		for (int i = 0; i < LineCount; ++i)
		{
			for (int x = 0; x < perLine; ++x)
			{
				Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
				var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
				brick.PointValue = pointCountArray[i];
				brick.onDestroyed.AddListener(AddPoint);
			}
		}
	}

	private void Update()
	{
		if (!m_Started)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				m_Started = true;
				float randomDirection = Random.Range(-1.0f, 1.0f);
				Vector3 forceDir = new Vector3(randomDirection, 1, 0);
				forceDir.Normalize();

				Ball.transform.SetParent(null);
				Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
			}
		}
		else if (m_GameOver)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}

	void AddPoint(int point)
	{
		m_Points += point;
		ScoreText.text = $"Score : {m_Points}";
	}

	public void GameOver()
	{
		m_GameOver = true;

		// Check if the current score is higher than the best score
		if (m_Points > bestScore)
		{
			bestScore = m_Points;
		}

		// Update the BestScoreText with the player's name and best score
		BestScoreText.text = $"Player: {playerName} - Best Score: {bestScore}";

		// Save best score to PlayerPrefs
		PlayerPrefs.SetInt("BestScore", bestScore);
		PlayerPrefs.Save();
	}

	void LoadGameData()
	{
		playerName = PlayerPrefs.GetString("PlayerName", "Player");
		bestScore = PlayerPrefs.GetInt("BestScore", 0);

		BestScoreText.text = $"Player: {playerName} - Best Score: {bestScore}"; // Update BestScoreText
	}
}
