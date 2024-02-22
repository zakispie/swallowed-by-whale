using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class to handle crew ghost's behavior and logic
/// </summary>
public class GhostBehavior : EnemyBehavior
{
    [Tooltip("How many units the ghost can see the player")] [SerializeField]
    private float aggroDistance;

    [Tooltip("How many units the ghost will attempt to attack the player")] [SerializeField]
    private float attackAttemptDistance;

    [Tooltip("How many units the ghost can actually attack or reach")] [SerializeField]
    private float attackReachDistance;

    [Tooltip("How many seconds the ghost will wait before attacking")] [SerializeField]
    private float attackTelegraphTime;

    [Tooltip("How close to a wall (or other obstacle) will the ghost get before turning around?")] [SerializeField]
    private float wallDistance;

    [Tooltip("How many units above the ghost should a raycast be cast from to check for a ledge?")] [SerializeField]
    private float ledgeCheckHeight;

    [Tooltip("How long should the raycast from the ledge check be?")] [SerializeField]
    private float ledgeCheckDistance;

    [Tooltip("Ghost move speed")] [SerializeField]
    private float moveSpeed;

    /// <summary>
    /// Assign necessary variables
    /// </summary>
    void Start()
    {
        // Randomly assign a facing direction
        facingDirection = Random.Range(0f, 1f) > 0.5f ? Vector3.right : Vector3.left;
        currentState = BehaviorState.Wander;
    }

    // Override Method
    protected override void Wander()
    {
        // Check if the ghost can see the player, transition to Chase if so
        if (CanSeePlayer())
        {
          //  print("I spotted the player! Chasing...");
            currentState = BehaviorState.Chase;
            return;
        }
        
       // print("Can't see player, wandering...");

        // Define an origin for start of ledge check
        var origin = transform.position + (Vector3.up * ledgeCheckHeight);
        // Define direction to be 45 degrees by making the y of facingDirection = 0.5f
        var direction = facingDirection + (Vector3.down * 0.5f);

        // Debug to show the ledge check line:
        // var endpoint = origin + direction * ledgeCheckDistance;
        // Debug.DrawLine(origin, endpoint);

        // Check if there's anything in the way of the ghost, or if the ghost is approaching a ledge (raycast for ledge check hits nothing)
        if (Physics.Raycast(transform.position, facingDirection, wallDistance)
            || !Physics.Raycast(origin, direction, ledgeCheckDistance))
        {
            facingDirection = facingDirection == Vector3.right ? Vector3.left : Vector3.right;
        }

        // Move in the current facing direction
        transform.position += facingDirection * (Time.fixedDeltaTime * moveSpeed);
    }

    // Override Method
    protected override void Chase()
    {
        // check if we have line of sight to the player by raycasting 
        if (Physics.Raycast(transform.position, PlayerController.Position - transform.position, out var hit,
                aggroDistance))
        {
            // we hit something
            if (!hit.collider.CompareTag("Player"))
            {
           //     print("Lost line of sight of player, wandering...");
                // lost line of sight since the raycast hit something that wasn't the player
                currentState = BehaviorState.Wander;
                return;
            }

            // we hit the player, still have line of sight, now how far is the player from us?
            if (Vector3.Distance(transform.position, PlayerController.Position) <= attackAttemptDistance)
            {
           //     print("Player in my attack range!");
                // we are close enough to attack
                currentState = BehaviorState.Attack;
                return;
            }

            // we are not close enough to attack but can see the player, set our facing direction to face the player then chase
            facingDirection = (PlayerController.Position - transform.position).normalized;
            facingDirection.y = 0; // ensure the ghost doesn't try to jump
            transform.position += facingDirection * (Time.fixedDeltaTime * moveSpeed);
        }
        else
        {
           // print("Player left my range, wandering...");
            // nothing was hit, so the player must be out of aggroDistance
            currentState = BehaviorState.Wander;
            return;
        }
    }

    // Override Method
    protected override void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            Invoke(nameof(PerformAttack), attackTelegraphTime);
        }
    }

    /// <summary>
    /// Helper method to actually perform the attack, so it can be delay invoked
    /// </summary>
    private void PerformAttack()
    {
        if (Physics.Raycast(transform.position, facingDirection, out var hit, attackReachDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerController.HealthBar.TakeDamage(1);
            }
        }

        attacking = false;
        currentState = BehaviorState.Wander;
    }

    /// <summary>
    /// Determine if the player can be spotted by raycasting in the currently facing direction
    /// </summary>
    /// <returns> True if the raycast hits, and the raycast connects with the player </returns>
    private bool CanSeePlayer()
    {
        return Physics.Raycast(transform.position, facingDirection, out var hit, aggroDistance) &&
               hit.collider.CompareTag("Player");
    }
}