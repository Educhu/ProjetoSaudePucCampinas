using Fusion;
using static Unity.Collections.Unicode;
using UnityEngine;

using System.Collections;
using UnityEngine.UI;

public class EnemySpawner : NetworkBehaviour
{
    public NetworkObject enemyPrefab;
    public float spawnInterval; // Intervalo de tempo entre cada spawn
    public float minX = -12f; // Limite m�nimo no eixo X para a posi��o do spawn
    public float maxX = 12f; // Limite m�ximo no eixo X para a posi��o do spawn
    public float minY = 20f; // Limite m�nimo no eixo Y para a posi��o do spawn
    public float maxY = 25f; // Limite m�ximo no eixo Y para a posi��o do spawn

    private float timer; // Contador de tempo para controlar os spawns
    public GameObject readyButton;



    private void Update()
    {
        if (Object.HasStateAuthority && gameObject.activeInHierarchy)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f; // Reseta o contador de tempo
            }
        }
    }

    private void SpawnEnemy()
    {
        // Gera uma posi��o aleat�ria dentro dos limites definidos
        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(minX, maxX),
            UnityEngine.Random.Range(minY, maxY),
            0
        );

        // Ajusta a posi��o com a posi��o do spawner
        spawnPosition += transform.position;

        // Spawna o inimigo usando o `Runner`
        if (enemyPrefab != null)
        {
            NetworkObject enemyObject = Runner.Spawn(enemyPrefab, spawnPosition, Quaternion.identity, Object.InputAuthority);
            spawnInterval = UnityEngine.Random.Range(0.1f, 1f); // Varia o intervalo entre spawns
        }
        else
        {
            Debug.LogWarning("O prefab do inimigo n�o est� definido.");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcDestroyEnemy(NetworkObject enemy)
    {
        if (enemy != null)
        {
            Runner.Despawn(enemy); // Despawns o inimigo
            Debug.Log($"Inimigo destru�do: {enemy}");
        }
    }

    public void ActivateSpawner()
    {
        gameObject.SetActive(true);
        Debug.Log("EnemySpawner ativado.");
        readyButton.SetActive(false); // Desativa o bot�o
    }

    // M�todo para desenhar gizmos no editor
    private void OnDrawGizmos()
    {
        // Definindo a cor dos gizmos
        Gizmos.color = Color.red;

        // Desenhando uma caixa que representa a �rea de spawn
        Vector3 center = transform.position + new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
        Gizmos.DrawWireCube(center, size);
    }
}