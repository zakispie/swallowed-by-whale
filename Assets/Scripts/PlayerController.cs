using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class for handling player inputs & applying physics to the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region variables
    // Keyboard Property
    // public static Keyboard Keyboard => _instance._keyboard;
    
    // Mouse Property
    public static Mouse Mouse => _instance._mouse;
    
    // Most Recent Facing Direction Property
    public static Vector3 FacingDirection => _instance._facingDirection;
    
    // Position Property
    public static Vector3 Position => _instance.transform.position;
    
    // HealthBar Component Property
    public static HealthBar HealthBar => _instance._healthBar;

    // Player's height
    private float _playerHeight;
    
    [Header("Ground Movement")]
    [Tooltip("Movement Strength")] 
    [SerializeField] private float moveStrength;
    
    [Tooltip("Maximum Velocity")]
    [SerializeField] private float maxVelocity;

    [Tooltip("Deceleration Strength")] 
    [SerializeField] private float deceleration;

    [Header("Air Movement")]
    [Tooltip("Deceleration in-air Strength (percentage of base deceleration)")] [Range(0f, 1f)] 
    [SerializeField] private float inAirDecelerationPercent;

    [Tooltip("Number of Jumps (Including First Jump)")] [Min(1)] 
    [SerializeField] private int numJumps;

    [Tooltip("Jump Strength")]
    [SerializeField] private float jumpStrength;

    [Tooltip("Climb Speed")]
    [SerializeField] private float climbSpeed;

    [Tooltip("How far down crouch goes")]
    [Range(1f, 5f)]
    [SerializeField] private float crouchingHeight;

    [Tooltip("Added force when falling to get rid of float")]
    [SerializeField] private float fallForce;

    [Header("Other")]
    [Tooltip("Ground Layer")]
    [SerializeField] private LayerMask groundLayer;

    // Singleton instance of the player controller
    private static PlayerController _instance;
    
    // Tracks the player's most recent facing direction (face right by default)
    private Vector3 _facingDirection = Vector3.right;

    // Tracks the player's current movement direction
    private Vector3 _movementDirection;

    // Tracks whether the player is currently grounded
    private bool _isGrounded;

    // Tracks whether the player is currently crouching
    private bool _isCrouched = false;

    // Trakc whether player is currently on a ladder
    public static bool _onLadder = false;

    // Tracks num jumps player has performed
    private int _jumpCount;
    
    // Cache the healthBar component
    private HealthBar _healthBar;

    // Cache the capsule colliders
    private CapsuleCollider[] _capsuleColliders;

    // Cache the rigidbody
    private Rigidbody _rigidbody;



    // Cache the keyboard
    private Keyboard _keyboard; 
    
    // Cache the mouse
    private Mouse _mouse;

    #endregion

    /// <summary>
    /// Assign components
    /// </summary>
    void Start()
    {
        _instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleColliders = GetComponents<CapsuleCollider>();
        _healthBar = GetComponent<HealthBar>();
        _keyboard = Keyboard.current;
        _mouse = Mouse.current;
        _playerHeight = transform.localScale.y;
    }

    /// <summary>
    /// Determine necessary movement from keypresses
    /// </summary>
    void Update()
    {

        // When on ladder you can not jump or crouch, rest of movement behavior is same
        if(_onLadder)
        {
    
            _rigidbody.useGravity = false;

            if (_keyboard.wKey.isPressed) // move up on ladder
            {
                transform.position += Vector3.up * climbSpeed * Time.deltaTime;
            } else if(_keyboard.sKey.isPressed && !_isGrounded) { // move down on ladder if player is not on ground
                transform.position += Vector3.down * climbSpeed * Time.deltaTime;
            }

        } else {

            _rigidbody.useGravity = true;
            
            // Allow jump if player is grounded or has not performed double jump
            if ((_keyboard.wKey.wasPressedThisFrame || _keyboard.spaceKey.wasPressedThisFrame )&& (_isGrounded || _jumpCount < numJumps - 1))
            {
                _rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                _facingDirection = Vector3.up;
                _jumpCount++;
            }

            // Implements crouching
            if(_keyboard.sKey.isPressed)
            {
                if(!_isCrouched)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
                    _isCrouched = true;
                }
                transform.localScale = new Vector3(1f, _playerHeight / crouchingHeight, 1f);
            }
            else
            {
                _isCrouched = false;
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        // Handle Inputs
        if (_keyboard.aKey.isPressed)
        {
            _movementDirection = Vector3.left;
            _facingDirection = Vector3.left;
        }
        else if (_keyboard.dKey.isPressed)
        {
            _movementDirection = Vector3.right;
            _facingDirection = Vector3.right;
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

        if(_facingDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        } 
        else if(_facingDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Check if player is grounded
        _isGrounded = IsGrounded();
        if (_isGrounded)
        {
            _jumpCount = 0;
        }

        if(IsFalling()){
            // After jumps --> on descent, add force to fall faster
            _rigidbody.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);

        }

        // Initialize movement vector and apply to the rigidbody
        Vector3 movement = _movementDirection * (moveStrength * Time.fixedDeltaTime);
        
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
            // After applying force, ensure the rigidbody velocity is below maxVelocity
            _rigidbody.velocity = new Vector3(
                Mathf.Clamp(_rigidbody.velocity.x, -maxVelocity, maxVelocity),
                _rigidbody.velocity.y,
                Mathf.Clamp(_rigidbody.velocity.z, -maxVelocity, maxVelocity));
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

    /// <summary>
    /// Determines if the player is falling
    /// </summary>
    /// <returns> True if the player is falling, false otherwise </returns>
    bool IsFalling()
    {
        return _rigidbody.velocity.y < 0;
    }
}