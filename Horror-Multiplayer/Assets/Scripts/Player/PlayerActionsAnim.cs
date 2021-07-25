using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionsAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerController playerController;


    public void Ok(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!playerController.isMoving)
            {
                anim.SetBool("ok", true);
            }
        }
        else if (context.canceled)
        {
            anim.SetBool("ok", false);
        }
    }

}
