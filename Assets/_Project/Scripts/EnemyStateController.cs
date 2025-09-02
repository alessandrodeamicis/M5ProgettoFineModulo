using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] private bool _isSentry;
    [SerializeField] private Transform[] _patrolPoints;

    private EnemyVision vision;
    private EnemyState currentState;
    private int patrolIndex = 0;
    private float waitTimer = 0f;
    private int sentryIndex = 0;
    private Quaternion sentryTargetRotation;
    private float sentryTimer = 0f;
    private NavMeshAgent agent;
    private EnemyState defaultState;
    private float[] sentryAngles = { 0, 90, 180, 270 };
    private Vector3 initialPosition;
    private Animator animator;

    public enum EnemyState { Patrol, Sentry, Chase }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<EnemyVision>();
        animator = GetComponent<Animator>();

        if (_isSentry)
        {
            currentState = EnemyState.Sentry;
            defaultState = EnemyState.Sentry;
            sentryTargetRotation = Quaternion.Euler(0, sentryAngles[0], 0);
            initialPosition = transform.position;
        }
        else
        {
            currentState = EnemyState.Patrol;
            defaultState = EnemyState.Patrol;
            agent.destination = _patrolPoints[0].position;
            animator.SetTrigger("move");
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
        if (vision.CanSeePlayer())
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= 2f)
            {
                patrolIndex = (patrolIndex + 1) % _patrolPoints.Length;
                agent.destination = _patrolPoints[patrolIndex].position;
                waitTimer = 0f;
            }
        }
    }

    void Sentry()
    {
        if (vision.CanSeePlayer())
        {
            currentState = EnemyState.Chase;
            animator.SetTrigger("move");
            return;
        }

        sentryTimer += Time.deltaTime;
        if (sentryTimer >= 3f)
        {
            sentryIndex = (sentryIndex + 1) % sentryAngles.Length;
            sentryTargetRotation = Quaternion.Euler(0, sentryAngles[sentryIndex], 0);
            sentryTimer = 0f;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, sentryTargetRotation, 120f * Time.deltaTime);
    }

    void Chasing()
    {
        if (vision.CanSeePlayer())
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
                agent.destination = _patrolPoints[patrolIndex].position;
            }
            else
            {
                agent.destination = initialPosition;
            }
        }
    }
}
