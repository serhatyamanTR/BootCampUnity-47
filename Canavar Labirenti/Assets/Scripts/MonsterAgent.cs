using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Unity.AI.Navigation;

public class MonsterAgent : Agent
{
    [SerializeField] private Transform northExit;
    [SerializeField] private Transform southExit;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private GameObject navMeshSurfaceHolder;
    private NavMeshSurface _navMeshSurface;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform _currentExit;

    public bool isTurnPlayer1;
    public bool isTurnPlayer2;
    private Vector3 lastPosition;
    private float totalDistanceMoved = 0f;
    private bool firstMoveCompleted = false;

    private Animator animator;
    public int health = 1; // Canavara can ekle

    private void Start()
    {
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshSurface = navMeshSurfaceHolder.GetComponent<NavMeshSurface>();
        animator = GetComponentInChildren<Animator>();

        // İki çıkışı da açık olarak ayarla
        northExit.gameObject.SetActive(true);
        southExit.gameObject.SetActive(true);

        // Daha kısa mesafeli çıkışı belirle
        float distanceToNorthExit = Vector3.Distance(transform.position, northExit.position);
        float distanceToSouthExit = Vector3.Distance(transform.position, southExit.position);
        _currentExit = distanceToNorthExit < distanceToSouthExit ? northExit : southExit;

        // NavMesh'i başlangıçta bir kez bake et
        _navMeshSurface.BuildNavMesh();

        // İlk hareketi otomatik olarak başlat
        lastPosition = transform.position;
        firstMoveCompleted = false;

        // Canavarları belirli bir layer'a ata ve çarpışmalarını ignore et
        gameObject.layer = LayerMask.NameToLayer("Monster");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Monster"), LayerMask.NameToLayer("Monster"));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Hedefe olan yön ve mesafe
        Vector3 directionToTarget = (_currentExit.position - transform.position).normalized;
        sensor.AddObservation(directionToTarget);
        sensor.AddObservation(Vector3.Distance(_currentExit.position, transform.position));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!isTurnPlayer1 && !isTurnPlayer2)
        {
            // Hedefe doğru hareket et
            _navMeshAgent.destination = _currentExit.position;

            // Dönüş eylemini uygula (isteğe bağlı)
            float rotate = actions.ContinuousActions[2];
            transform.Rotate(Vector3.up, rotate * rotationSpeed * Time.deltaTime);

            // Hareket edilen mesafeyi hesapla
            totalDistanceMoved += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;

            if (totalDistanceMoved >= 5f)
            {
                _navMeshAgent.speed = 0;
                isTurnPlayer1 = true;
                totalDistanceMoved = 0f;
            }
            else
            {
                _navMeshAgent.speed = 1;
            }
        }
    }

    private void Update()
    {
        if (!firstMoveCompleted)
        {
            _navMeshAgent.speed = 1;
            totalDistanceMoved += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;

            if (totalDistanceMoved >= 5f)
            {
                _navMeshAgent.speed = 0;
                firstMoveCompleted = true;
                isTurnPlayer1 = true;
                totalDistanceMoved = 0f;
                animator.SetFloat("speed", 0); // idle animasyonuna geçiş için hız değerini sıfırla
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Manuel kontrol (isteğe bağlı)
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Monster is dead.");
            // Ölme animasyonu veya başka işlemler gerekirse ekleyebilirsiniz
        }
    }

    public void ResetAgent()
    {
        health = 1;
        animator.SetFloat("speed", 0); // idle animasyonuna geçiş için hız değerini sıfırla
    }
}
