using UnityEngine;

public class BaseActivatableModifier : MonoBehaviour
{
    public virtual bool activatable
    {
        get
        {
            return true;
        }
    }
}