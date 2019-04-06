using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class MovingPlatformController : RayCastUser {
    public Vector3 move;

    public LayerMask passengerMask;

    public override void Start() {

        base.Start();
    }

    private void Update() {
        UpdateRayCastOrigins();

        Vector3 velocity = move * Time.deltaTime;

        MovePassengers(velocity);
        transform.Translate(velocity);
    }

    void MovePassengers(Vector3 velocity) {

        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform (up and down)

        if (velocity.y != 0) {

            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++) {

                // Detemrine where to start shooting the rays from (bottom left of player or bottom right)
                Vector2 rayOrigin = (directionY == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.topLeft;

                rayOrigin += Vector2.right * (verticalRaySpacing * i);

                // Shoot a ray that is looking for objects in the correct layermask.
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit) {

                    if (!movedPassengers.Contains(hit.transform)) {

                        movedPassengers.Add(hit.transform);

                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
                        float pushX = (directionY == 1) ? velocity.x : 0;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }

        // If a passenger is on top of downward moving platform.

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {

            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++) {

                // Detemrine where to start shooting the rays from (bottom left of player or bottom right)
                Vector2 rayOrigin = rayCastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);

                // Shoot a ray that is looking for objects in the correct layermask.
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit) {

                    if (!movedPassengers.Contains(hit.transform)) {

                        movedPassengers.Add(hit.transform);

                        float pushY = velocity.y;
                        float pushX = velocity.x;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }
    }
}
