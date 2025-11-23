using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.VisualScripting;

public class NPC_BinaryBehaviour : MonoBehaviour
{
    // Boolean to set whether receiving fuzzy reaction state or set random binary state
    public bool is_fuzzy = true;

    // Store rigid body component for movement
    private Rigidbody2D body;

    // Editable speed value 
    [SerializeField] private float speed = 3.0f;

    // Store the door's transform for fleeing behaviour
    [SerializeField] private Transform door;
    // Store the criminal's transform for fighting behaviour 
    [SerializeField] private Transform criminal;

    // Different states for the NPC - NONE = Idle 
    public enum ReactionState { COWER, FLEE, FIGHT, NONE };

    // How this NPC should respond - editable in editor 
    [SerializeField] private ReactionState reaction_state = ReactionState.NONE;
    private ReactionState current_state;

    // Target position for NPC to move towards 
    private Vector2 target_pos;

    // Pathfinding agent to move towards target
    [SerializeField] private NavMeshAgent agent;

    private void OnEnable()
    {
        if (!is_fuzzy)
        {
            EventManager.OnCommitCrime += RandomReaction;
        }
    }

    private void OnDisable()
    {
        if (!is_fuzzy)
        {
            EventManager.OnCommitCrime -= RandomReaction;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        // If the door and criminal have not been set in the editor, find them in the scene by their tags (if they exist)
        if (door == null) door = GameObject.FindWithTag("Door")?.transform;
        if (criminal == null) criminal = GameObject.FindWithTag("Criminal")?.transform;

        // Set default state to NONE
        current_state = ReactionState.NONE;

        // Get the navmesh agent component and ensure it stays within 2d space and does not rotate
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void SetReactionState(ReactionState state)
    {
        if (is_fuzzy) reaction_state = state;
        else
        {   // If it's a binary simulation, set a random state that isn't None 
             do { reaction_state = (ReactionState)Random.Range(0, System.Enum.GetValues(typeof(ReactionState)).Length); } while (reaction_state == ReactionState.NONE); 
        }
    }

    public void RandomReaction()
    {
        reaction_state = (ReactionState)Random.Range(0, System.Enum.GetValues(typeof(ReactionState)).Length);
    }

    public void RespondToSignal() => ChangeState();

    // Set target position based on reaction state 
    private void ChangeState()
    {
        // If the current state is the same, early return
        if (current_state == reaction_state) return;
        current_state = reaction_state;

        switch (reaction_state)
        {
            // If this NPC is a fleeing one, set its target position to the door position
            case ReactionState.FLEE:
                target_pos = door.transform.position;
                break;
            // If this NPC is a fighting one, set its target position to the criminal position
            case ReactionState.FIGHT:
                target_pos = criminal.transform.position;
                break;
        }
    }

    // FixedUpdate is called once per physics tick
    private void FixedUpdate()
    {
        // Switch to control which behaviour function to run 
        switch (current_state)
        {
            case ReactionState.NONE:
                break;
            case ReactionState.COWER:
                CowerResponse();
                break;
            case ReactionState.FLEE:
                MoveTowardTarget(target_pos);
                break;
            case ReactionState.FIGHT:
                MoveTowardTarget(criminal.position);
                break;
        }
    }

    // Shake on the spot to imitate shaking in fear 
    void CowerResponse()
    {
        Vector2 cower_shake = new Vector2(Mathf.Sin(Time.time * speed) * 0.1f, 0);
        body.MovePosition(body.position + cower_shake * Time.deltaTime);
    }

    // Move towards target in the scene (door or criminal)
    private void MoveTowardTarget(Vector2 target)
    {
        agent.SetDestination(target);
    }

    // Handle NPC collisions 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If a fleeing NPC collides with the door, destroy the NPC 
        if (current_state == ReactionState.FLEE && collision.gameObject.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
        // Otherwise, if a fighting NPC collides with the player, reload the scene ("kill" the criminal) 
        else if (current_state == ReactionState.FIGHT && collision.gameObject.CompareTag("Criminal"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Fighting criminal");
        }
    }
}
