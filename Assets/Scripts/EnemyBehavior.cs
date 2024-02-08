using UnityEngine;

/// <summary>
/// Abstract class representing the skeleton of an enemy's behavior
/// </summary>
public abstract class EnemyBehavior : MonoBehaviour
{
    /// <summary>
    /// Enum to represent discrete behavior states
    /// </summary>
    protected enum BehaviorState
    {
        Wander,
        Chase,
        Attack
    }

    // Tracks the current state of the enemy
    protected BehaviorState currentState;

    // Tracks currently facing direction
    protected Vector3 facingDirection;

    // Tracks whether the enemy is currently attacking
    protected bool attacking;

    /// <summary>
    /// Handles facing direction flips and behavior state delegation
    /// </summary>
    void FixedUpdate()
    {
        // Make the enemy flip to its facing direction
        if (facingDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (facingDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Perform the behavior based on the current state
        switch (currentState)
        {
            case BehaviorState.Wander:
                Wander();
                break;
            case BehaviorState.Chase:
                Chase();
                break;
            case BehaviorState.Attack:
                Attack();
                break;
        }
    }

    /// <summary>
    /// Wander State of this enemy
    /// </summary>
    protected abstract void Wander();

    /// <summary>
    /// Chase State of this enemy
    /// </summary>
    protected abstract void Chase();

    /// <summary>
    /// Attack State of this enemy
    /// </summary>
    protected abstract void Attack();
}