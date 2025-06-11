using UnityEngine;

public class RandomAnimStart : MonoBehaviour
{
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        animator.Play(stateInfo.fullPathHash, 0, Random.Range(0f, 1f));
    }
}