using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] int thisIndex;

    private void Update()
    {
        ActionMenuButton();
    }
    public void ActionMenuButton()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }
    public void SubmitMenuButton()
    {
        animator.SetBool("pressed", true);
    }
}
