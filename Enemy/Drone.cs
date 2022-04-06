using UnityEngine;

public class Drone : Destructible
{
    [SerializeField] private Transform mesh;
    [SerializeField] private Weapon[] droneTurrets;
    [SerializeField] private GameObject[] meshComponents;
    [SerializeField] private Renderer[] meshRenderers;
    [SerializeField] private Material[] deadMaterials;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationLerpFactor;
    [SerializeField] private float hoverAmplitude;
    [SerializeField] private float hoverSpeed;

    private AIDrone aiDrone;
    private LayerMask m_LayerMask;
    public LayerMask LayerMask => m_LayerMask;

    public Transform Mesh => mesh;

    protected override void Start()
    {
        base.Start();
        aiDrone = GetComponent<AIDrone>();
        m_LayerMask = LayerMask.GetMask("Destructible");
    }

    private void Update()
    {
        mesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime);
    }
    protected override void OnDeath()
    {
        Deactivate();
    }

    public void LookAt(Vector3 target)
    {
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), Time.deltaTime * rotationLerpFactor);
        transform.rotation = rotation;
    }

    public void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
    }

    public void Attack(Vector3 target)
    {
        foreach (var turret in droneTurrets)
        {
            turret.FirePointLookAt(target);
            turret.Fire();
        }
    }

    public void Deactivate()
    {
        EventOnDeath?.Invoke();
        enabled = false;
        for (int i = 0; i < meshComponents.Length; i++)
        {
            if (meshComponents[i].GetComponent<Rigidbody>() == null)
            {
                meshComponents[i].AddComponent<Rigidbody>();
            }
        }

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = deadMaterials[i];
        }
    }

    [System.Serializable]
    public class AIDroneState
    {
        public Vector3 Position;
        public int HitPoints;
        public int MovementAreaId;

        public AIDroneState(Vector3 position, int hitPoints, int movementAreaId)
        {
            Position = position;
            HitPoints = hitPoints;
            MovementAreaId = movementAreaId;
        }
    }


    public override string SerializeState()
    {
        AIDroneState state = new AIDroneState(transform.position, CurrentHitPoints, aiDrone.MovementArea.Id);
        return JsonUtility.ToJson(state);
    }

    public override void DeserializeState(string state)
    {
        AIDroneState savedState = JsonUtility.FromJson<AIDroneState>(state);
        transform.position = savedState.Position;
        m_CurrentHitPoints = savedState.HitPoints;
        aiDrone.SetMovementAreaFromId(savedState.MovementAreaId);
    }

}
