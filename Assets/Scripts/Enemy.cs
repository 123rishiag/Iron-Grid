using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        waypointIndex = 0;
    }

    private void Update()
    {
        // Check if the agent is close to the current target point
        if (agent.remainingDistance < .5f)
        {
            // Set the destination to next waypoint
            agent.SetDestination(GetNextWaypoint());
        }
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
