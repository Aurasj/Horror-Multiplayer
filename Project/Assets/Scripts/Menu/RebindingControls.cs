using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingControls : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;
    public void SaveBindings()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        Debug.Log("Save bindings!");
    }
    public void ResetBindings()
    {
        foreach(InputActionMap map in actions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
            Debug.Log("Reset bindings!");
        }
        PlayerPrefs.DeleteKey("rebinds");
    }

}
