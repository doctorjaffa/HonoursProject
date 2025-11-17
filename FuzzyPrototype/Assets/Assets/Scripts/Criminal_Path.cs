using UnityEngine;

public class Criminal_Path : MonoBehaviour
{
    // List of waypoint nodes to follow
    [SerializeField]
    private Transform[] waypoints;

    private float speed = 3f;
    // Index of current node the agent is on
    private int current_index = 0;
    // How close to the node before moving onto the next 
    private float reach_threshold = 0.02f;

    private Rigidbody2D body;

    private void Start()
    {
       body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the list is empty, early return
        if (waypoints.Length == 0) return;

        // Move towards the current waypoint node 
        Transform target = waypoints[current_index];
        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;
        
        // Rotate to face the waypoint node 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // If the agent is closer than the threshold
        if (direction.magnitude < reach_threshold)
        {
            // Move to next waypoint 
            current_index++;
            if (current_index >= waypoints.Length)
            {
                // Reached end of path, commit the crime 
                CommitCrime();
            }
        }
    }

    void CommitCrime()
    {
        // Broadcast the event (will be picked up by NPCs) 
        EventManager.TriggerCrimeResponse();
        // Stop moving 
        enabled = false;
    }
}
