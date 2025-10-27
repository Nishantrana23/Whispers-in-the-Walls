using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public float idleTime = 2f;
    public float walkSpeed = 2f;
    public float chaseSpeed = 4f;
    public float sightDistance = 10f;
    public AudioClip idleSound;
    public AudioClip walkingSound;
    public AudioClip chasingSound;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private float idleTimer = 0f;
    private Transform player;
    private AudioSource audioSource;

    private enum EnemyState { Idle, Walk, Chase }
    private EnemyState currentState = EnemyState.Idle;

    private bool isChasingAnimation = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Make sure it has the 'Player' tag.");
        }

        if (waypoints != null && waypoints.Length > 0)
        {
            if (agent.isOnNavMesh)
                SetDestinationToWaypoint();
            else
                Debug.LogError("NavMeshAgent is not on a NavMesh! Bake the NavMesh and place the agent correctly.");
        }
        else
        {
            Debug.LogError("Waypoints not assigned on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (player == null || agent == null || animator == null || !agent.isOnNavMesh) return;

        switch (currentState)
        {
            case EnemyState.Idle:
                idleTimer += Time.deltaTime;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false);
                PlaySound(idleSound);

                if (idleTimer >= idleTime)
                {
                    NextWaypoint();
                }

                CheckForPlayerDetection();
                break;

            case EnemyState.Walk:
                idleTimer = 0f;
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsChasing", false);
                PlaySound(walkingSound);

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = EnemyState.Idle;
                }

                CheckForPlayerDetection();
                break;

            case EnemyState.Chase:
                idleTimer = 0f;
                agent.speed = chaseSpeed;
                if (player != null)
                {
                    agent.SetDestination(player.position);
                }
                isChasingAnimation = true;
                animator.SetBool("IsChasing", true);
                PlaySound(chasingSound);

                if (Vector3.Distance(transform.position, player.position) > sightDistance)
                {
                    currentState = EnemyState.Walk;
                    agent.speed = walkSpeed;
                    SetDestinationToWaypoint(); // Resume waypoint patrol
                }
                break;
        }
    }

    private void CheckForPlayerDetection()
    {
        if (player == null) return;

        RaycastHit hit;
        Vector3 playerDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                currentState = EnemyState.Chase;
                Debug.Log("Player detected!");
            }
        }
    }

    private void PlaySound(AudioClip soundClip)
    {
        if (audioSource == null || soundClip == null) return;

        if (!audioSource.isPlaying || audioSource.clip != soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    private void NextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0 || !agent.isOnNavMesh) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        SetDestinationToWaypoint();
    }

    private void SetDestinationToWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0 || agent == null || !agent.isOnNavMesh) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint != null)
        {
            agent.speed = walkSpeed;
            agent.SetDestination(targetWaypoint.position);
            currentState = EnemyState.Walk;
            animator.enabled = true;
        }
        else
        {
            Debug.LogWarning("Null waypoint found at index " + currentWaypointIndex);
        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = currentState == EnemyState.Chase ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
