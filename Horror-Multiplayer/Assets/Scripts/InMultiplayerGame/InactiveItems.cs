using UnityEngine;

public class InactiveItems : MonoBehaviour
{
    public static InactiveItems instance;

    [SerializeField] public GameObject pushToTalkIcon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
