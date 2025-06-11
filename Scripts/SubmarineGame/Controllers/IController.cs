using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public abstract void Init(GameEvents events);

    public abstract void UpdateController();
}
