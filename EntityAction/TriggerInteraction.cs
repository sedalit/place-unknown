using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerInteraction : MonoBehaviour
{
    [SerializeField] private UnityEvent onStartInteract;
    [SerializeField] private UnityEvent onEndInteract;
    [SerializeField] private InteractType interactType;
    [SerializeField] private int interactAmount;
    [SerializeField] protected ActionInteractProperties interactProperties;

    protected ActionInteract action;
    protected GameObject owner;

    public UnityEvent OnStartInteract => onStartInteract;
    public UnityEvent OnEndInteract => onEndInteract;
    public ActionInteractProperties InteractProperties => interactProperties;

    protected virtual void InitActionProperties()
    {
        action.SetProperties(interactProperties);
    }

    protected virtual void OnStartAction(GameObject owner){ }

    protected virtual void OnEndAction(GameObject owner){ }

    private void OnTriggerEnter(Collider other)
    {
        if (interactAmount == 0 && interactAmount != -1) return;
        if (other.TryGetComponent(out EntityActionCollector actionCollector))
        {
            action = GetActionInteract(actionCollector);

            if (action != null)
            {
                InitActionProperties();
                action.IsCanStart = true;
                owner = other.gameObject;
                action.OnStartAction.AddListener(ActionStart);
                action.OnEndAction.AddListener(ActionEnd);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactAmount == 0 && interactAmount != -1) return;
        if (other.TryGetComponent(out EntityActionCollector actionCollector))
        {
            action = GetActionInteract(actionCollector);

            if (action != null)
            {
                action.IsCanStart = false;
                owner = null;
                action.OnStartAction.RemoveListener(ActionStart);
                action.OnEndAction.RemoveListener(ActionEnd);
            }
        }
    }

    private void ActionStart()
    {
        OnStartAction(owner);
        if (interactAmount != -1) interactAmount--;
        onStartInteract?.Invoke();
    }

    private void ActionEnd()
    {
        action.IsCanStart = false;
        action.IsCanEnd = false;
        action.OnStartAction.RemoveListener(ActionStart);
        action.OnEndAction.RemoveListener(ActionEnd);
        onEndInteract?.Invoke();

        OnEndAction(owner);
    }

    private ActionInteract GetActionInteract(EntityActionCollector actionCollector)
    {
        List<ActionInteract> actions = actionCollector.GetActionList<ActionInteract>();

        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].InteractType == interactType)
            {
                return actions[i];
            }
        }
        return null;
    }
}
