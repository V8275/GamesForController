using UnityEngine;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private int levelCount = 1;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonHolder;
    [SerializeField] private GameController gameController;

    private int additiveIndex = 0;

    private void Awake()
    {
        SpawnLevelButtons();
    }

    void SpawnLevelButtons()
    {
        for (int i = 0; i < levelCount; i++)
        {
            var button = Instantiate(buttonPrefab, buttonHolder);
            int levelnum = i + 1 + additiveIndex;
            button.GetComponent<Button>().onClick.AddListener(() => gameController.LoadLevel(levelnum));

            button.GetComponentInChildren<Text>().text = levelnum.ToString();
        }
    }

    public void PlusStartIndex(int i)
    {
        additiveIndex += i;
    }

    public void ResetAdditiveIndex()
    {
        additiveIndex = 0;
    }
}
