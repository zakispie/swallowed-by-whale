using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    
    // Keyboard Property
    public static Keyboard Keyboard => _instance._keyboard;
    
    // Most Recent Facing Direction Property
    public static Vector3 FacingDirection => _instance._facingDirection;

    // Most Recent Moving Direction Property
    public static Vector3 MovementDirection => _instance._movementDirection;

    // Most Recent Crouch Status
    public static bool IsCrouched => _instance._isCrouched;
   
    // Position Property
    public static Vector3 Position => _instance.transform.position;

    public static PlayerController Instance => _instance;
    
    // Health Component Property
    public static Health Health => _instance._health;

    // Animation Controller Child Property
    public static Animator AnimationController => _instance._animationController;

    // Player's height
    private float _playerHeightLocalScale;
    
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

    // Used to hold the fall force set in the inspector when the fall force is set to 0
    // when using in the ladder so we can reset the fall force back
    private float tempFallForce;

    [Header("Other")]
    [Tooltip("Ground Layer")]
    [SerializeField] private LayerMask groundLayer;

    [Header("UI")]
    [Tooltip("Slider for displaying power-up duration")]
    public Slider PowerUpUI;

    [Header("SFX")]
    [Tooltip("Manages sound for level")]
    public static SoundManager soundManager;

    // Singleton instance of the player controller
    private static PlayerController _instance;
    
    // Tracks the player's most recent facing direction (face right by default)
    private Vector3 _facingDirection = Vector3.right;

    // Tracks the player's current movement direction
    private Vector3 _movementDirection;

    // Tracks whether the player is currently grounded
    // private bool _isGrounded;

    // Tracks whether the player is currently crouching
    private bool _isCrouched = false;
    
    // Trakc whether player is currently on a ladder
    public static bool _onLadder = false;

    // Tracks num jumps player has performed
    private int _jumpCount;
    
    // Cache the health component
    private Health _health;

    //Cache the animationController component
    private Animator _animationController;

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
        _health = GetComponent<Health>();
        _keyboard = Keyboard.current;
        _mouse = Mouse.current;
        _playerHeightLocalScale = transform.localScale.y;
        tempFallForce = fallForce;
        _animationController = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Determine necessary movement from keypresses
    /// </summary>
    void Update()
    {
        // When on ladder you can not jump or crouch, rest of movement behavior is same
        HandleJumpOrLadder();
        ApplyInputs();
    }

    /// <summary>
    /// Handle gravity application
    /// </summary>
    void HandleJumpOrLadder()
    {
        if (_onLadder)
        {
            // prevents player from being effected by gravity
            _rigidbody.useGravity = false;
            tempFallForce = fallForce;
            fallForce = 0;
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            // adds gravitational forces back
            _rigidbody.useGravity = true;
            fallForce = tempFallForce;
        }

        // If on ladder:
        // * moves the player character forward onto the ladder
        // * reads to player that the player character is on ladder + allows them to move past platforms on ladder
        // If not on ladder:
        // * moves player back to the game's z axis to stand on platforms
        Vector3 newPosition = transform.position;
        newPosition.z = _onLadder ? -1.5f : 0f;
        transform.position = newPosition;

        var isGrounded = IsGrounded();

        if (_onLadder)
        {
            if (_keyboard.wKey.isPressed) // move up on ladder
            {
                transform.position += Vector3.up * (climbSpeed * Time.deltaTime);
            }
            else if (_keyboard.sKey.isPressed && !isGrounded)
            {
                // move down on ladder if player is not on ground
                transform.position += Vector3.down * (climbSpeed * Time.deltaTime);
            }
            else if (_keyboard.sKey.isPressed && isGrounded)
            {
                // crouch if at bottom of ladder and on ground
                Crouch();
            }
            else
            {
                StandUp();
            }
        }
        else
        {
            // Allow jump if player is grounded or has not performed double jump
            if ((_keyboard.wKey.wasPressedThisFrame || _keyboard.spaceKey.wasPressedThisFrame) && (isGrounded || _jumpCount < numJumps - 1))
            {
                
                if(isGrounded)
                {   
                    // IF on first jump, apply the full jumpstrength
                    _rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                } else {
                    // If doing a double jump, apply half the jumpstrength
                    // Makes it feel less like you can fly
                    _rigidbody.AddForce(Vector3.up * (jumpStrength / 1.9f), ForceMode.Impulse);
                }
                _facingDirection = Vector3.up;
                _jumpCount++;
            }

            // Implements crouching
            if(_keyboard.sKey.isPressed)
            {
                Crouch();
            }
            else
            {
                StandUp();
            }
        }
    }

    void ApplyInputs()
    {
        // Handle Inputs
        if (_keyboard.aKey.isPressed)
        {
            _movementDirection = Vector3.left;
            _facingDirection = Vector3.left;

            SoundManager.Instance.PlayWalkingSFX();
        }
        else if (_keyboard.dKey.isPressed)
        {
            _movementDirection = Vector3.right;
            _facingDirection = Vector3.right;

            SoundManager.Instance.PlayWalkingSFX();
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
        if(!_onLadder)
        {
            FlipRotationBasedOnFacingDirection();
        }

        if (IsGrounded())
        {
            _jumpCount = 0;
        }

        if(IsFalling()){
            // After jumps --> on descent, add force to fall faster
            _rigidbody.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);

        }

        ApplyMovementVector();
    }

    void FlipRotationBasedOnFacingDirection()
    {
        if(_facingDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        } 
        else if(_facingDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void ApplyMovementVector()
    {
        // Initialize movement vector and apply to the rigidbody
        Vector3 movement = _movementDirection * (moveStrength * Time.fixedDeltaTime);
        
        if (movement == Vector3.zero) 
        {
            // No movement input, we should decelerate
            Vector3 curVelocity = _rigidbody.velocity;
            
            // Apply the deceleration force (on the x-axis only) via a Lerp
            _rigidbody.velocity =
                Vector3.Lerp(curVelocity, new Vector3(0, curVelocity.y, curVelocity.z),
                    deceleration * Time.fixedDeltaTime);
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
        CapsuleCollider groundCollider;

        // Check if the material is not null to pick the collider that is frictionless
        if (_capsuleColliders[0].material != null)
        {
           groundCollider = _capsuleColliders[0];
        }
        else
        {
           groundCollider = _capsuleColliders[1];
        }

        // The position, height, and radius of the capsule's collider.
        Vector3 capsulePosition = _capsuleColliders[0].transform.position;
        float capsuleHeight = _capsuleColliders[0].height;
        float capsuleRadius = _capsuleColliders[0].radius;

        // Check for overlapping colliders below the capsule to determine if it's grounded.
        Collider[] hitColliders = Physics.OverlapCapsule(
            capsulePosition + new Vector3(0, 100 + (capsuleHeight / 2 - capsuleRadius), 0),
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
        return _rigidbody.velocity.y < 2.0;
    }

    /// <summary>
    /// Puts player in crouching position
    /// </summary>
    void Crouch()
    {
        if(!_isCrouched)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
            _isCrouched = true;
        }

        //transform.localScale = new Vector3(1f, _playerHeightLocalScale / crouchingHeight, 1f);
    }

    /// <summary>
    /// Takes player out of crouching position
    /// </summary>
    void StandUp()
    {
        _isCrouched = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    /// <summary>
    /// Applies speed powerup to the player
    /// </summary>
    public static void SpeedPowerup()
    {
        _instance.StartCoroutine(nameof(ApplySpeedPowerup));
    }

    /// <summary>
    /// Applies speed powerup, doubling player velocity for 10 seconds before returning to normal
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplySpeedPowerup()
    {
        // TODO we can abstract this to take in multiple powerup types, and also clean up the code a lot
        // this is just a messy version that gets the job done for playtesting
        var originalMoveStrength = moveStrength;
        var originalMaxVelocity = maxVelocity;
        var originalDeceleration = deceleration;
        moveStrength *= 2;
        maxVelocity *= 2;
        deceleration *= 2;

        // Displays in UI how much time is left for this
        // TODO Abstract UI into own script???
        float startTime = Time.time;
        float totalTime = 10f;

        PowerUpUI.gameObject.SetActive(true);
        PowerUpUI.value = 1f;
        while (Time.time - startTime < totalTime)
        {
            float elapsedTime = Time.time - startTime;

            // Calculate the progress of the powerup (0 to 1) and uses Lerp to make
            // UI/time progress smoothly
            float progress = elapsedTime / totalTime;
            PowerUpUI.value = Mathf.Lerp(1f, 0f, progress);

            // Waits for next frame
            yield return null;
        }
        PowerUpUI.gameObject.SetActive(false);

        moveStrength = originalMoveStrength;
        maxVelocity = originalMaxVelocity;
        deceleration = originalDeceleration;
    }
}