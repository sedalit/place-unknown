using UnityEngine;

public class Projectile : Entity
{
    [SerializeField] protected float Velocity;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected int damage;

    [SerializeField] private ImpactEffect impactEffetPrefab;

    protected float timer;

    private void Update() {
        float stepLength = Time.deltaTime * Velocity;
        Vector3 step = transform.forward * stepLength;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, stepLength))
        {
            if (hit.collider.isTrigger != true)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                if (dest != null & dest != m_Parent)
                {
                    dest.ApplyDamage(damage, m_Parent);
                }
                OnProjectileLifeEnd(hit.collider, hit.point, hit.normal);
            }
        }

        timer += Time.deltaTime;
        if(timer > lifeTime)
        {
            Destroy(gameObject);
        }

        transform.position += new Vector3(step.x, step.y, step.z);
    }
    private void OnProjectileLifeEnd(Collider col, Vector3 pos, Vector3 normal)
    {
        if (col is CharacterController) return;
        if (impactEffetPrefab != null)
        {
            ImpactEffect impact = Instantiate(impactEffetPrefab, pos, Quaternion.LookRotation(normal), col.transform);
        }
        Destroy(gameObject);
    }

    private Destructible m_Parent;

    public void SetParentShooter(Destructible parent)
    {
        m_Parent = parent;
    }
}


