using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Projectile : NetworkBehaviour
{
    private PlayerMovementFusion owner; // Referência ao jogador que disparou o projétil
    [SerializeField] private int pointsForKill = 50;  // Pontos por destruir um inimigo
    [SerializeField] private float lifeTime = 4.0f; // Tempo de vida do projétil em segundos

    private TickTimer lifeTimer; // Temporizador para controlar o tempo de vida do projétil

    public void SetOwner(PlayerMovementFusion player)
    {
        owner = player;
    }

    public override void Spawned()
    {
        // Inicia o temporizador de vida do projétil
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o projétil colidiu com um inimigo
        if (collision.CompareTag("Enemy"))
        {
            // Chama o método OnEnemyHit no jogador para adicionar pontos
            if (owner != null)
            {
                owner.OnEnemyHit(pointsForKill);
            }
            // Destrói o inimigo
            Runner.Despawn(collision.GetComponent<NetworkObject>());

            // Destrói o projétil
            Runner.Despawn(Object);
        }

        //atualiza os pontos na UI
        PlayerPoints playerPoints = FindObjectOfType<PlayerPoints>();
        playerPoints.UpdateScoreUI();
    }

    public override void FixedUpdateNetwork()
    {
        // Verifica se o tempo de vida do projétil expirou
        if (lifeTimer.Expired(Runner))
        {
            // Destrói o projétil automaticamente após o tempo de vida
            Runner.Despawn(Object);
        }
    }
}
