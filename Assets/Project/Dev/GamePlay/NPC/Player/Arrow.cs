
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb != null && rb.velocity.magnitude > 0)
        {
            // Вычисляем угол наклона стрелы на основе направления скорости
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90 для корректного выравнивания
        }
    }
}
