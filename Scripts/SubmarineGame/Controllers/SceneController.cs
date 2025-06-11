using System.Collections.Generic;
using UnityEngine;

namespace SubmarineGame
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField]
        private Spawner[] spawners;
        [SerializeField]
        private DecoratorFactory DecorativeSpawner;
        [SerializeField]
        private ScriptableSceneData SceneData;
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private Controller[] controllers;
        [SerializeField]
        private int levelNumber = 0;
        [SerializeField]
        private int MaxDecorationsOnLevel = 0;

        private int MaxCountObjects = 0;
        private int MaxCountSceneObjects = 0;
        private int ObjectCount = 0;
        private int OnSceneObjectCount = 0;
        private int MaxDecorationsOnLevel_Temp = 0;

        private GameEvents gameEvents;
        private ScoreController scoreController;

        private bool LevelPlay = true;

        private List<IUnit> OnSceneUnits;
        private List<IUnit> OnSceneDecorations;

        private void Start()
        {
            gameEvents = new GameEvents();
            gameEvents.OnGameFinished += (isWin) =>
            {
                EndGame(isWin);
            };
            scoreController = FindAnyObjectByType<ScoreController>();
            if (scoreController != null )
                scoreController.Init(levelNumber);

            MaxCountObjects = SceneData.MaxCountObjects;
            MaxCountSceneObjects = SceneData.MaxCountSceneObjects;
            MaxDecorationsOnLevel_Temp = MaxDecorationsOnLevel;

            OnSceneUnits = new List<IUnit>();
            OnSceneDecorations = new List<IUnit>();

            foreach (var spawner in spawners) spawner.Init();
            foreach (var controller in controllers) controller.Init(gameEvents);

            DecorativeSpawner.Init();
            playerController.Init(gameEvents);
        }

        private void Update()
        {
            if (LevelPlay)
            {
                ControlObjects();
                UpdateScripts();
            }
        }

        private void UpdateScripts()
        {
            foreach (var obj in OnSceneUnits) obj.UpdateUnit();

            foreach (var controller in controllers) controller.UpdateController();

            foreach (var decors in OnSceneDecorations) decors.UpdateUnit();

            playerController.UpdatePlayer();
        }

        private void ControlObjects()
        {
            if (ObjectCount < MaxCountObjects)
            {
                if(OnSceneObjectCount < MaxCountSceneObjects)
                {
                    int randNum = Random.Range(0, spawners.Length);
                    var Spawnpoint = spawners[randNum].SetRandomSpawnPoint();
                    IUnit newUnit = spawners[randNum].SpawnUnit();

                    newUnit.Init(Spawnpoint.position, this, gameEvents);

                    OnSceneUnits.Add(newUnit);
                    OnSceneObjectCount++;
                    ObjectCount++;
                }
            }
            else if(ObjectCount >= MaxCountObjects)
            {
                if(OnSceneUnits.Count <= 0)
                    gameEvents.TriggerGameFinish(true);
            }

            if (MaxDecorationsOnLevel > 0)
            {
                ControlDecorations();
                MaxDecorationsOnLevel--;
            }
        }

        private void ControlDecorations()
        {
            var Spawnpoint = DecorativeSpawner.SetRandomSpawnPoint();
            IUnit newUnit = DecorativeSpawner.SpawnUnit();

            newUnit.Init(Spawnpoint.position, this, gameEvents);

            OnSceneDecorations.Add(newUnit);
        }

        public void DeleteUnitFromList(IUnit unit)
        {
            if (OnSceneUnits.Contains(unit))
            {
                OnSceneUnits.Remove(unit);
                OnSceneObjectCount--;
            }
            else
            {
                OnSceneDecorations.Remove(unit);
                MaxDecorationsOnLevel++;
            }
        }

        private void EndGame(bool win)
        {
            LevelPlay = false;
            playerController.OnEndGame();

            if (win)
            {
                print("Win!");
            }
            else
            {
                print("Game Over!");
            }
        }

        public GameEvents GetGameEvents()
        {
            return gameEvents;
        }
    }
}
