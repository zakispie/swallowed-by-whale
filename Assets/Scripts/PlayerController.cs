using UnityEngine;

/// <summary>
/// Class for handling player inputs & applying physics to the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region variables
    [Tooltip("Move Speed")]
    [SerializeField] private float _moveSpeed;
    
    [Tooltip("Jump Strength")]
    [SerializeField] private float _jumpStrength;

    public LayerMask groundLayer;

    // Tracks the player's current movement direction
    private Vector3 _movementDirection;
    
    // Tracks whether the player is currently grounded
    private bool _isGrounded;

    // Tracks num jumps player has performed
    private int _jumpCount;
    
    // Cache the transform
    private Transform _transform;
    
    // Cache the rigidbody
    private Rigidbody _rigidbody;
    #endregion

    /// <summary>
    /// Assign components
    /// </summary>
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Determine necessary movement from keypresses
    /// </summary>
    void Update()
    {

        Debug.Log("jump count: " + _jumpCount);
        Debug.Log("Is on ground: " + _isGrounded);

        // Implements only allowing double jump
        // Checks if player is grounded
        _isGrounded = IsGrounded();
        if(_isGrounded){
            _jumpCount = 0;
        }

        // TODO refactor to use new InputSystem
        if (Input.GetKeyDown(KeyCode.W) && (_isGrounded || _jumpCount < 1)) // TODO disable jumping if the player is already in-air
        {
            _rigidbody.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
            _jumpCount++;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            _movementDirection = Vector3.left;
        } else if (Input.GetKey(KeyCode.D))
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
        // TODO add deceleration variable and implement it
        
        // Initialize movement vector
        Vector3 movement = _movementDirection * _moveSpeed;

        // Apply force
        _rigidbody.AddForce(movement * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    bool IsGrounded()
    {
        // The position, height, and radius of the capsule's collider.
        Vector3 capsulePosition = transform.position;
        float capsuleHeight = GetComponent<CapsuleCollider>().height;
        float capsuleRadius = GetComponent<CapsuleCollider>().radius;

        // Check for overlapping colliders below the capsule to determine if it's grounded.
        Collider[] hitColliders = Physics.OverlapCapsule(capsulePosition + new Vector3(0, capsuleHeight / 2 - capsuleRadius, 0),
                                                          capsulePosition - new Vector3(0, capsuleHeight / 2 - capsuleRadius, 0),
                                                          capsuleRadius, groundLayer);

        return hitColliders.Length > 0;
    }
}
