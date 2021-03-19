using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Damageable damageable;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    [Header("Config")] 
    [SerializeField] private float attackingDistance = 1;
    
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private void Awake()
    {
        damageable.onDie.AddListener(() => Destroy(gameObject));
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        
        if (distanceToPlayer <= attackingDistance)
        {
            StartAttacking();
        }
        else
        {
            StopAttacking();
        }
    }

    private void StartAttacking()
    {
        animator.SetBool(Attacking, true);
        transform.LookAt(player.transform.position);
    }

    private void StopAttacking()
    {
        animator.SetBool(Attacking, false);
    }
}