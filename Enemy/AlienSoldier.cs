using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSoldier : Destructible, ISoundListener
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private SpreadShootRig spreadShootRig;
    [SerializeField] private AIAlienSoldier aiAlienSoldier;
    [SerializeField] private float hearingDistance;

    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }

    public void Fire(Vector3 point)
    {
        if (weapon.CanFire == false) return;

        weapon.FirePointLookAt(point);
        weapon.Fire();
        spreadShootRig.Spread();
    }

    public void Heard(float distance)
    {
        if (distance <= hearingDistance)
        {
            aiAlienSoldier.OnHeard();
        }
    }

    [System.Serializable]
    public class AIAlienState
    {
        public Vector3 Position;
        public int HitPoints;
        public int Behaivour;
        public int PathId;

        public AIAlienState(Vector3 position, int hitPoints, int behaivour, int pathId)
        {
            Position = position;
            HitPoints = hitPoints;
            Behaivour = behaivour;
            PathId = pathId;
        }
    }

    public override string SerializeState()
    {
        AIAlienState state = new AIAlienState(transform.position, CurrentHitPoints, (int)aiAlienSoldier.CurrentBehaivour, aiAlienSoldier.Path.Id);
        return JsonUtility.ToJson(state);
    }

    public override void DeserializeState(string state)
    {
        AIAlienState savedState = JsonUtility.FromJson<AIAlienState>(state);
        aiAlienSoldier.SetPosition(savedState.Position);
        m_CurrentHitPoints = savedState.HitPoints;
        aiAlienSoldier.CurrentBehaivour = (AIBehaivour)savedState.Behaivour;
        aiAlienSoldier.SetPathFromId(savedState.PathId);
    }
}
