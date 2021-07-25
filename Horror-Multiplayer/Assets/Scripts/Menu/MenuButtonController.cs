using TMPro;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] private TMP_Text highlightedText;

    [SerializeField] public AudioSource audioSource;
    public void CheckIndex(int indexx)
    {
        index = indexx;
        CheckHighlightedText();
    }
    private void CheckHighlightedText()
    {
        if (index == 0)
        {
            highlightedText.text = "Singleplayer";
        }
        else if (index == 1)
        {
            highlightedText.text = "Multiplayer";
        }
        else if (index == 2)
        {
            highlightedText.text = "Settings";
        }
        else if (index == 3)
        {
            highlightedText.text = "Credits";
        }
        else if (index == 4)
        {
            highlightedText.text = "Quit";
        }
        else
        {
            highlightedText.text = "Nothing Selected";
        }
    }
}
