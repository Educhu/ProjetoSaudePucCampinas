using Fusion;
using static Unity.Collections.Unicode;
using UnityEngine;

using System.Collections;
using UnityEngine.UI;


public class EnemySpawner : NetworkBehaviour
{
    public NetworkObject enemyPrefab;
    public NetworkObjectPoolDefault objectPool; // Refer�ncia � pool de objetos
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
        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(minX, maxX),
            UnityEngine.Random.Range(minY, maxY),
            0) + transform.position;

        NetworkObject enemyObject = objectPool.GetObjectFromPool(enemyPrefab);

        if (enemyObject != null)
        {
            enemyObject.transform.position = spawnPosition;
            enemyObject.transform.rotation = Quaternion.identity;

            Runner.Spawn(enemyObject, spawnPosition, Quaternion.identity, Object.InputAuthority);
            spawnInterval = UnityEngine.Random.Range(0.1f, 1f);
        }
        else
        {
            Debug.LogWarning("N�o foi poss�vel obter um objeto da pool.");
        }
    }

    public void ActivateSpawner()
    {
        gameObject.SetActive(true);
        Debug.Log("EnemySpawner ativado.");
        readyButton.SetActive(false); // Desativa o bot�o
    }
}