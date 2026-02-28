using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IRewindable {
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    private bool isGrounded;
    public LayerMask groundLayer;
    private Rigidbody2D rb;

    private struct PlayerSnapshot {
        public Vector2 Position;
        public Vector2 Velocity;
    }

    private readonly LinkedList<PlayerSnapshot> _history = new LinkedList<PlayerSnapshot>();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (TimeLoopController.Instance != null)
            TimeLoopController.Instance.Register(this);
    }

    void OnDestroy() {
        if (TimeLoopController.Instance != null)
            TimeLoopController.Instance.Unregister(this);
    }

    void Update() {
        // Block player input while time is rewinding.
        if (TimeLoopController.Instance != null && TimeLoopController.Instance.IsRewinding)
            return;
        Move();
        Jump();
    }

    private void Move() {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);
    }

    private void Jump() {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void RecordState(int maxFrames) {
        _history.AddLast(new PlayerSnapshot {
            Position = rb.position,
            Velocity = rb.velocity,
        });
        while (_history.Count > maxFrames)
            _history.RemoveFirst();
    }

    public void RewindState() {
        if (_history.Count == 0)
            return;
        PlayerSnapshot snap = _history.Last.Value;
        _history.RemoveLast();
        rb.position = snap.Position;
        rb.velocity = snap.Velocity;
    }
}