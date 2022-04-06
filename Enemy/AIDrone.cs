using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Drone))]
public class AIDrone : MonoBehaviour
{
    [SerializeField] private CubeArea droneMovementArea;
    [SerializeField] private ColliderViewer colliderViewer;

    public CubeArea MovementArea { get { return droneMovementArea; } set { droneMovementArea = value; } }

    private Drone m_Drone;
    private Vector3 m_MovementTarget;
    private Transform m_ShootTarget;

    private void Start()
    {
        FindMovementArea();
        m_MovementTarget = droneMovementArea.GetRandomInsideZone();
        m_Drone = GetComponent<Drone>();
        m_Drone.EventOnDeath.AddListener(OnDroneDeath);
        m_Drone.GetDamage += OnGetDamage;
    }
    private void OnDisable()
    {
        m_Drone.EventOnDeath.RemoveListener(OnDroneDeath);
        m_Drone.GetDamage -= OnGetDamage;
    }
    private void Update()
    {
        UpdateAI();
    }
    private void UpdateAI()
    {
        ActionFindShootTarget();
        ActionMove();
        ActionFire();
    }

    private void OnGetDamage(Destructible other)
    {
        ActionAssignTargetToTeam(other.transform);
    }

    private void OnDroneDeath()
    {
        enabled = false;
    }

    private void ActionFindShootTarget()
    {
        Transform potentialTarget = FindShootTarget();
        if (potentialTarget != null)
        {
            ActionAssignTargetToTeam(potentialTarget);
        }
    }

    private void ActionMove()
    {
        if (transform.position == m_MovementTarget)
        {
            m_MovementTarget = droneMovementArea.GetRandomInsideZone();
        }
        if (Physics.Linecast(transform.position, m_MovementTarget))
        {
            m_MovementTarget = droneMovementArea.GetRandomInsideZone();
        }
        m_Drone.MoveTo(m_MovementTarget);
        if (m_ShootTarget != null)
        {
            m_Drone.LookAt(m_ShootTarget.position);
        }
        else
        {
            m_Drone.LookAt(m_MovementTarget);
        }
    }

    private void ActionFire()
    {
        if (m_ShootTarget != null)
        {
            m_Drone.Attack(m_ShootTarget.position);
        }
    }

    private void FindMovementArea()
    {
        if (droneMovementArea == null)
        {
            CubeArea[] cubeAreas = FindObjectsOfType<CubeArea>();
            float minDistance = float.MaxValue;

            foreach (var area in cubeAreas)
            {
                if (Vector3.Distance(area.transform.position, transform.position) < minDistance)
                {
                    droneMovementArea = area;
                }
            }
        }
    }

    public void SetMovementAreaFromId(int id)
    {
        CubeArea[] cubeAreas = FindObjectsOfType<CubeArea>();
        foreach (var area in cubeAreas)
        {
            if (area.Id == id)
            {
                droneMovementArea = area;
            }
        }
    }

    public void SetShootTarget(Transform target)
    {
        m_ShootTarget = target;
    }

    private Transform FindShootTarget()
    {
        List<Destructible> targets = Destructible.GetAllNoneTeamMembers(m_Drone.TeamId);
        foreach (var target in targets)
        {
            if (colliderViewer.IsObjectVisible(target.gameObject)) return target.transform;
        }
        return null;
    }

    private void ActionAssignTargetToTeam(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMembers(m_Drone.TeamId);
        foreach (var mate in team)
        {
            AIDrone ai = mate.transform.root.GetComponent<AIDrone>();
            Debug.Log(mate.name);
            if (ai != null && ai.enabled == true)
            {
                ai.SetShootTarget(other);
            }
        }
    }
}
