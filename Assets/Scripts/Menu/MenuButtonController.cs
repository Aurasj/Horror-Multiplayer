using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] public int index;
    private bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

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
        }
        else
        {
            keyDown = false;
        }
    }
}
