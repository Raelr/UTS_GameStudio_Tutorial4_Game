using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionUser : MonoBehaviour
{
    protected Collider2D currentPlatformCollider;

    protected Platform currentPlatform;

    [Header("Player Controller")]
    [SerializeField]
    protected Controller2D controller;

    protected Dictionary<Collider2D, Platform> platforms = new Dictionary<Collider2D, Platform>();

    protected abstract bool IgnoreCollisions(RaycastHit2D hit, float direction = 0, bool isCrouching = false);

    protected void Initialise() {

        controller = GetComponent<Controller2D>();

        controller.ignoringCollisions += IgnoreCollisions;

        controller.onCollision += CheckPlatformCollider;
    }

    public void CheckPlatformCollider(RaycastHit2D[] hits) {

        foreach (RaycastHit2D hit in hits) {

            if (hit.collider != currentPlatformCollider) {

                currentPlatformCollider = hit.collider;

                if (!platforms.ContainsKey(currentPlatformCollider)) {

                    currentPlatform = hit.transform.GetComponent<Platform>();

                    platforms.Add(currentPlatformCollider, currentPlatform);

                } else {

                    currentPlatform = platforms[currentPlatformCollider];
                }
            }
        }
    }
}
