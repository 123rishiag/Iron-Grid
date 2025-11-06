using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;

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
        waypointIndex = 0;
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
        if (waypointIndex >= waypoints.Length)
        {
            return transform.position;
        }

        Vector3 targetPoint = waypoints[waypointIndex].position;
        ++waypointIndex;

        return targetPoint;
    }
}
