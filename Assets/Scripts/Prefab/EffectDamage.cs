using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EffectDamage : MonoBehaviour
{
    public int damage = 15;
    public float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 確認是 Enemy
        var e = other.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
            Debug.Log($"Hit {other.name} for {damage}");
            // 如果要第一碰就消失： Destroy(gameObject);
        }
    }
}
