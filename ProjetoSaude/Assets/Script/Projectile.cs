using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Projectile : NetworkBehaviour
{
    private PlayerMovementFusion owner; // Refer�ncia ao jogador que disparou o proj�til
    [SerializeField] private int pointsForKill = 50;  // Pontos por destruir um inimigo
    [SerializeField] private float lifeTime = 4.0f; // Tempo de vida do proj�til em segundos

    private TickTimer lifeTimer; // Temporizador para controlar o tempo de vida do proj�til

    public void SetOwner(PlayerMovementFusion player)
    {
        owner = player;
    }

    public override void Spawned()
    {
        // Inicia o temporizador de vida do proj�til
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o proj�til colidiu com um inimigo
        if (collision.CompareTag("Enemy"))
        {
            // Chama o m�todo OnEnemyHit no jogador para adicionar pontos
            if (owner != null)
            {
                owner.OnEnemyHit(pointsForKill);
            }
            // Destr�i o inimigo
            Runner.Despawn(collision.GetComponent<NetworkObject>());

            // Destr�i o proj�til
            Runner.Despawn(Object);
        }

        //atualiza os pontos na UI
        PlayerPoints playerPoints = FindObjectOfType<PlayerPoints>();
        playerPoints.UpdateScoreUI();
    }

    public override void FixedUpdateNetwork()
    {
        // Verifica se o tempo de vida do proj�til expirou
        if (lifeTimer.Expired(Runner))
        {
            // Destr�i o proj�til automaticamente ap�s o tempo de vida
            Runner.Despawn(Object);
        }
    }
}
