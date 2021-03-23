using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Damageable damageable;
    [SerializeField] private Animator animator;
    [SerializeField] public GameObject player;
    [SerializeField] private Damager damager;
    [SerializeField] private CharacterController controller;

    [Header("Config")] 
    [SerializeField] private float attackingDistance = 1;
    [SerializeField] [Range(0, 1)] private float giveDamageOffset = .7f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private AnimationCurve hopAnimation = new AnimationCurve();

    private Damageable playerDamageable;
    private bool isAttacking;
    private bool isMoving;
    private bool isDead;
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int DieAnimation = Animator.StringToHash("Die");

    private void OnEnable()
    {
        damageable.onDie.AddListener(HandleDeath);
    }

    private void OnDisable()
    {
        damageable.onDie.RemoveListener(HandleDeath);
    }

    private void Start()
    {
        playerDamageable = player.GetComponent<Damageable>();
    }

    private void Update()
    {
        if (isDead)
        {
            if (isMoving)
            {
                StopMoving();
            }

            if (isAttacking)
            {
                StopAttacking();
            }
            
            return;
        }
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);

        if (distanceToPlayer <= attackingDistance)
        {
            if (!isAttacking)
            {
                StartAttacking();
            }

            if (isMoving)
            {
                StopMoving();
            }
        }
        else
        {
            if (isAttacking)
            {
                StopAttacking();
            }

            if (!isMoving)
            {
                StartMoving();
            }
        }
    }

    private void StartMoving()
    {
        isMoving = true;
        StartCoroutine(Move());
    }

    private void StopMoving()
    {
        isMoving = false;
        StopCoroutine(Move());
    }
    
    private IEnumerator Move()
    {
        FacePlayer();

        var ellapsedTime = 0f;
        while (ellapsedTime < 1f /* animation time */)
        {
            ellapsedTime += Time.deltaTime;
            var adjustment = hopAnimation.Evaluate(ellapsedTime) * Time.deltaTime;
            transform.position += moveSpeed * adjustment * transform.forward;
            yield return null;
            if (isDead)
            {
                yield break;
            }
        }
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        if (distanceToPlayer > attackingDistance)
        {
            StartCoroutine(Move());
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
    
    private IEnumerator Attack()
    {
        FacePlayer();
        yield return new WaitForSeconds(giveDamageOffset);
        
        if (isDead)
        {
            yield break;
        }
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        if (distanceToPlayer <= attackingDistance)
        {
            playerDamageable.TakeDamage(damager.damage);
            // Wait remainder of animation.
            yield return new WaitForSeconds(1 - giveDamageOffset);

            if (isDead)
            {
                yield break;
            }
            
            StartCoroutine(Attack());
        }
    }

    private void FacePlayer()
    {
        transform.LookAt(player.transform.position);
    }

    private void HandleDeath()
    {
        if (isDead)
        {
            return;
        }
        
        isDead = true;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        animator.SetBool(DieAnimation, true);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}