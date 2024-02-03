using UnityEngine;
using Random = UnityEngine.Random;

public class RatBehavior : MonoBehaviour
{
    [Tooltip("How many units the rat can see the player")] [SerializeField]
    private float aggroDistance;

    [Tooltip("How many units the rat will attempt to attack the player")] [SerializeField]
    private float attackAttemptDistance;

    [Tooltip("How many units the rat can actually attack or reach")] [SerializeField]
    private float attackReachDistance;
    
    [Tooltip("How many seconds the rat will wait before attacking")] [SerializeField]
    private float attackTelegraphTime;
    
    [Tooltip("How close to a wall (or other obstacle) will the rat get before turning around?")] [SerializeField]
    private float wallDistance;

    [Tooltip("Rat move speed")] [SerializeField]
    private float moveSpeed;

    private Vector3 _facingDirection;

    private BehaviorState _currentState;

    enum BehaviorState
    {
        Wander,
        Chase,
        Attack
    }

    void Start()
    {
        // Randomly assign a facing direction
        _facingDirection = Random.Range(0f, 1f) > 0.5f ? Vector3.right : Vector3.left;
        _currentState = BehaviorState.Wander;
    }

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

        switch (_currentState)
        {
            case BehaviorState.Wander:
                Wander();
                break;
            case BehaviorState.Chase:
                Chase();
                break;
            case BehaviorState.Attack:
                // telegraph here
                print("telegraphing attack...");
                Invoke(nameof(Attack), attackTelegraphTime);
                break;
        }
    }

    private void Wander()
    {
        // Check if the rat can see the player, transition to Chase if so
        if (CanSeePlayer())
        {
            print("I spotted the player! Chasing...");
            _currentState = BehaviorState.Chase;
            return;
        }
        
        print("Can't see player, wandering...");

        // Check if there's anything in the way of the rat
        if (Physics.Raycast(transform.position, _facingDirection, wallDistance))
        {
            _facingDirection = _facingDirection == Vector3.right ? Vector3.left : Vector3.right;
        }

        // Move in the current facing direction
        transform.position += _facingDirection * (Time.fixedDeltaTime * moveSpeed);
    }

    private void Chase()
    {
        // check if we have line of sight to the player by raycasting 
        if (Physics.Raycast(transform.position, PlayerController.Position - transform.position, out var hit, aggroDistance))
        {
            // we hit something
            if (!hit.collider.CompareTag("Player"))
            {
                print("Lost line of sight of player, wandering...");
                // lost line of sight since the raycast hit something that wasn't the player
                _currentState = BehaviorState.Wander;
                return;
            }

            // we hit the player, still have line of sight, now how far is the player from us?
            if (Vector3.Distance(transform.position, PlayerController.Position) <= attackAttemptDistance)
            {
                print("Player in my attack range!");
                // we are close enough to attack
                _currentState = BehaviorState.Attack;
                return;
            }

            // we are not close enough to attack but can see the player, set our facing direction to face the player then chase
            _facingDirection = (PlayerController.Position - transform.position).normalized;
            _facingDirection.y = 0; // ensure the rat doesn't try to jump
            transform.position += _facingDirection * (Time.fixedDeltaTime * moveSpeed);
        }
        else
        {
            print("Player left my range, wandering...");
            // nothing was hit, so the player must be out of aggroDistance
            _currentState = BehaviorState.Wander;
            return;
        }
    }

    private void Attack()
    {
        print("Biting the player!");
        if (Physics.Raycast(transform.position, _facingDirection, out var hit, attackReachDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                print("I hit the player!");
                // deal damage to the player
            }
            else
            {
                print("I hit something not the player!");
            }
        }
        else
        {
            print("I missed the player and hit nothing!");
        }
        
        // return to Wander State
        _currentState = BehaviorState.Wander;
    }

    /// <summary>
    /// Determine if the player can be spotted by raycasting in the currently facing direction
    /// </summary>
    /// <returns> True if the raycast hits, and the raycast connects with the player </returns>
    private bool CanSeePlayer()
    {
        return Physics.Raycast(transform.position, _facingDirection, out var hit, aggroDistance) &&
               hit.collider.CompareTag("Player");
    }

}