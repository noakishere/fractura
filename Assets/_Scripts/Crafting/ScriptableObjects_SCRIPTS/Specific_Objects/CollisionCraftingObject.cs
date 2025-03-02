using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoundedCollisionCraftingObjectData", menuName = "CraftingSystem/Bounded Collision Crafting Object Data", order = 3)]
public class CollisionCraftingObject : CraftingObject
{
    [Header("Collision Conditions")]
    [SerializeField] private bool requireCollisionCondition = true;

    [Tooltip("If set, the outcome will only execute when colliding with this specific GameObject. If not set, a tag check will be used.")]
    [SerializeField] private GameObject requiredCollider;

    [Tooltip("If no specific collider is set, the outcome will execute only if a collider with this tag is found.")]
    [SerializeField] private string requiredCollisionTag;

    [Tooltip("Search radius to look for a valid target when using a tag (e.g. for finding nearby rats).")]
    [SerializeField] private float collisionSearchRadius = 5f;

    [SerializeField] private bool shouldDestroyOnExecution;

    public override void ExecuteOutcome(GameObject user)
    {
        //// First, check that the user is within the allowed bounds (from BoundedCraftingObject).
        //Vector2 userPos = user.transform.position;
        //if (userPos.x < MinBounds.x || userPos.x > MaxBounds.x || userPos.y < MinBounds.y || userPos.y > MaxBounds.y)
        //{
        //    Debug.LogWarning($"{ObjectName} cannot execute because the user is outside allowed bounds ({MinBounds} to {MaxBounds}).");
        //    return;
        //}

        // Then, check collision conditions if required.
        if (requireCollisionCondition)
        {
            // If a specific collider is set, then the colliding object must be exactly that.
            if (requiredCollider != null)
            {
                if (user != requiredCollider)
                {
                    Debug.LogWarning($"{ObjectName} outcome not executed because the colliding object is not the required one.");
                    return;
                }
            }
            // Otherwise, use the required tag. For cases like "Rat" where there might be multiple valid targets:
            else if (!string.IsNullOrEmpty(requiredCollisionTag))
            {
                // If the user is not the target, then search for valid colliders within a given radius.
                // For example, if the effect should only happen when a rat is nearby, we look for the nearest rat.
                Collider2D[] colliders = Physics2D.OverlapCircleAll(user.transform.position, collisionSearchRadius);
                GameObject validTarget = null;
                float closestDist = float.MaxValue;
                foreach (var col in colliders)
                {
                    //Debug.Log(col);
                    if (col.CompareTag(requiredCollisionTag))
                    {
                        float dist = Vector2.Distance(user.transform.position, col.transform.position);
                        if (dist < closestDist)
                        {
                            validTarget = col.gameObject;
                            closestDist = dist;
                        }
                    }
                }

                if(validTarget != null)
                {
                    // TO REMOVE FOR THE BUILD
                    validTarget.GetComponent<SpriteRenderer>().color = Color.yellow;

                    if(validTarget.TryGetComponent(out IOutcomeReceiver outcomeReceiver))
                    {
                        outcomeReceiver.ReceiveOutcome(this);
                    }
                }

                if (validTarget == null)
                {
                    Debug.LogWarning($"{ObjectName} outcome not executed because no {requiredCollisionTag} was found within radius {collisionSearchRadius}.");
                    return;
                }

                // Optionally, if you want the outcome to affect the rat instead of the player, you can substitute:
                //user = validTarget;
            }
        }

        // All conditions met; execute the outcome using the chosen valid collider (player or nearest rat).
        base.ExecuteOutcome(user);

        if(shouldDestroyOnExecution)
        {
            Inventory.Instance.RemoveItems(this, 1);
        }
    }
}
