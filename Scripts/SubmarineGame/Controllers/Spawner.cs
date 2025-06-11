using UnityEngine;

namespace SubmarineGame
{
    public abstract class Spawner : MonoBehaviour
    {
        [SerializeField]
        protected Transform[] SpawnPoints;

        public abstract void Init();

        public abstract IUnit SpawnUnit();

        public abstract Transform SetRandomSpawnPoint();
    }
}