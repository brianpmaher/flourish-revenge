using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float health = 1;

    [Header("Events")]
    public UnityEvent onDie;
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            onDie.Invoke();
        }
    }
}