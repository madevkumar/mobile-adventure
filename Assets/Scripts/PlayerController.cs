using UnityEngine;

/// <summary>
/// PlayerController handles core player movement and platforming mechanics.
/// Integrates with TimeLoopManager for time manipulation.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;

    [Header("Ground Check")]
    [SerializeField] private float groundDragDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    private Rigidbody2D rb;
    private TimeLoopManager timeLoopManager;
    private bool isGrounded;
    private float horizontalInput;
    private bool jumpInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeLoopManager = FindObjectOfType<TimeLoopManager>();
    }

    private void Update()
    {
        HandleInput();
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        Move();
        ApplyDrag();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.Space);

        if (jumpInput && isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyDrag()
    {
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDragDistance, groundLayer);
        isGrounded = hit.collider != null;
    }

    public void ResetPosition(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero;
    }
}