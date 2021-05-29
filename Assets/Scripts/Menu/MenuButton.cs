using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }
    private void Update()
    {
        ActionMenuButton();
    }
    private void ActionMenuButton()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if(Input.GetButton("Submit"))
            {
                animator.SetBool("pressed", true);
                button.Select();
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
            }
            if (Input.GetMouseButton(0))
            {
                animator.SetBool("pressed", true);
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }


}
