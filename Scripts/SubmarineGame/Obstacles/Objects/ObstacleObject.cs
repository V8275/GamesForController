using UnityEngine;

namespace SubmarineGame
{
    public class ObstacleObject : MovingObject
    {
        [SerializeField]
        private string playerTag;

        private void OnCollisionEnter(Collision collision)
        {
            var ColObject = collision.gameObject;
            if (ColObject.CompareTag(playerTag))
            {
                ColObject.GetComponent<PlayerController>().Collide();
            }
            if (ColObject.CompareTag("Destruct"))
            {
                Destroy(gameObject);
            }
        }
    }
}