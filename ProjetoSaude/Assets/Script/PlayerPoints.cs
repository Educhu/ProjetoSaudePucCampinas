using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PlayerPoints : MonoBehaviour
{
    public TextMeshProUGUI PlayerPointText;
    private PlayerMovementFusion player;
    private int playerPoints; // Vari�vel para armazenar a pontua��o do jogador

    private void Start()
    {
        PlayerPointText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
        playerPoints = 0; // Inicializa a pontua��o do jogador
        UpdatePointsText(); // Atualiza o texto no in�cio
    }

    // M�todo para adicionar pontos
    public void AddPoints(int points)
    {
        playerPoints += points; // Adiciona os pontos
        UpdatePointsText(); // Atualiza o texto
    }

    // M�todo para atualizar o texto
    private void UpdatePointsText()
    {
        PlayerPointText.text = "Pontos: " + playerPoints; // Atualiza o texto com a pontua��o
    }

    // (Opcional) Se voc� quiser definir o jogador para receber pontos
    public void SetPlayer(PlayerMovementFusion newPlayer)
    {
        player = newPlayer;
    }
}