using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    public int Id;
    internal int cachedId = -1;

    protected virtual void InitializeInteractable()
    {
        EventManager.OnInstructionDefined += CacheCurrentId;
    }

    protected virtual void DisableInteractable()
    {
        EventManager.OnInstructionDefined -= CacheCurrentId;
        cachedId = -1;
    }

    private void CacheCurrentId(int id)
    {
        cachedId = id;
    }

    protected void SelectionMade()
    {
        if (cachedId == -1)
        {
            return;
        }

        if (cachedId != Id)
        {
            EventManager.OnInstructionFailed?.Invoke();
        }
        else
        {
            EventManager.OnInstructionPassed?.Invoke();
        }
    }
}
