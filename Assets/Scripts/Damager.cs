using UnityEngine;

public class Damager : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] public float damage;

    private void OnCollisionEnter(Collision other)
    {
        HandleCollision(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject other)
    {
        var damageable = other.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}