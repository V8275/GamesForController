using UnityEngine;
using UnityEngine.UI;

public class UIFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource ButtonClickSound;

    private void Start()
    {
        SetSoundOnButtons();
    }

    private void SetSoundOnButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(ClickSound);
        }
    }

    private void ClickSound()
    {
        if (ButtonClickSound != null)
        {
            ButtonClickSound.Play();
        }
    }
}
