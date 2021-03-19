using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDie;
    
    [Header("Config")] 
    [SerializeField] private float health = 1;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            onDie.Invoke();
        }
    }
}