using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Damageable damageable;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;
    [SerializeField] private Damager damager;

    [Header("Config")] 
    [SerializeField] private float attackingDistance = 1;
    [SerializeField] private float giveDamageOffset = .7f;

    private Damageable playerDamageable;
    private bool isAttacking;
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private void Awake()
    {
        damageable.onDie.AddListener(() => Destroy(gameObject));
        playerDamageable = player.GetComponent<Damageable>();
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);

        if (distanceToPlayer <= attackingDistance)
        {
            if (!isAttacking)
            {
                StartAttacking();
            }
        }
        else
        {
            StopAttacking();
        }
    }

    private IEnumerator Attack()
    {
        transform.LookAt(player.transform.position);
        yield return new WaitForSeconds(giveDamageOffset);
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        if (distanceToPlayer <= attackingDistance)
        {
            playerDamageable.TakeDamage(damager.damage);
            StartCoroutine(Attack());
        }
    }

    private void StartAttacking()
    {
        isAttacking = true;
        animator.SetBool(Attacking, true);
        StartCoroutine(Attack());
    }
    
    private void StopAttacking()
    {
        isAttacking = false;
        animator.SetBool(Attacking, false);
    }
}