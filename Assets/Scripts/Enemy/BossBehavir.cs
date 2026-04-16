using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : Enemy
{
    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public Transform[] patrolPoints;
    private int patrolIndex;
    private Vector2 currentDirection;

    [Header("Chase Settings")]
    public float locationUpdateInterval = 10f;
    public float chaseRange = 5f;
    public float chaseDuration = 5f;
    private float locationUpdateTimer;
    private float chaseTimer;
    private bool isChasing;

    [Header("Attack Settings")]
    public float defaultAttackCooldown = 3f;
    public float lowHPAttackCooldown = 1f;
    private float attackCooldownTimer;
    public int rapidAttackCount = 3;
    public float rapidAttackInterval = 0.2f;
    private bool isAttacking;

    [Header("Health & Hurt")]
    public int maxHealth = 200;
    private int currentHealth;
    private bool isHurt;

    [Header("Low HP Flash")]
    public float lowHPThreshold = 0.5f;
    public float lowHPFlashInterval = 0.2f;
    private bool isFlashing;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;

    [Header("Attack Prefabs")]
    public GameObject DogAttackanim1;
    public GameObject DogAttackanim2;
    public GameObject DogAttackanim3;

    // Patrol pause
    private bool patrolPaused;
    private float pauseTimer;
    private float pauseDuration;

    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player")?.transform;

        audioSource = GetComponent<AudioSource>(); //  加上這行
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        // Stop movement when hurt
        if (isHurt) rb.velocity = Vector2.zero;

        // Low HP flash
        bool lowNow = currentHealth <= maxHealth * lowHPThreshold;
        if (lowNow && !isFlashing)
        {
            isFlashing = true;
            StartCoroutine(LowHPFlash());
        }
        else if (!lowNow && isFlashing)
        {
            isFlashing = false;
            StopCoroutine(LowHPFlash());
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Attack cooldown
        if (attackCooldownTimer > 0f) attackCooldownTimer -= Time.deltaTime;

        // Chase trigger
        locationUpdateTimer += Time.deltaTime;
        if ((!isChasing && player && Vector2.Distance(transform.position, player.position) <= chaseRange)
            || locationUpdateTimer >= locationUpdateInterval)
        {
            isChasing = true;
            chaseTimer = 0f;
            locationUpdateTimer = 0f;
        }

        if (isChasing)
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer <= chaseDuration && player)
            {
                currentDirection = (player.position - transform.position).normalized;
                TryAttack();
            }
            else
            {
                isChasing = false;
                currentDirection = Vector2.zero;
            }
        }
        else
        {
            PatrolUpdate();
        }

        // Apply movement
        if (!isHurt)
            rb.velocity = currentDirection * moveSpeed;

        anim.SetBool("Walked", currentDirection != Vector2.zero);
        if (currentDirection.x > 0) transform.localScale = new Vector3(5,5, 1);
        else if (currentDirection.x < 0) transform.localScale = new Vector3(-5, 5, 1);
    }

    private IEnumerator LowHPFlash()
    {
        var sr = GetComponent<SpriteRenderer>();
        while (isFlashing)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(lowHPFlashInterval);
            sr.color = Color.white;
            yield return new WaitForSeconds(lowHPFlashInterval);
        }
        sr.color = Color.white;
    }

    private void PatrolUpdate()
    {
        if (patrolPoints.Length == 0)
        {
            currentDirection = Vector2.zero;
            return;
        }

        // Pause at point
        if (patrolPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration) patrolPaused = false;
            currentDirection = Vector2.zero;
            return;
        }

        // Move towards point
        Vector2 target = patrolPoints[patrolIndex].position;
        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            // Start pause
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            patrolPaused = true;
            pauseDuration = Random.Range(1f, 3f);
            pauseTimer = 0f;
            currentDirection = Vector2.zero;
        }
        else
        {
            currentDirection = (target - (Vector2)transform.position).normalized;
        }
    }

    private void TryAttack()
    {



        if (isAttacking || attackCooldownTimer > 0f) return;
        isAttacking = true;
        anim.SetTrigger("Attack");
        attackCooldownTimer = (currentHealth <= maxHealth * lowHPThreshold)
            ? lowHPAttackCooldown : defaultAttackCooldown;
    }

    public void AttackOver()
    {
        isAttacking = false;
        StartCoroutine(RapidAttackSequence());
    }

    private IEnumerator RapidAttackSequence()
    {
        float dir = transform.localScale.x;
        for (int i = 0; i < rapidAttackCount; i++)
        {
            Vector3 spawn = transform.position + Vector3.right;
            AttackSelect(spawn);

            //  僅第一段攻擊播放音效，避免連播太吵
            if (i == 0 && attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }

            yield return new WaitForSeconds(rapidAttackInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet")) return;
        int before = currentHealth;
        currentHealth -= 10;
        isHurt = true;
        if (before > maxHealth * lowHPThreshold)
            anim.SetTrigger("Hurted");
        else
            HurtOver();
        if (currentHealth <= 0) Die();
    }

    public void HurtOver()
    {
        isHurt = false;
        isFlashing = (currentHealth <= maxHealth * lowHPThreshold);
        anim.ResetTrigger("Hurted");
    }

    public override void TakeDamage(int damage)
    {

        if (hurtSound != null && audioSource != null)
            audioSource.PlayOneShot(hurtSound);

        if (isHurt) return;
        isHurt = true;
        currentHealth -= damage;
        anim.SetTrigger("Hurted");
        if (currentHealth <= 0) Die();
    }

    private void AttackSelect(Vector3 pos)
    {
        int a = Random.Range(1, 4);
        GameObject prefab = (a == 1) ? DogAttackanim1 : (a == 2) ? DogAttackanim2 : DogAttackanim3;
        var atk = Instantiate(prefab, pos, Quaternion.identity);
        float dirSign = transform.localScale.x;
        var sr = atk.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = dirSign < 0f;
        else
        {
            var s = atk.transform.localScale;
            s.x = Mathf.Abs(s.x) * dirSign;
            atk.transform.localScale = s;
        }
        // Prefab animation plays automatically on instantiation; no need to manually trigger it
    }

    private void Die()
    {
        if (deathSound != null)
        {
            GameObject soundObj = new GameObject("BossDeathSound");
            AudioSource tempSource = soundObj.AddComponent<AudioSource>();
            tempSource.clip = deathSound;
            tempSource.Play();
            Destroy(soundObj, deathSound.length);
        }

        Destroy(gameObject); // 不延遲銷毀本體

        // 呼叫檢查勝利條件
    FindObjectOfType<GameManager>().CheckVictory();
    }
}
