using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    private Transform playertr;

    private void Start()
    {
        playertr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 newPosition = new Vector3 (transform.position.x, playertr.position.y, transform.position.z);
        transform.position = newPosition;
    }
}