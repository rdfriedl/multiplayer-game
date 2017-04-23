using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        var hit = other.gameObject;
        var health = hit.GetComponent<PlayerHealth>();
        if (health)
        {
            health.TakeDamage(damage);
        }
    }
}
