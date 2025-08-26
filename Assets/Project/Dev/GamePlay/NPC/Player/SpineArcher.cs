using Spine;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Project.Dev.GamePlay.NPC.Player
{
    public class SpineArcher : MonoBehaviour
    {
        [SpineAnimation] public string idle;
        [SpineAnimation] public string attack_start;
        [SpineAnimation] public string attack_target;
        [SpineAnimation] public string attack_finish;

        private SkeletonAnimation skeletonAnimation;
        private AnimationState spineAnimationState;
        private Skeleton skeleton;

        private Bone gunBone; // Ссылка на кость "gun"
        private Bone bulletBone; // Ссылка на кость "bullet"
        private Bone string_c;

        [SerializeField] private GameObject arrowPrefab; // Префаб стрелы
        public float arrowSpeed; // Скорость полета стрелы
        [SerializeField] private Transform stringObject;
        public float tiltSpeed; // Скорость наклона
        public float maxTiltAngle; // Максимальный угол наклона

        private Vector3 lastMousePosition; // Последняя позиция мыши
        private bool isAttacking = false; // Флаг для отслеживания атаки
        private bool isFinishingAttack = false; // Флаг для отслеживания завершения атаки

        private float scaleFactor = 1f;

        private void Start()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;

            gunBone = skeleton.FindBone("gun");
            bulletBone = skeleton.FindBone("bullet");
            string_c = skeleton.FindBone("string_c");

            lastMousePosition = Input.mousePosition; // Инициализируем последнюю позицию мыши
        }

        private void Update()
        {
            Attack();

            lastMousePosition = Input.mousePosition;

            // Проверяем состояние атаки и завершаем атаку
            if (isFinishingAttack)
            {
                var currentAnimation = spineAnimationState.GetCurrent(0);
                if (currentAnimation != null && currentAnimation.Animation.Name == attack_finish &&
                    currentAnimation.IsComplete)
                {
                    isFinishingAttack = false;
                    PlayerIdleAnimation(); // Переход к idle после завершения атаки
                }
            }
            else if (!isAttacking && !IsPlayingIdle())
            {
                PlayerIdleAnimation();
            }
        }

        private void PlayStartAttackAnimation()
        {
            var currentAnimation = spineAnimationState.GetCurrent(0);

            if (currentAnimation == null || currentAnimation.Animation.Name != attack_start)
            {
                spineAnimationState.SetAnimation(0, attack_start, false);
                isAttacking = true;
            }
        }

        private void Attack()
        {
            if (isAttacking)
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

                // Изменяем угол наклона в зависимости от перемещения мыши
                if (gunBone != null)
                {
                    float newAngle = gunBone.Rotation + (-mouseDelta.y * tiltSpeed * Time.deltaTime);
                    newAngle = Mathf.Clamp(newAngle, -maxTiltAngle, maxTiltAngle); // Ограничиваем угол наклона
                    gunBone.Rotation = newAngle;

                    AdjustStringLength();
                    stringPositionAndIncrease();
                }
            }
        }

        private void AdjustStringLength()
        {
            // Получаем текущую позицию мыши в мировых координатах
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.nearClipPlane));

            // Вычисляем расстояние по оси X между позицией объекта и позицией мыши
            float distanceX = transform.position.x - mouseWorldPosition.x;

            // Проверяем, находится ли мышь слева от объекта
            if (distanceX > 0)
            {
                // Устанавливаем масштаб линии натяжения в зависимости от расстояния
                scaleFactor = Mathf.Clamp(distanceX / 2f, 1f, 3f); // Измените делитель и пределы по необходимости

                // Увеличиваем линию натяжения по оси X (или Y в зависимости от вашей логики)
                stringObject.transform.localScale = new Vector3(1, scaleFactor, 1f);
            }
            else
            {
                // Если мышь справа от объекта, устанавливаем масштаб на минимальное значение
                stringObject.transform.localScale = new Vector3(1f, 1f, 1f);
                scaleFactor = 1f; // Сбрасываем scaleFactor
            }
        }


        private void stringPositionAndIncrease()
        {
            Vector3 stringPos = string_c.GetWorldPosition(transform);
            stringObject.transform.position = stringPos + new Vector3(1.5f, -0.6f, 0);

            var stringRot = string_c.WorldRotationY;
            stringObject.transform.rotation = Quaternion.Euler(0, 0, stringRot);
        }

        private void PlayFinishAttack()
        {
            var currentAnimation = spineAnimationState.GetCurrent(0);

            if (currentAnimation == null || currentAnimation.Animation.Name != attack_finish)
            {
                spineAnimationState.SetAnimation(0, attack_finish, false);
                isFinishingAttack = true;
                Debug.Log("Запуск анимации завершения атаки");

                ShootArrow(); // Запускаем стрелу при завершении атаки
            }
        }

        private void ShootArrow()
        {
            if (arrowPrefab != null && bulletBone != null)
            {
                GameObject arrowInstance = Instantiate(arrowPrefab);
                Vector3 bulletPosition = bulletBone.GetWorldPosition(transform);
                arrowInstance.transform.position = bulletPosition;
                float bulletRotation = bulletBone.WorldRotationY;
                arrowInstance.transform.rotation = Quaternion.Euler(0, 0, bulletRotation - 180f);
                Rigidbody2D rb = arrowInstance.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 direction = Quaternion.Euler(0, 0, bulletRotation - 90f) * Vector2.right;
                    rb.velocity = direction * arrowSpeed * scaleFactor; // Умножаем скорость на scaleFactor
                }

            }
        }

        private void PlayerIdleAnimation()
        {
            var currentAnimation = spineAnimationState.GetCurrent(0);

            if (currentAnimation == null || currentAnimation.Animation.Name != idle)
            {
                spineAnimationState.SetAnimation(0, idle, true);
                isAttacking = false;
            }
        }

        public bool IsPlayingIdle()
        {
            var currentAnimation = spineAnimationState.GetCurrent(0);
            return currentAnimation != null && currentAnimation.Animation.Name == idle;
        }

        private void OnMouseDown()
        {
            PlayStartAttackAnimation();
        }

        private void OnMouseUp()
        {
            isAttacking = false;
            PlayFinishAttack();
        }
    }
}
