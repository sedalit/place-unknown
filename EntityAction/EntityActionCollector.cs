﻿using System.Collections.Generic;
using UnityEngine;

public class EntityActionCollector : MonoBehaviour
{
    [SerializeField] private Transform m_ParentWithActions;
    [SerializeField] private List<EntityAction> m_AllActions = new List<EntityAction>();

    private void Awake()
    {
        for (int i = 0; i < m_ParentWithActions.childCount; i++)
        {
            if (m_ParentWithActions.GetChild(i).gameObject.activeSelf)
            {
                EntityAction action = m_ParentWithActions.GetChild(i).GetComponent<EntityAction>();
                if (action != null)
                {
                    m_AllActions.Add(action);
                }
            }
        }
    }
    public T GetAction<T>() where T : EntityAction
    {
        for (int i = 0; i < m_AllActions.Count; i++)
        {
            if (m_AllActions[i] is T)
            {
                return (T)m_AllActions[i];
            }
        }
        return null;
    }
    public List<T> GetActionList<T>() where T : EntityAction
    {
        List<T> actions = new List<T>();
        for (int i = 0; i < m_AllActions.Count; i++)
        {
            if (m_AllActions[i] is T)
            {
                actions.Add((T)m_AllActions[i]);
            }
        }
        return actions;
    }
}
