using UnityEngine;
using UnityEngine.AI;

public class CapsuleNavMeshMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public float repathDistance = 0.5f;
    public float randomRadius = 5f; // randomRadius burada tanımlandı

    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMesh Agent bileşeni bulunamadı!");
        }
        lastPosition = transform.position; // Başlangıç pozisyonunu kaydet
    }

    public float raycastDistance = 1f; // Işın mesafesi

    private void FindRandomPath()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomRadius;
        randomDirection += target.position; // Hedefe doğru yönelme ekleniyor
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);

            // Takılma kontrolü
            if (Vector3.Distance(transform.position, lastPosition) < repathDistance)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= 1f)
                {
                    // Rastgele yeni yol ara
                    FindRandomPath();
                    stuckTimer = 0f;
                }
            }
            else
            {
                stuckTimer = 0f;
                lastPosition = transform.position;
            }
            // Plane üzerinde kalmayı sağla
            Vector3 pos = transform.position;
            pos.y = 1;
            transform.position = pos;
        }

        
    }
}
