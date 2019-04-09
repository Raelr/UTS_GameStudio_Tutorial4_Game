using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CollisionUser {

    public bool IsAlive { get { return isAlive; } }

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    [SerializeField]
    protected Collider2D boxCollider;

    [SerializeField]
    protected Animator animator;

    protected Vector3 direction;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    protected bool isAlive;

    protected bool canMove;

    bool instantiated = false;

    protected void Move() {

        if (Utilities.Vector3Equals(direction, Vector3.right) && controller.CollisionInfo.isRight) {

            direction = -Vector3.right;

        } else if (Utilities.Vector3Equals(direction, -Vector3.right) && controller.CollisionInfo.isLeft) {

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

                if (spriteRenderer.transform.rotation.y == 0) {
                    spriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(spriteRenderer.transform.rotation.eulerAngles.x, 180, spriteRenderer.transform.rotation.eulerAngles.z));
                }

            } else if (Utilities.Vector3Equals(direction, -Vector3.right)) {

                if (spriteRenderer.transform.rotation.y == 180) {

                    spriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(spriteRenderer.transform.rotation.eulerAngles.x, 0, spriteRenderer.transform.rotation.eulerAngles.z));
                }
            }
        }
    }

    protected void Init() {

        controller = GetComponent<Controller2D>();

        boxCollider = GetComponent<BoxCollider2D>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        CanMove = true;

        controller.ignoringCollisions += IgnoreCollisions;

        controller.onCollision += CheckPlatformCollider;

        controller.onCollision += OnPlayerCollisionAfterInstatiation;

    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0, bool isCrouching = false) {

        return hit.transform.tag == "Player";
    }

    protected void OnPlayerCollisionAfterInstatiation(RaycastHit2D[] hits) {

        foreach (RaycastHit2D hit in hits) {

            if (hit.transform.tag == "Player" && instantiated) {
                GameManager.instance.KillPlayer();
            }
        }

    }

    public void InitialiseEnemy() {

        StartCoroutine(Initialised());
    }

    protected IEnumerator Initialised() {

        instantiated = true;

        yield return null;

        instantiated = false;
    } 
}
