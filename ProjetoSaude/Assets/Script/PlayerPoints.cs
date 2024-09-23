using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPoints : MonoBehaviour
{
    public TextMeshProUGUI PlayerPointText;
    public PlayerMovementFusion player;
    private int pointsToPowerUp = 300;
    private int pointsToBoss = 500;
    private int lastPowerUp = 0;
    private int lastBoss = 0;

    private void Start()
    {
        PlayerPointText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
        UpdateScoreUI();
    }

    private void Update()
    {
        UpdateScoreUI();   
    }

    public void UpdateScoreUI()
    {
        player = FindObjectOfType<PlayerMovementFusion>();
        PlayerPointText.text = $"Pontua��o: {player.GetScore()}";

        int currentScore = player.GetScore();
        CheckForMilestones(currentScore);
    }

    private void CheckForMilestones(int _score)
    {
        // Verifica se a pontua��o � m�ltipla de 300 e se j� n�o foi logada anteriormente
        if (_score >= pointsToPowerUp && _score / pointsToPowerUp > lastPowerUp)
        {
            Debug.Log("Spawna PowerUp");
            lastPowerUp = _score / pointsToPowerUp; // Atualiza o controle do �ltimo log
        }

        // Verifica se a pontua��o � m�ltipla de 500 e se j� n�o foi logada anteriormente
        if (_score >= pointsToBoss && _score / pointsToBoss > lastBoss)
        {
            Debug.Log("Spawna Boss");
            lastBoss = _score / 500; // Atualiza o controle do �ltimo log
        }
    }
}
