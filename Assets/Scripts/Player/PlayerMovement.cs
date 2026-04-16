using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public GameObject[] guns;
        public float speed;
        private Vector2 input;
        private Vector2 mousePos;
        private Rigidbody2D rigidBody;
        public float sprintSpeed = 20f;
        public float sprintDuration = 0.5f;
        public float sprintTimer = 0f;
        private bool isSprinting = false;
        public TrailRenderer sprintshadow;
        private Image hpPlayer;
        public bool isInvincible = false;

        [Header("HP")]
        public int maxHP = 100;
        private int currentHP;
        private bool isDead = false;

        [Header("Components")]
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private int gunNum;

        [Header("音效")]
 
        public AudioClip hurtSound;
        public AudioClip deathSound;

        private AudioSource audioSource;


        void Start()
        {
            sprintshadow.emitting = true; 
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            currentHP = maxHP;
            guns[0].SetActive(true);
            GameObject go = GameObject.Find("hpPlayer");
            if (go != null)
            {
                hpPlayer = go.GetComponent<Image>();
            }
            else
            {
                Debug.LogWarning("PlayerMovement 無法找到名為 'GlobalHPImage' 的物件，請確認它已在場景中並且已用 DontDestroyOnLoad 保持常駐。");
            }

            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {

            if (isDead) return;
            sprint();
            SwitchGun();

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            float currentSpeed = isSprinting ? sprintSpeed : speed;
            rigidBody.velocity = input.normalized  * currentSpeed;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 角色朝向
            if (mousePos.x > transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);

            // 動畫控制（可選）
            if (animator != null)
                animator.SetBool("isMoving", input != Vector2.zero);
        }

        void SwitchGun()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                guns[gunNum].SetActive(false);
                gunNum = (gunNum - 1 + guns.Length) % guns.Length;
                guns[gunNum].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                guns[gunNum].SetActive(false);
                gunNum = (gunNum + 1) % guns.Length;
                guns[gunNum].SetActive(true);
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            currentHP -= damage;
            hpPlayer.fillAmount = (float)currentHP / maxHP;
            Debug.Log("Player takes " + damage + " damage. Current HP: " + currentHP);

            if (hurtSound != null)
            {
                audioSource.PlayOneShot(hurtSound);

                Debug.Log("播放受傷音效！");
            }

            StartCoroutine(DamageFlash());

            if (currentHP <= 0)
            {
                Die();
            }
        }

        IEnumerator DamageFlash() //閃爍紅色的 Coroutine：
        {
            int flashCount = 3;
            float flashDuration = 0.1f;

            for (int i = 0; i < flashCount; i++)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(flashDuration);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(flashDuration);
            }
        }

        void sprint()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting)
            {
                isSprinting = true;
                sprintTimer = sprintDuration;

                if (sprintshadow != null)
                    sprintshadow.emitting = true; // 播放一次殘影
            }

            if (isSprinting)
            {
                sprintTimer -= Time.deltaTime;
                if (sprintTimer <= 0f)
                {
                    isSprinting = false;
                    // 不用 Stop()
                    if (sprintshadow != null)
                        sprintshadow.emitting = false; // 關閉拖尾
                }
            }
        }



        void Die()
        {
            isDead = true;
            rigidBody.velocity = Vector2.zero;
            Debug.Log("Player is dead!");
            hpPlayer.fillAmount = maxHP;

            if (deathSound != null && audioSource != null)
                audioSource.PlayOneShot(deathSound);


            // 顯示失敗畫面
            FindObjectOfType<EndGameManager>().ShowDefeat();


            Destroy(gameObject, 0.5f); // 延遲刪除，讓音效播完

        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // 每次碰到敵人扣「最大生命的 20%」
                int damage = Mathf.CeilToInt(maxHP * 0.1f
                    );
                TakeDamage(damage);
            }
        }
    }
}
