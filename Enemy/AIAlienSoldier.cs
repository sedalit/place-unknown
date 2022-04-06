using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum AIBehaivour
{
    None = 0,
    Idle = 1,
    PatrolRandom = 2,
    CirclePatrol = 3,
    PursuitTarget = 4,
    SeekTarget = 5,
    FindLostTarget = 6
}
public class AIAlienSoldier : MonoBehaviour
{
    public event UnityAction<AIBehaivour> BehaviourChanged;

    [SerializeField] private AIBehaivour aiBehaivour;
    [SerializeField] private AlienSoldier alienSoldier;
    [SerializeField] private ColliderViewer viewer;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private GameObject potentialTarget;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PatrolPath path;
    [SerializeField] private float aimingDistance;
    [SerializeField] private int patrolPathNodeIndex = 0;
    [SerializeField] private float secondsToFindTarget = 60;
    [SerializeField] private CubeArea cubeAreaPrefab;

    public AIBehaivour CurrentBehaivour { get { return aiBehaivour; } set { aiBehaivour = value; } }
    public PatrolPath Path => path;

    private NavMeshPath navMeshPath;
    private PatrolPathNode currentPathNode;
    private Transform pursuitTarget;
    private Vector3 seekTarget;
    private CubeArea findZone;
    private bool isTargetDetected;

    private void Start()
    {
        characterMovement.UpdatePosition = false;
        navMeshPath = new NavMeshPath();
        potentialTarget = Player.Instance.gameObject;
        FindPatrolPath();
        StartBehaivour(aiBehaivour);

        alienSoldier.GetDamage += OnGetDamage;
        alienSoldier.EventOnDeath.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        alienSoldier.GetDamage -= OnGetDamage;
        alienSoldier.EventOnDeath.RemoveListener(OnDeath);
    }

    private void Update()
    {
        SyncAgentWithCharacterMovement();
        UpdateAI();
    }
    private void UpdateAI()
    {
        ActionUpdateTarget();
        if (aiBehaivour == AIBehaivour.Idle)
        {
            return;
        }
        if (aiBehaivour == AIBehaivour.PursuitTarget)
        {
            agent.CalculatePath(pursuitTarget.position, navMeshPath);
            agent.SetPath(navMeshPath);
            characterMovement.Aiming();
            if (Vector3.Distance(transform.position, pursuitTarget.position) <= aimingDistance)
            {
                if (Vector3.Angle(pursuitTarget.position - transform.position, transform.forward) < 10f)
                {
                    agent.isStopped = true;
                }
                alienSoldier.Fire(pursuitTarget.position + new Vector3(0, 1, 0));
            }
        }
        if (aiBehaivour == AIBehaivour.SeekTarget)
        {
            agent.CalculatePath(seekTarget, navMeshPath);
            agent.SetPath(navMeshPath);
            if (AgentReachedDestination())
            {
                StartBehaivour(AIBehaivour.FindLostTarget);
            }
        }
        if (aiBehaivour == AIBehaivour.PatrolRandom)
        {
            if (AgentReachedDestination())
            {
                StartCoroutine(SetBehaviour(AIBehaivour.Idle, currentPathNode.IdleTime));
            }
        }
        if (aiBehaivour == AIBehaivour.CirclePatrol)
        {
            if (AgentReachedDestination())
            {
                StartCoroutine(SetBehaviour(AIBehaivour.Idle, currentPathNode.IdleTime));
            }
        }
        if (aiBehaivour == AIBehaivour.FindLostTarget)
        {
            InitTargetFinding();
        }
    }

    public void SetPosition(Vector3 position)
    {
        agent.Warp(position);
    }

    public void SetPathFromId(int id)
    {
        foreach (var path in FindObjectsOfType<PatrolPath>())
        {
            if (path.Id == id)
            {
                this.path = path;
                break;
            }
        }
    }

    public void SetPursueTarget(Transform target)
    {
        pursuitTarget = target;
    }

    public void OnHeard()
    {
        ActionStartPursuit();
    }

    private void InitTargetFinding()
    {
        if (findZone == null)
        {
            findZone = Instantiate(cubeAreaPrefab);
            findZone.transform.position = seekTarget;
            findZone.movementArea = new Vector3(seekTarget.x, seekTarget.y, seekTarget.z * 2f);
        }
        if (AgentReachedDestination())
        {
            Vector3 point = transform.position + findZone.GetRandomInsideZone();
            agent.CalculatePath(point, navMeshPath);
            agent.SetPath(navMeshPath);
            StartCoroutine(FindTargetInLastSeenZone(point));
        }
    }

