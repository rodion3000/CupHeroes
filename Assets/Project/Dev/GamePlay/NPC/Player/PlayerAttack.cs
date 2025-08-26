
using Spine;
using Spine.Unity;
using UnityEngine;
using Zenject;

public class PlayerAttack : ITickable, IInitializable
{
    private readonly GameObject player;
    private readonly GameObject arrowPrefab;
    public float arrowSpeed { get; private set; }
    public float tiltSpeed { get; private set; } // Скорость наклона
    public GameObject stringObject { get; private set; }
    public float maxTiltAngle { get; private set; } // Максимальный угол наклона
    private float scaleFactor = 1f;
    private Bone gunBone; 
    private Bone bulletBone; 
    private Bone string_c;
    private Skeleton skeleton;
    private SkeletonAnimation skeletonAnimation;
    private Vector3 lastMousePosition; // Последняя позиция мыши
    private PlayerAnimationController _playerAnimationController;
    private DiContainer _container;

    [Inject]
   // private void Construct(PlayerAnimationController playerAnimationController, DiContainer container, HeroLocalData config)
   // {
   //     _playerAnimationController = playerAnimationController;
   //     _container = container;
    //    player = config.player;
    //    arrowPrefab = config.arrowPrefab;
     //   arrowSpeed = config.arrowSpeed;
     //   tiltSpeed = config.tiltSpeed;
      //  stringObject = config.stringObject;
      //  maxTiltAngle = config.maxTiltAngle;
  //  }//
    public void Initialize()
    {
        skeleton = skeletonAnimation.Skeleton;
        gunBone = skeleton.FindBone("gun");
        bulletBone = skeleton.FindBone("bullet");
        string_c = skeleton.FindBone("string_c");
        lastMousePosition = Input.mousePosition; // Инициализируем последнюю позицию мыши
    }

    public void Tick()
    {
        Attack();
        lastMousePosition = Input.mousePosition;
    }
    
    private void Attack()
    {
        if (_playerAnimationController.isAttacking)
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
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // Вычисляем расстояние по оси X между позицией объекта и позицией мыши
        float distanceX = player.transform.position.x - mouseWorldPosition.x;

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
        Vector3 stringPos = string_c.GetWorldPosition(player.transform);
        stringObject.transform.position = stringPos + new Vector3(1.5f, -0.6f, 0);
        
        var stringRot = string_c.WorldRotationY;
        stringObject.transform.rotation = Quaternion.Euler(0, 0, stringRot);
    }
    public void ShootArrow()
    {
        if (arrowPrefab != null && bulletBone != null)
        {
            // Создаем экземпляр стрелы с помощью контейнера
            GameObject arrowInstance = _container.InstantiatePrefab(arrowPrefab, bulletBone.GetWorldPosition(player.transform), Quaternion.Euler(0, 0, bulletBone.WorldRotationY - 180f), null);
        
            Rigidbody2D rb = arrowInstance.GetComponent<Rigidbody2D>();
    
            if (rb != null)
            {
                Vector2 direction = Quaternion.Euler(0, 0, bulletBone.WorldRotationY - 90f) * Vector2.right; 
                rb.velocity = direction * arrowSpeed * scaleFactor; 
            }
        }
    }
}
