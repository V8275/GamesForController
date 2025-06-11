using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SubmarineGame
{
    public class ObstacleFactory : Spawner
    {
        [SerializeField]
        private ObstacleObject[] ObstaclePrefabs;

        private List<Transform> SpawnPointsTemp;

        public override void Init()
        {
            SpawnPointsTemp = new List<Transform>();
            SpawnPointsTemp = SpawnPoints.ToList();
        }

        public override IUnit SpawnUnit()
        {
            int randNum = Random.Range(0, ObstaclePrefabs.Length);
            ObstacleObject prefab = Instantiate(ObstaclePrefabs[randNum]);
            return prefab;
        }

        public override Transform SetRandomSpawnPoint()
        {
            int randNum = Random.Range(0, SpawnPointsTemp.Count);
            var RandomPoint = SpawnPointsTemp[randNum];
            SpawnPointsTemp.RemoveAt(randNum);
            if(SpawnPointsTemp.Count == 0)
                SpawnPointsTemp = SpawnPoints.ToList();
            return RandomPoint;
        }
    }
}
