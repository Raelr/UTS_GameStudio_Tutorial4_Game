using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CollisionUser {

    public bool IsAlive { get { return isAlive; } }

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    [SerializeField]
    protected Collider2D collider;

    [SerializeField]
    protected Animator animator;

    protected Vector3 direction;

    [SerializeField]
    SpriteRenderer renderer;

    protected bool isAlive;

    protected bool canMove;

    protected void Move() {

        if (Utilities.Vector3Equals(direction, Vector3.right) && controller.CollisionInformation.isRight) {

            direction = -Vector3.right;

        } else if (Utilities.Vector3Equals(direction, -Vector3.right) && controller.CollisionInformation.isLeft) {

            direction = Vector3.right;
        }

        controller.ApplyGravity(ref direction);

        controller.ApplyMovement(direction);
    }


    protected void UpdateFacingDirection(Vector3 input) {

        float xValue = Mathf.RoundToInt(input.x);

        Vector3 direction = new Vector3(xValue, 0, 0);

        if (direction != Vector3.zero) {

            if (Utilities.Vector3Equals(direction, Vector3.right)) {

                if (renderer.transform.rotation.y == 0) {
                    renderer.transform.rotation = Quaternion.Euler(new Vector3(renderer.transform.rotation.eulerAngles.x, 180, renderer.transform.rotation.eulerAngles.z));
                }

            } else if (Utilities.Vector3Equals(direction, -Vector3.right)) {

                if (renderer.transform.rotation.y == 180) {

                    renderer.transform.rotation = Quaternion.Euler(new Vector3(renderer.transform.rotation.eulerAngles.x, 0, renderer.transform.rotation.eulerAngles.z));
                }
            }
        }
    }

    protected void Init() {

        controller = GetComponent<Controller2D>();

        collider = GetComponent<BoxCollider2D>();

        renderer = GetComponentInChildren<SpriteRenderer>();

        CanMove = true;

        controller.ignoringCollisions += IgnoreCollisions;

        controller.onCollision += CheckPlatformCollider;

    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0, bool isCrouching = false) {

        return false;
    }
}
