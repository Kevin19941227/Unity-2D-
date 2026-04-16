using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public float interval = 1f;
    public float attackRange = 1.2f;
    public int damage = 15;

    public Transform attackPoint; // 在 Inspector 指定位置

    private bool hasDealtDamage = false;

    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    protected float flipY;
    protected Animator animator;

    public GameObject ChairAttack;

    public AudioClip swingSound;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        flipY = transform.localScale.y;

        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0f) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);

        // 更新冷卻時間
        if (timer > 0)
            timer -= Time.deltaTime;

        // 執行攻擊檢查
        Attack();
    }

    protected virtual void Attack()
    {
        direction = (mousePos - (Vector2)transform.position).normalized;
        transform.right = direction;

        if (Input.GetButtonDown("Fire1") && timer <= 0f)
        {
            timer = interval;
            MeleeAttack();
        }
    }

    protected virtual void MeleeAttack()
    {
        animator.SetTrigger("Shoot");
        hasDealtDamage = false;

        if (audioSource != null && swingSound != null)
            audioSource.PlayOneShot(swingSound);
    }


    // 給 Animation Event 呼叫
    //public void TriggerDamage()
    //{
    //    if (hasDealtDamage) return; // 避免重複傷害

    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

    //    foreach (Collider2D enemyCollider in hitEnemies)
    //    {
    //        Enemy enemy = enemyCollider.GetComponent<Enemy>();
    //        if (enemy != null)
    //        {
    //            enemy.TakeDamage(damage);
    //            Debug.Log("特定幀造成傷害：" + enemy.name);
    //        }
    //    }

    //    hasDealtDamage = true;
    //}

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void CreateEffect()
    {
        if (attackPoint == null || ChairAttack == null)
        {
            Debug.LogError("請指定 attackPoint 或 ChairAttack Prefab！");
            return;
        }

        // 生成並指定 parent 讓它跟著動（可選）
        var eff = Instantiate(
            ChairAttack,
            attackPoint.position,
            transform.rotation,
            this.transform
        );

        // 調整 localPosition 確保和 attackPoint 一致
        eff.transform.localPosition = attackPoint.localPosition;
    }
}