    private IEnumerator FindTargetInLastSeenZone(Vector3 firstPoint)
    {
        yield return new WaitForSeconds(secondsToFindTarget);
        if (findZone != null)
        {
            Destroy(findZone.gameObject);
            findZone = null;
        }
        if (path != null) StartBehaivour(AIBehaivour.CirclePatrol);
        else StartBehaivour(AIBehaivour.Idle);
    }
    private IEnumerator SetBehaviour(AIBehaivour state, float time)
    {
        AIBehaivour previous = aiBehaivour;
        aiBehaivour = state;
        StartBehaivour(aiBehaivour);
        yield return new WaitForSeconds(time);
        StartBehaivour(previous);
    }
    private void StartBehaivour(AIBehaivour state)
    {
        if (alienSoldier.IsDead) return;
        if (state == AIBehaivour.Idle)
        {
            SendPlayerStopPursuit();
            agent.isStopped = true;
            characterMovement.UnAiming();
        }
        if (state == AIBehaivour.CirclePatrol)
        {
            SendPlayerStopPursuit();
            agent.isStopped = false;
            SetDestinationByPathNode(path.GetNextNode(ref patrolPathNodeIndex));
            characterMovement.UnAiming();
        }
        if (state == AIBehaivour.PatrolRandom)
        {
            SendPlayerStopPursuit();
            agent.isStopped = false;
            SetDestinationByPathNode(path.GetRandomNode());
            characterMovement.UnAiming();
        }
        if (state == AIBehaivour.PursuitTarget)
        {
            agent.isStopped = false;
            characterMovement.Aiming();
        }
        if (state == AIBehaivour.SeekTarget)
        {
            agent.isStopped = false;
            characterMovement.Aiming();
        }
        if (state == AIBehaivour.FindLostTarget)
        {
            agent.isStopped = false;
            characterMovement.Aiming();
        }
        aiBehaivour = state;
        BehaviourChanged?.Invoke(aiBehaivour);
    }
    private void SetDestinationByPathNode(PatrolPathNode pathNode)
    {
        currentPathNode = pathNode;
        agent.CalculatePath(currentPathNode.transform.position, navMeshPath);
        agent.SetPath(navMeshPath);
    }

    private void ActionAssignTargetToTeam(Transform other, AIBehaivour aiBehaivour)
    {
        List<Destructible> team = Destructible.GetAllTeamMembers(alienSoldier.TeamId);
        foreach (var mate in team)
        {
            AIAlienSoldier ai = mate.transform.root.GetComponent<AIAlienSoldier>();
            Debug.Log(mate.name);
            if (ai != null && ai.enabled == true)
            {
                ai.SetPursueTarget(other);
                ai.StartBehaivour(aiBehaivour);
            }
        }
    }

    private void ActionUpdateTarget()
    {
        potentialTarget = Player.Instance.gameObject;
        if (viewer.IsObjectVisible(potentialTarget))
        {
            if (findZone != null)
            {
                Destroy(findZone.gameObject);
                findZone = null;
            }
            SendPlayerStartPursuit();
            pursuitTarget = potentialTarget.transform;
            ActionAssignTargetToTeam(pursuitTarget, AIBehaivour.PursuitTarget);
        }
        else
        {
            if (pursuitTarget != null)
            {
                seekTarget = pursuitTarget.position;
                pursuitTarget = null;
                StartBehaivour(AIBehaivour.SeekTarget);
            }
        }
    }

    private bool AgentReachedDestination()
    {
        if (agent.pathPending == false)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.hasPath == false || agent.velocity.sqrMagnitude == 0.0f)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }
    private void SyncAgentWithCharacterMovement()
    {
        agent.speed = characterMovement.CurrentSpeed;
        float factor = agent.velocity.magnitude / agent.speed;
        characterMovement.targetDirectionControl = transform.InverseTransformDirection(agent.velocity.normalized) * factor;
    }

    private void OnGetDamage(Destructible other)
    {
        if (other == null) return;
        if (other.TeamId != alienSoldier.TeamId)
        {
            if (viewer.IsObjectVisible(other.gameObject)) ActionAssignTargetToTeam(other.transform, AIBehaivour.PursuitTarget);
            else ActionAssignTargetToTeam(other.transform, AIBehaivour.FindLostTarget);
        }
    }

    private void OnDeath()
    {
        SendPlayerStopPursuit();
    }

    private void FindPatrolPath()
    {
        if (path == null)
        {
            PatrolPath[] paths = FindObjectsOfType<PatrolPath>();
            float minDistance = float.MaxValue;

            foreach (var path in paths)
            {
                if (Vector3.Distance(path.transform.position, transform.position) < minDistance)
                {
                    this.path = path;
                }
            }
        }
    }

    private void ActionStartPursuit()
    {
        SendPlayerStartPursuit();
        pursuitTarget = potentialTarget.transform;
        ActionAssignTargetToTeam(pursuitTarget, AIBehaivour.PursuitTarget);
    }

    private void SendPlayerStartPursuit()
    {
        if (isTargetDetected == false)
        {
            Player.Instance.StartPursuet();
            isTargetDetected = true;
        }
    }
    private void SendPlayerStopPursuit()
    {
        if (isTargetDetected == true)
        {
            Player.Instance.StopPursuet();
            isTargetDetected = false;
        }
    }
}
