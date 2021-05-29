using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;

    public void PlaySound(AudioClip Sound)
    {
        if (!disableOnce)
        {
            menuButtonController.audioSource.PlayOneShot(Sound);
        }
        else
        {
            disableOnce = false;
        }
    }
}
