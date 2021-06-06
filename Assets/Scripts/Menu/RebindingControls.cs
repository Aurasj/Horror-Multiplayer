using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingControls : MonoBehaviour
{
    [SerializeField] private InputActionReference moveForward = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private Button bindingButton = null;
    [SerializeField] private GameObject startingRebindObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;

    public void StartRebinding()
    {
        startingRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        moveForward.action.PerformInteractiveRebinding();

    }


}
