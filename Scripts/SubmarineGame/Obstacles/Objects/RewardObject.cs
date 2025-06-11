using DG.Tweening;
using UnityEngine;

namespace SubmarineGame
{
    public class RewardObject : MovingObject
    {
        [SerializeField]
        private string playerTag = "Player";
        [SerializeField]
        private int ScoreCount = 1;
        [SerializeField]
        private float RotateDuration = 2;
        [SerializeField]
        private GameObject RotationObject;


        void Start()
        {
            RotationObject.transform.DORotate(new Vector3(0, 360, 0), RotateDuration, RotateMode.FastBeyond360)
                     .SetEase(Ease.Linear)
                     .SetLoops(-1, LoopType.Restart);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var ColObject = collision.gameObject;
            if (ColObject.CompareTag(playerTag))
            {
                var scorecontroller = FindFirstObjectByType<ScoreController>();
                if (scorecontroller)
                {
                    scorecontroller.SetCurrentScore(ScoreCount);
                    int score = scorecontroller.GetCurrentScore();

                    gameEvents.ChangeScore(score);
                }

                Destroy(gameObject);
            }
            if (ColObject.CompareTag("Destruct"))
            {
                Destroy(gameObject);
            }
        }
    }
}

