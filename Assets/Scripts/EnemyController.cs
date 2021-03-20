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
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private void Awake()
    {
        damageable.onDie.AddListener(() => Destroy(gameObject));
    }

    private void Start()
    {
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
        
        var distanceToPlayer = Vector3.Magnitude(transform.position - player.transform.position);
        if (distanceToPlayer <= attackingDistance)
        {
            playerDamageable.TakeDamage(damager.damage);
            // Wait remainder of animation.
            yield return new WaitForSeconds(1 - giveDamageOffset); 
            StartCoroutine(Attack());
        }
    }

    private void FacePlayer()
    {
        transform.LookAt(player.transform.position);
    }
}