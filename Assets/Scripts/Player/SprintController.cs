using BulletHell;
using UnityEngine;
using System.Collections;

public class SprintController : MonoBehaviour
{
    public PlayerMovement player; // 在 Inspector 中指定
    public TrailRenderer sprintShadow;

    private float sprintTimer;
    private bool isSprinting;
    private bool isInCooldown;

    private int originalLayer;
    public bool IsSprinting => isSprinting;

    [Header("參數")]
    public float sprintSpeed = 20f;
    public float sprintDuration = 0.5f;
    public float sprintCooldown = 3.0f; // 冷卻時間（秒）
    private float cooldownTimer = 0f;

    void Start()
    {
        originalLayer = gameObject.layer;
    }

    void Update()
    {
        UpdateCooldown();
        HandleSprint();
    }

    void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isInCooldown = false;
            }
        }
    }

    void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting && !isInCooldown)
        {
            // 開始衝刺
            isSprinting = true;
            sprintTimer = sprintDuration;
            cooldownTimer = sprintCooldown;
            isInCooldown = true;

            if (player != null)
            {
                player.isInvincible = true;
                player.gameObject.layer = LayerMask.NameToLayer("PlayerNoCollideEnemy"); // 換圖層
            }

            if (sprintShadow != null)
                sprintShadow.emitting = true;
        }

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0f)
            {
                isSprinting = false;

                if (sprintShadow != null)
                    sprintShadow.emitting = false;

                if (player != null)
                    StartCoroutine(EndSprintEffects());
            }
        }
    }

    IEnumerator EndSprintEffects()
    {
        yield return new WaitForSeconds(0.7f); // 延遲還原圖層
        if (player != null)
        {
            player.isInvincible = false;
            player.gameObject.layer = originalLayer;
        }
    }
}
