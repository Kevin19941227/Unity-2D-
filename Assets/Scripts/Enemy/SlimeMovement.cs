using System.Collections;
using System.Collections.Generic;
using UnityEngine;
        public class SlimeMovement : MonoBehaviour
        {
        public float moveSpeed = 2f;
        public float stoppingDistance = 1.5f;
        private Transform player;
        private Rigidbody2D rb;
        private Animator animator;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (player == null)
                return;

            Vector2 direction = (player.position - transform.position).normalized;
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance > stoppingDistance)
            {
                rb.velocity = direction * moveSpeed;
                animator.SetBool("isMoving", true);
            }
            else
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isMoving", false);
            }

            // ­±´Âª±®a
            if (player.position.x > transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

