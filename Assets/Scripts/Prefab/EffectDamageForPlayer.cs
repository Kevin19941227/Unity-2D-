using BulletHell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamageForPlayer : MonoBehaviour
{
    public int damage = 15;
    public float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Effect collided with {other.name} (layer: {other.gameObject.layer})");

        // 確認是 Enemy
        var e = other.GetComponent<PlayerMovement>();
        if (e != null)
        {
            Debug.Log("  → Found PlayerMovement component!");
            e.TakeDamage(damage);
            Debug.Log($"Hit {other.name} for {damage}");
            // 如果要第一碰就消失： Destroy(gameObject);
        }
    }
}
