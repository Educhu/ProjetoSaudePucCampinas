using Fusion;
using static Unity.Collections.Unicode;
using UnityEngine;

using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemySpawner : NetworkBehaviour
{ // Lista de prefabs de inimigos
    public List<NetworkObject> enemyPrefabs;
    public List<NetworkObject> powerUpPrefabs;
    public float spawnInterval; // Intervalo de tempo entre cada spawn
    public float spawnIntervalPowerEps;
    public float minX = -12f; // Limite m�nimo no eixo X para a posi��o do spawn
    public float maxX = 12f; // Limite m�ximo no eixo X para a posi��o do spawn
    public float minY = 20f; // Limite m�nimo no eixo Y para a posi��o do spawn
    public float maxY = 25f; // Limite m�ximo no eixo Y para a posi��o do spawn
    public GameObject boss; // Refer�ncia ao GameObject do Boss
    public float BossAparece;
    private float timer; // Contador de tempo para controlar os spawns
    private float timerPowerUps;
    public GameObject readyButton;
    private bool bossActivated = false;

    public void Start()
    {
        spawnIntervalPowerEps = Random.Range(5f, 15f);
        StartCoroutine(ActivateBossAfterDelay(BossAparece));
      
    }

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
        // Apenas o servidor controla o tempo de spawn
        if (Object.HasStateAuthority && gameObject.activeInHierarchy)
        {
            timerPowerUps += Time.deltaTime;

            if (timerPowerUps >= spawnIntervalPowerEps)
            {
                SpawnPowerUp();
                timerPowerUps = 0f; // Reseta o contador de tempo
                spawnIntervalPowerEps = Random.Range(10f, 15f);
            }
        }
    }
    private void SpawnPowerUp()
    {
        if (powerUpPrefabs != null && powerUpPrefabs.Count > 0)
        {
            // Gera uma posi��o aleat�ria dentro dos limites definidos
            Vector3 spawnPosition = new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY),
                0
            );

            // Ajusta a posi��o com a posi��o do spawner
            spawnPosition += transform.position;

            // Escolhe um Power-up aleat�rio da lista de prefabs
            int randomIndex = UnityEngine.Random.Range(0, powerUpPrefabs.Count);
            NetworkObject chosenPowerUpPrefab = powerUpPrefabs[randomIndex];

            // Spawna o Power-up usando o `Runner`
            NetworkObject powerUpObject = Runner.Spawn(chosenPowerUpPrefab, spawnPosition, Quaternion.identity, Object.InputAuthority);

            // Varia o intervalo entre spawns se necess�rio
            spawnInterval = UnityEngine.Random.Range(2f, 5f); // Intervalo vari�vel entre 2 e 5 segundos
        }
        else
        {
            Debug.LogWarning("Nenhum prefab de Power-up foi atribu�do na lista.");
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs != null && enemyPrefabs.Count > 0)
        {
            // Gera uma posi��o aleat�ria dentro dos limites definidos
            Vector3 spawnPosition = new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY),
                0
            );

            // Ajusta a posi��o com a posi��o do spawner
            spawnPosition += transform.position;

            // Escolhe um inimigo aleat�rio da lista de prefabs
            int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);
            NetworkObject chosenEnemyPrefab = enemyPrefabs[randomIndex];

            // Spawna o inimigo usando o `Runner`
            NetworkObject enemyObject = Runner.Spawn(chosenEnemyPrefab, spawnPosition, Quaternion.identity, Object.InputAuthority);

            // Varia o intervalo entre spawns
            spawnInterval = UnityEngine.Random.Range(0.1f, 1f);
        }
        else
        {
            Debug.LogWarning("Nenhum prefab de inimigo foi atribu�do na lista.");
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

    private IEnumerator ActivateBossAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (boss != null && !bossActivated)
        {
            boss.SetActive(true);
            bossActivated = true; // Para garantir que o boss s� seja ativado uma vez
            Debug.Log("Boss ativado!");
        }
        else
        {
            Debug.LogWarning("O Boss n�o foi atribu�do ou j� foi ativado.");
        }
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