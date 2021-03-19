using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private Damageable damageable;

    private void Awake()
    {
        damageable.onDie.AddListener(() => Destroy(gameObject));
    }
}