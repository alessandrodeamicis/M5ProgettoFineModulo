using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] private bool _isSentry;
    public enum EnemyState { Patrol, Sentry, Chase }
    private EnemyState currentState;

    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;
    public float rotationSpeed = 120f;
    public float timeBetweenRotations = 3f;
    public float[] sentryAngles;

    private int patrolIndex = 0;
    private float waitTimer = 0f;
    private int sentryIndex = 0;
    private float sentryTimer = 0f;
    private Quaternion sentryTargetRot;

    private NavMeshAgent agent;
    private EnemyVision vision;
    private EnemyState defaultState;
    private Vector3 _initialPosition;
    private Animator _animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<EnemyVision>();
        _animator = GetComponent<Animator>();

        if (_isSentry)
        {
            currentState = EnemyState.Sentry;
            defaultState = EnemyState.Sentry;
            sentryTargetRot = Quaternion.Euler(0, sentryAngles[0], 0);
            _initialPosition = transform.position;
        }
        else
        {
            currentState = EnemyState.Patrol;
            defaultState = EnemyState.Patrol;
            agent.destination = patrolPoints[0].position;
            _animator.SetTrigger("move");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrolling();
                break;

            case EnemyState.Sentry:
                Sentry();
                break;

            case EnemyState.Chase:
                Chasing();
                break;
        }
    }

    void Patrolling()
    {
        if (vision.CanSeeTarget())
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                agent.destination = patrolPoints[patrolIndex].position;
                waitTimer = 0f;
            }
        }
    }

    void Sentry()
    {
        if (vision.CanSeeTarget())
        {
            currentState = EnemyState.Chase;
            _animator.SetTrigger("move");
            return;
        }

        sentryTimer += Time.deltaTime;
        if (sentryTimer >= timeBetweenRotations)
        {
            sentryIndex = (sentryIndex + 1) % sentryAngles.Length;
            sentryTargetRot = Quaternion.Euler(0, sentryAngles[sentryIndex], 0);
            sentryTimer = 0f;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, sentryTargetRot, rotationSpeed * Time.deltaTime);
    }

    void Chasing()
    {
        if (vision.CanSeeTarget())
        {
            agent.isStopped = false;
            agent.SetDestination(vision.target.position);
        }
        else
        {
            currentState = defaultState;
            agent.isStopped = false;

            if (defaultState == EnemyState.Patrol)
            {
                agent.destination = patrolPoints[patrolIndex].position;
            } else
            {
                agent.destination = _initialPosition;
            }
        }
    }
}
