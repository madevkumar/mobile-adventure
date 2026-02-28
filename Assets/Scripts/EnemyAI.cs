using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI with two behaviours:
///   • Patrol  – walks back and forth between two waypoints.
///   • Chase   – runs toward the player when they enter the detection radius.
///
/// The enemy fully participates in the time-loop rewind system: every physics
/// frame its transform position and AI state are recorded so that rewinding
/// restores the enemy to exactly where it was.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour, IRewindable
{
    // ------------------------------------------------------------------ //
    //  Inspector fields
    // ------------------------------------------------------------------ //

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed  = 4f;

    [Header("Patrol Waypoints")]
    [Tooltip("Left boundary of the patrol route (world X position).")]
    public Transform waypointLeft;
    [Tooltip("Right boundary of the patrol route (world X position).")]
    public Transform waypointRight;
    [Tooltip("How close the enemy must be to a waypoint before reversing direction.")]
    public float waypointArrivalThreshold = 0.15f;

    [Header("Detection")]
    [Tooltip("Tag used to identify the player GameObject.")]
    public string playerTag = "Player";
    public float detectionRadius = 5f;

    // ------------------------------------------------------------------ //
    //  Internal state
    // ------------------------------------------------------------------ //

    private enum AIState { Patrol, Chase }

    private AIState   _state          = AIState.Patrol;
    private Transform _player;
    private Rigidbody2D _rb;

    // Patrol direction: +1 = moving right, -1 = moving left.
    private float _patrolDirection = 1f;

    // ------------------------------------------------------------------ //
    //  Rewind history
    // ------------------------------------------------------------------ //

    private struct EnemySnapshot
    {
        public Vector2  Position;
        public Vector2  Velocity;
        public AIState  State;
        public float    PatrolDirection;
    }

    private readonly LinkedList<EnemySnapshot> _history = new LinkedList<EnemySnapshot>();

    // ------------------------------------------------------------------ //
    //  Unity lifecycle
    // ------------------------------------------------------------------ //

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Find the player by tag (non-allocating after the first call).
        var playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            _player = playerObj.transform;

        // Register with the time-loop controller if one exists in the scene.
        if (TimeLoopController.Instance != null)
            TimeLoopController.Instance.Register(this);
    }

    void OnDestroy()
    {
        if (TimeLoopController.Instance != null)
            TimeLoopController.Instance.Unregister(this);
    }

    void FixedUpdate()
    {
        // Skip AI logic while time is being rewound.
        if (TimeLoopController.Instance != null && TimeLoopController.Instance.IsRewinding)
            return;

        switch (_state)
        {
            case AIState.Patrol: UpdatePatrol(); break;
            case AIState.Chase:  UpdateChase();  break;
        }
    }

    // ------------------------------------------------------------------ //
    //  AI behaviours
    // ------------------------------------------------------------------ //

    private void UpdatePatrol()
    {
        // Check whether the player has entered detection range.
        if (_player != null)
        {
            float dist = Vector2.Distance(transform.position, _player.position);
            if (dist <= detectionRadius)
            {
                _state = AIState.Chase;
                return;
            }
        }

        // Walk toward the current target waypoint.
        Transform target = _patrolDirection > 0f ? waypointRight : waypointLeft;
        if (target == null)
        {
            // No waypoints set – stand still.
            _rb.velocity = new Vector2(0f, _rb.velocity.y);
            return;
        }

        _rb.velocity = new Vector2(_patrolDirection * patrolSpeed, _rb.velocity.y);

        // Flip direction when the waypoint is reached.
        float dx = target.position.x - transform.position.x;
        if (Mathf.Abs(dx) < waypointArrivalThreshold)
            _patrolDirection = -_patrolDirection;
    }

    private void UpdateChase()
    {
        if (_player == null)
        {
            _state = AIState.Patrol;
            return;
        }

        float dist = Vector2.Distance(transform.position, _player.position);

        // Return to patrol when the player is far enough away.
        if (dist > detectionRadius * 1.5f)
        {
            _state = AIState.Patrol;
            return;
        }

        float dir = Mathf.Sign(_player.position.x - transform.position.x);
        _rb.velocity = new Vector2(dir * chaseSpeed, _rb.velocity.y);
    }

    // ------------------------------------------------------------------ //
    //  IRewindable implementation
    // ------------------------------------------------------------------ //

    public void RecordState(int maxFrames)
    {
        _history.AddLast(new EnemySnapshot
        {
            Position       = _rb.position,
            Velocity       = _rb.velocity,
            State          = _state,
            PatrolDirection = _patrolDirection,
        });

        // Keep the buffer bounded.
        while (_history.Count > maxFrames)
            _history.RemoveFirst();
    }

    public void RewindState()
    {
        if (_history.Count == 0)
            return;

        EnemySnapshot snap = _history.Last.Value;
        _history.RemoveLast();

        _rb.position   = snap.Position;
        _rb.velocity   = snap.Velocity;
        _state         = snap.State;
        _patrolDirection = snap.PatrolDirection;
    }

    // ------------------------------------------------------------------ //
    //  Editor helpers
    // ------------------------------------------------------------------ //

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Detection radius.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Chase-exit radius.
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius * 1.5f);

        // Patrol path.
        if (waypointLeft != null && waypointRight != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(waypointLeft.position, waypointRight.position);
            Gizmos.DrawSphere(waypointLeft.position,  0.15f);
            Gizmos.DrawSphere(waypointRight.position, 0.15f);
        }
    }
#endif
}
