using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    [SerializeField]
    private Vector3 MoveDirection = Vector3.left;

    [SerializeField]
    private float minSpeed = 0.1f; // Минимальная скорость
    [SerializeField]
    private float maxSpeed = 1.0f; // Максимальная скорость

    private float Speed;

    private void Start()
    {
        // Устанавливаем случайную скорость в заданном диапазоне
        Speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        transform.Translate(MoveDirection * Speed * Time.deltaTime);
    }
}
