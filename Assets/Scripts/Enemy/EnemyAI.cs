using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private Transform playerTarget;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;

    [Header("Zombie Attack Stats")]
    public float attackDistance = 2.5f;
    public float attackDamage = 20f;
    public float attackRate = 1.5f;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                Debug.LogError("The Zombie found the Player, but the Player is missing the 'PlayerHealth' script!");
            }
        }
        else
        {
            Debug.LogError("No object with the 'Player' tag was found!");
        }
    }

    void Update()
    {
        if (playerTarget != null && playerHealth != null && playerHealth.currentHealth > 0)
        {
            agent.SetDestination(playerTarget.position);

            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer <= attackDistance)
            {
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            playerHealth.TakeDamage(attackDamage);
        }
    }
}