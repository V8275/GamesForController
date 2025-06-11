using UnityEngine;

namespace SubmarineGame
{
    public class MovingObject : MonoBehaviour, IUnit
    {
        [SerializeField]
        private float Speed = 1;
        [SerializeField]
        private Vector3 MoveDirection = Vector3.forward;

        private int Side;
        private SceneController SController;

        protected GameEvents gameEvents;

        public void Init(Vector3 spawnPoint, SceneController controller, GameEvents events, int side = 1)
        {
            Side = side;
            SController = controller;
            transform.position = spawnPoint;
            gameEvents = events;
        }

        public void UpdateUnit()
        {
            transform.Translate(MoveDirection * Speed * Time.deltaTime * Side);
        }

        public void ForceDestroy()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            SController.DeleteUnitFromList(this);
        }
    }

    public interface IUnit
    {
        public void Init(Vector3 spawnPoint, SceneController controller, GameEvents events, int side = 1);

        public void UpdateUnit();

        public void ForceDestroy();
    }
}