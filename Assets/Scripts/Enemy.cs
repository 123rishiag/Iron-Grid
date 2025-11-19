using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    public int healthPoints = 4;

    [Header("Movement")]
    [SerializeField] private float turnSpeed = 10f;

    private NavMeshAgent agent;

    private Transform[] waypoints;
    private int waypointIndex;

    private float totalDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        // So that faster enemy always have more priority than slow
        // and goes around it without disturbing it
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    private void Start()
    {
        waypoints = FindFirstObjectByType<WaypointManager>().GetWaypoints();
        waypointIndex = 0;

        CollectTotalDistance();
    }

    private void Update()
    {
        FaceTarget(agent.steeringTarget);

        // Check if the agent is close to the current target point
        if (agent.remainingDistance < .5f)
        {
            // Set the destination to next waypoint
            agent.SetDestination(GetNextWaypoint());
        }
    }

    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;

    private void CollectTotalDistance()
    {
        for (int i = 0; i < waypoints.Length - 1; ++i)
        {
            float distance = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance = totalDistance + distance;
        }
    }

    private void FaceTarget(Vector3 _newTarget)
    {
        // Calculate the direction from current position to the new target
        Vector3 directionToTarget = _newTarget - transform.position;
        if (directionToTarget.magnitude == 0)
        {
            return;
        }

        directionToTarget.y = 0; // Ignore any difference in the vertical position // Removes vertical component

        // Create a rotation that points the forward vector up the calculated direction
        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate from the current rotation to the target rotation at the defined speed // Time.deltaTime makes it frame rate independent
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWaypoint()
    {
        // Check if the waypoint index is beyond the last waypoint
        if (waypointIndex >= waypoints.Length)
        {
            // If true, return the agent's current position, effectively stopping it
            // Uncomment the line below to loop the waypoints
            // waypointIndex = 0;
            return transform.position;
        }

        // Get the current target point from the waypoints array
        Vector3 targetPoint = waypoints[waypointIndex].position;

        // If this is not the first waypoint, calculate the distance from the previous waypoint
        if(waypointIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex - 1].position);
            // Subtract this distance from the total distance
            totalDistance = totalDistance - distance;
        }

        ++waypointIndex;

        return targetPoint;
    }

    public void TakeDamage(int damage)
    {
        healthPoints = healthPoints - damage;

        if(healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
