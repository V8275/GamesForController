using SubmarineGame;
using UnityEngine;

public class DecorativeObject : MovingObject
{
    private void OnCollisionEnter(Collision collision)
    {
        var ColObject = collision.gameObject;
        if (ColObject.CompareTag("Destruct"))
        {
            Destroy(gameObject);
        }
    }
}
