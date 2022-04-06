using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum InteractType
{
    PickupItem,
    EnterCode,
    ClimbLadder,
    UseVehicle,
    StealthKill
}
[System.Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private int interactPointsAmount;
    [SerializeField] private Transform[] interactPoints;

    public Transform[] InteractPoints => interactPoints;

    public void SetPointsAmount(int amount)
    {
        interactPointsAmount = amount;
    }
}

public class ActionInteract : EntityActionContext
{
    [SerializeField] protected Transform owner;
    [SerializeField] private UnityEngine.Animations.Rigging.Rig leftHandRig;
    [SerializeField] private InteractType interactType;
    [SerializeField] private int currentPointIndex = 0;

    public InteractType InteractType => interactType;
    public int CurrentPointIndex => currentPointIndex;

    protected new ActionInteractProperties Properties;

    public override void SetProperties(EntityActionProperties properties)
    {
        Properties = properties as ActionInteractProperties;
    }

    public override void StartAction()
    {
        if (IsCanStart == false) return;
        if (leftHandRig != null) leftHandRig.weight = 0;
        if (Properties.InteractPoints.Length != 0)
        {
            owner.DOMove(Properties.InteractPoints[currentPointIndex].position, 0.5f).OnComplete(() => { base.StartAction(); });
        }
        else
        {
            base.StartAction();
        }
        currentPointIndex++;
        if (currentPointIndex >= Properties.InteractPoints.Length) currentPointIndex = 0;
    }

    public override void EndAction()
    {
        if (leftHandRig != null) leftHandRig.weight = 1;
        if (m_IsStarted && m_IsPlayingAnimation == false) IsCanEnd = true;
        base.EndAction();
    }
}
