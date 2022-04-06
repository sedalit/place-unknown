using UnityEngine;
using UnityEngine.Events;

public abstract class EntityActionProperties
{

}

public abstract class EntityAction : MonoBehaviour
{
    [SerializeField] private UnityEvent m_OnStartAction;
    [SerializeField] private UnityEvent m_OnEndAction;

    private EntityActionProperties m_Properties;
    protected bool m_IsStarted;

    public UnityEvent OnStartAction => m_OnStartAction;
    public UnityEvent OnEndAction => m_OnEndAction;
    public EntityActionProperties Properties => m_Properties;

    public virtual void StartAction()
    {
        if (m_IsStarted) return;
        m_IsStarted = true;
        m_OnStartAction?.Invoke();
    }
    public virtual void EndAction()
    {
        m_IsStarted = false;
        m_OnEndAction?.Invoke();
    }
    public virtual void SetProperties(EntityActionProperties properties)
    {
        m_Properties = properties;
    }
}
