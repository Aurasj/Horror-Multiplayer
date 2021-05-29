using UnityEngine;
using TMPro;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] public int index;
    private bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;
    [SerializeField] private TMP_Text highlightedText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        CheckIndex(index);
    }
    public void CheckIndex(int indexx)
    {
        index = indexx;
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                        indexx++;
                    }
                    else
                    {
                        index = 0;
                        indexx = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                        indexx--;
                    }
                    else
                    {
                        index = maxIndex;
                        indexx = maxIndex;
                    }
                }
                keyDown = true;
            }
            CheckHighlightedText();
        }
        else
        {
            keyDown = false;
        }
    }
    public void CheckHighlightedText()
    {
        if(index == 0)
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
