using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    [SerializeField]
    private Vector3 MoveDirection = Vector3.left;

    [SerializeField]
    private float minSpeed = 0.1f; // ����������� ��������
    [SerializeField]
    private float maxSpeed = 1.0f; // ������������ ��������

    private float Speed;

    private void Start()
    {
        // ������������� ��������� �������� � �������� ���������
        Speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        transform.Translate(MoveDirection * Speed * Time.deltaTime);
    }
}
