using DG.Tweening;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField]
    private Material[] MovementMaterials;
    [SerializeField]
    private string MoveVectorName = "_Movement";
    [SerializeField]
    private Vector4 MoveVector = Vector4.zero;
    private const float defaultRotateTime = 1;
    private Quaternion initialRotation;

    public void Init(GameEvents events)
    {
        events.OnGameFinished += (isWin) => StopGame();
        initialRotation = transform.rotation;
        foreach (var movementMaterial in MovementMaterials)
            movementMaterial.SetVector(MoveVectorName, MoveVector);
    }

    private void StopGame()
    {
        foreach (var movementMaterial in MovementMaterials)
            movementMaterial.SetVector(MoveVectorName, Vector4.zero);
    }

    public void Rotate(float angle, float time = defaultRotateTime)
    {
        transform.DORotate(initialRotation.eulerAngles + new Vector3(0, 0, angle), time);
    }

    public void DefaultRotation(float time = defaultRotateTime)
    {
        transform.DORotate(initialRotation.eulerAngles, defaultRotateTime);
    }
}
