using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class animCharacters : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool hasAttackedThisTurn = false;

    public bool isTurnPlayer1;
    public bool isTurnPlayer2;

    void Start()
    {
        agent = gameObject.GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Layer ayarları
        gameObject.layer = LayerMask.NameToLayer("Monster");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Monster"), LayerMask.NameToLayer("Monster"));
    }

    void Update()
    {
        if (agent.speed > 0.90f)
        {
            animator.SetFloat("speed", agent.speed);
        }
        else if (agent.speed < 0.90f)
        {
            animator.SetFloat("speed", agent.speed);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("gate") && !(isTurnPlayer1||isTurnPlayer2))
        {
            HandleAttack(other);

        }
    }

    public void HandleAttack(Collider exit)
    {
        ExitLife exitLife = exit.GetComponent<ExitLife>();
        if (exitLife == null)
        {
            Debug.LogError("ExitLife component not found on the exit object!");
            return;
        }

        if (!hasAttackedThisTurn)
        {
            animator.SetBool("isAttack", true);
            agent.speed = 0;

            exitLife.exitHealth -= 1;
            Debug.Log(exitLife.exitHealth);

            if (exitLife.exitHealth <= 0)
            {
                Destroy(exit.gameObject);
            }

            hasAttackedThisTurn = true;
            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1); // Bir sonraki turu bekle
        isTurnPlayer1=true;
        hasAttackedThisTurn = false;
        animator.SetBool("isAttack", false);
        animator.SetFloat("speed", 0); // idle animasyonuna geçiş için hız değerini sıfırla
        agent.speed = 1;
    }

    public void ResetAttackStatus()
    {
        hasAttackedThisTurn = false;
        animator.SetBool("isAttack", false);
        animator.SetFloat("speed", 0); // idle animasyonuna geçiş için hız değerini sıfırla
        agent.speed = 1;
    }
}
