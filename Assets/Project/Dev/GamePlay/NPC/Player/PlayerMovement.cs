using UnityEngine;
using Spine.Unity;
using Spine;
using AnimationState = Spine.AnimationState;

namespace Project.Dev.GamePlay.NPC.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f; // скорость перемещения
        private Rigidbody2D rb;
        private SkeletonAnimation skeletonAnimation;
        private AnimationState spineAnimationState;

        [SpineAnimation] public string idleAnimation = "idle";
        [SpineAnimation] public string runAnimation = "run";

        private bool isMoving = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            if (skeletonAnimation != null)
            {
                spineAnimationState = skeletonAnimation.AnimationState;
            }
            else
            {
                Debug.LogError("SkeletonAnimation компонент не найден!");
            }
        }

        void Update()
        {
            HandleMovement();
            UpdateAnimation();
        }

        private void HandleMovement()
        {
            float moveInput = Input.GetAxisRaw("Horizontal"); // -1, 0 или 1

            if (moveInput != 0)
            {
                Vector2 moveDirection = new Vector2(moveInput, 0);
                rb.velocity = moveDirection * moveSpeed;
                isMoving = true;

                // Поворот героя в сторону движения
                if (moveInput > 0)
                    transform.localScale = new Vector3(1, 1, 1);
                else if (moveInput < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                rb.velocity = Vector2.zero;
                isMoving = false;
            }
        }

        private void UpdateAnimation()
        {
            if (spineAnimationState == null) return;

            var currentAnim = spineAnimationState.GetCurrent(0);
            if (isMoving)
            {
                if (currentAnim == null || currentAnim.Animation.Name != runAnimation)
                {
                    spineAnimationState.SetAnimation(0, runAnimation, true);
                }
            }
            else
            {
                if (currentAnim == null || currentAnim.Animation.Name != idleAnimation)
                {
                    spineAnimationState.SetAnimation(0, idleAnimation, true);
                }
            }
        }
    }
}
