using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInputController : MonoBehaviour
{
    [SerializeField] private EntityActionCollector actionCollector;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<EntityActionContext> actions = actionCollector.GetActionList<EntityActionContext>();
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].StartAction();
                actions[i].EndAction();
            }
        }
    }
}
