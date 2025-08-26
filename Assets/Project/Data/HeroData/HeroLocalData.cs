using UnityEngine;

namespace Project.Data.HeroLocalData
{
    [CreateAssetMenu(fileName = "HeroLocalData", menuName = "Configs/HeroConfig/Hero Local Data")]

    public class HeroLocalData : ScriptableObject
    {
        [field: SerializeField] public float arrowSpeed = 5; // Скорость полета стрелы
        [field: SerializeField] public float tiltSpeed = 5; // Скорость наклона
        [field: SerializeField] public float maxTiltAngle = 5; // Максимальный угол наклона
    }
}
