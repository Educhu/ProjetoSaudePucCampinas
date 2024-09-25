using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : NetworkBehaviour
{
    public float moveSpeed = 2f; // Velocidade do boss
    public Transform middlePoint; // Ponto de destino (meio da tela)
    public Animator animator; // Refer�ncia ao Animator
    public float shootInterval = 3f; // Intervalo entre os disparos

    private Vector3 targetPosition;
    private bool isAtMiddle = false;
    private float shootTimer = 0f;

    // M�todo chamado quando o objeto de rede � spawnado
    public void Start()
    {
        targetPosition = middlePoint.position; // Definir o destino para o meio da tela
    }

    void Update()
    {
       // if (!Object.HasStateAuthority) return; // Garantir que somente o dono do objeto controla a l�gica

        if (!isAtMiddle)
        {
            MoveToMiddle();
        }
        else
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0f; // Reseta o timer
            }
        }
    }

    private void MoveToMiddle()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            isAtMiddle = true;
            animator.SetBool("IsShooting", true); // Ativa a anima��o de atirar
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("Shoot"); // Aciona a anima��o de disparo
    }

    // Este m�todo ser� chamado atrav�s do Animation Event 'BossShoot'
    public void BossShoot()
    {
        // L�gica para instanciar o proj�til (implementado futuramente)
        Debug.Log("Boss disparou!");
    }
}

