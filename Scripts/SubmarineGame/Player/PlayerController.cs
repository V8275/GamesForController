using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int HP = 1;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private EffectsController effectsController;
    [SerializeField] private float rotationResetThreshold = -0.1f;
    [SerializeField] private Rigidbody rb;

    private bool isGrounded = true;
    private bool touchinput = true;
    private GameEvents gameEvents;

    public void Init(GameEvents events)
    {
        gameEvents = events;
        effectsController?.Init(gameEvents);


        if (WebSocketManager.Instance != null)
            if (WebSocketManager.Instance.isConnected())
                touchinput = false;
        //rb = GetComponent<Rigidbody>();
        //if (rb == null)
        //{
        //    rb = gameObject.AddComponent<Rigidbody>();
        //    rb.useGravity = true;
        //    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //}
    }

    public void UpdatePlayer()
    {
        if (Input.touchCount > 0 && touchinput)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !IsPointerOverUIObject(touch))
            {
                Jump();
            }
        }

        CheckFalling();
    }

    private void Jump()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        effectsController?.Rotate(15);
    }

    public void Jump(bool isj)
    {
        if(isj)
        {
            Jump();
            isj = false;
        }
    }

    private void CheckFalling()
    {
        if (!isGrounded && rb.linearVelocity.y < rotationResetThreshold)
        {
            effectsController?.DefaultRotation();
        }
    }

    private bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touch.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Фильтрация результатов - проверяем только UI с определенным тегом/слоем
        foreach (var result in results)
        {
            // Пример: игнорируем только объекты с тегом "IgnoreUI"
            if (result.gameObject.CompareTag("IgnoreUI"))
            {
                return true;
            }

            // Или можно проверять по слою
            // if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            // {
            //     return true;
            // }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            effectsController?.DefaultRotation();
        }
    }

    public void Collide()
    {
        HP--;
        if (HP < 0) gameEvents.TriggerGameFinish(false);
    }

    public void OnEndGame()
    {
        if (effectsController)
            effectsController?.DefaultRotation();
        rb.isKinematic = true;
    }

    void OnEnable()
    {
        if (WebSocketManager.Instance != null)
            WebSocketManager.Instance.OnDataReceived += HandleData;
    }

    void OnDisable()
    {
        if (WebSocketManager.Instance != null)
            WebSocketManager.Instance.OnDataReceived -= HandleData;
    }

    private void HandleData(SensorData data)
    {
        Jump(data.strength > 50);
    }

    public void SetInput(bool touch)
    {
        touchinput = touch;
    }
}