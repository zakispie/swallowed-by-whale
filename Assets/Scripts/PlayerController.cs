using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class for handling player inputs & applying physics to the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region variables
    [Header("Ground Movement")]
    [Tooltip("Move Speed")] 
    [SerializeField] private float moveSpeed;

    [Tooltip("Deceleration Strength")] 
    [SerializeField] private float deceleration;

    [Header("Air Movement")]
    [Tooltip("Deceleration in-air Strength (percentage of base deceleration)")] [Range(0f, 1f)] 
    [SerializeField] private float inAirDecelerationPercent;

    [Tooltip("Number of Jumps (Including First Jump)")] [Min(1)] 
    [SerializeField] private int numJumps;

    [Tooltip("Jump Strength")] 
    [SerializeField] private float jumpStrength;

    [Header("Other")]
    [Tooltip("Ground Layer")]
    [SerializeField] private LayerMask groundLayer;

    // Tracks the player's current movement direction
    private Vector3 _movementDirection;

    // Tracks whether the player is currently grounded
    private bool _isGrounded;

    // Tracks num jumps player has performed
    private int _jumpCount;

    // Cache the capsule colliders
    private CapsuleCollider[] _capsuleColliders;

    // Cache the rigidbody
    private Rigidbody _rigidbody;

    // Cache the keyboard
    private Keyboard _keyboard;

    #endregion

    /// <summary>
    /// Assign components
    /// </summary>
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleColliders = GetComponents<CapsuleCollider>();
        _keyboard = Keyboard.current;
    }

    /// <summary>
    /// Determine necessary movement from keypresses
    /// </summary>
    void Update()
    {
        // Allow jump if player is grounded or has not performed double jump
        if (_keyboard.wKey.wasPressedThisFrame && (_isGrounded || _jumpCount < numJumps - 1))
        {
            _rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            _jumpCount++;
        }

        // Handle Inputs
        if (_keyboard.aKey.isPressed)
        {
            _movementDirection = Vector3.left;
        }
        else if (_keyboard.dKey.isPressed)
        {
            _movementDirection = Vector3.right;
        }
        else
        {
            _movementDirection = Vector3.zero;
        }
    }

    /// <summary>
    /// Update the physics of the rigidbody based on current movement direction
    /// </summary>
    void FixedUpdate()
    {
        // Check if player is grounded
        _isGrounded = IsGrounded();
        if (_isGrounded)
        {
            _jumpCount = 0;
        }

        // Initialize movement vector and apply to the rigidbody
        Vector3 movement = _movementDirection * (moveSpeed * Time.fixedDeltaTime);
        
        if (movement == Vector3.zero) 
        {
            // No movement input, we should decelerate
            Vector3 curVelocity = _rigidbody.velocity;
            // Apply the deceleration force (on the x-axis only) via a Lerp
            if (_isGrounded)
            {
                _rigidbody.velocity =
                    Vector3.Lerp(curVelocity, new Vector3(0, curVelocity.y, curVelocity.z),
                        deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _rigidbody.velocity =
                    Vector3.Lerp(curVelocity, new Vector3(0, curVelocity.y, curVelocity.z),
                        (deceleration * inAirDecelerationPercent) * Time.fixedDeltaTime);
            }
        }
        else
        {
            // Movement input requested, apply movement force in that direction
            _rigidbody.AddForce(movement, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Determines if the player is currently grounded
    /// </summary>
    /// <returns> True if the player is grounded, false otherwise </returns>
    bool IsGrounded()
    {
        // The position, height, and radius of the capsule's collider.
        Vector3 capsulePosition = _capsuleColliders[0].transform.position;
        float capsuleHeight = _capsuleColliders[0].height;
        float capsuleRadius = _capsuleColliders[0].radius;

        // Check for overlapping colliders below the capsule to determine if it's grounded.
        Collider[] hitColliders = Physics.OverlapCapsule(
            capsulePosition + new Vector3(0, capsuleHeight / 2 - capsuleRadius, 0),
            capsulePosition - new Vector3(0, capsuleHeight / 2 - capsuleRadius, 0),
            capsuleRadius, groundLayer);

        return hitColliders.Length > 0;
    }
}