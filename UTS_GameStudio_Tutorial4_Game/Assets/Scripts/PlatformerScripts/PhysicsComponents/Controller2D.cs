﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Controller2D : RayCastUser {

    public float Gravity { get { return gravity; } }

    public float MoveSpeed { get { return moveSpeed; } }

    public float JumpVelocity { get { return jumpVelocity; } }

    public float JumpHeight { get { return jumpHeight; } }

    public float TimeToJumpApex { get { return timeToJumpApex; } }

    public float VelocityXSmoothing { get { return velocityXSmoothing; } }

    public float VelocityYSmoothing { get { return velocityYSmoothing; } }

    public float AccelerationTimeAirborn { get { return accelerationTimeAirborn; } }

    public float AccelerationTimeGrounded { get { return accelerationTimeGrounded; } }

    public bool IsCrouching { get; set; }

    [Header("Collision Mask")]
    [SerializeField]
    LayerMask layerMask = 0;

    [Header("Physics Configuration")]

    [SerializeField]
    float moveSpeed = 3;

    [SerializeField]
    float jumpHeight = 0;

    [SerializeField]
    float timeToJumpApex = 0;

    [SerializeField]
    float accelerationTimeAirborn = 0;

    [SerializeField]
    float accelerationTimeGrounded = 0;

    [Header("Physics Results")]
    [SerializeField, ReadOnly]
    float jumpVelocity;

    [SerializeField, ReadOnly]
    float gravity;

    float velocityXSmoothing = 0;

    float velocityYSmoothing = 0;

    [Header("SlopeClimbing")]
    [SerializeField, Range(0, 360)]
    float maxClimbAngle = 0;

    Vector3 velocity;

    public bool ignoreNextRayCast;

    public delegate bool IgnoreCollisionHandler(RaycastHit2D hit, float direction = 0, bool isCrouching = false);

    public IgnoreCollisionHandler ignoringCollisions;

    public delegate void CollisionHandler(RaycastHit2D[] hits);

    public CollisionHandler onCollision;

    public delegate void VerticalPostConditionHanlder(RaycastHit2D hit);

    public VerticalPostConditionHanlder onVerticalCollision;

    public delegate void HorizontalPostConditionHanlder(RaycastHit2D hit);

    public HorizontalPostConditionHanlder onHorizontalCollision;

    /// <summary>
    /// Set jump velocity and gravity base don the jump height and timeToJumpApex variables. 
    /// </summary>

    public override void Start() {

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        base.Start();

    }

    /// <summary>
    /// Handles all movement (included jumps) and sets all possible collisions for the player to have given the velocity vector.
    /// </summary>
    /// <param name="input"> The velocity vector used to move the player </param>

    public void Move(Vector3 input, bool isStandingOnPlatform = false) {

        // Sets the positions where the raycasts will shoot from. Can account for changes in sprite and collider size. 
        UpdateRayCastOrigins();

        collisionInformation.Reset();

        // Update Collisons if player is moving horizontally.

        if (input.x != 0) {
            HorizontalCollisions(ref input);
        }

        if (!Utilities.Vector3Equals(input, Vector3.zero)) {

            Physics2D.SyncTransforms();
        }

        // Update Collisons if player is moving vertically (jumping or falling).

        if (input.y != 0) {
            VerticalCollisions(ref input);
        }

        if (!Utilities.Vector3Equals(input, Vector3.zero)) {

            // Move the player.
            transform.Translate(input);

            if (isStandingOnPlatform) {
                collisionInformation.isBelow = true;
            }
            Physics2D.SyncTransforms();
            Physics.SyncTransforms();
        }
    }

    /// <summary>
    /// Handles horizontal movement and damps the x axis in order to provide smooth movement.
    /// Also applies gravity.
    /// </summary>
    /// <param name="input"> The inputted Velocity vector </param>

    public void ApplyMovement(Vector3 input, bool isStandingOnPlatform = false) {

        // Get the velocity we need.
        float targetVelocityX = input.x * MoveSpeed;

        // Value set for smoothing the movement. 
        float xSmoothing = VelocityXSmoothing;

        // Damp the horizontal movement.
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref xSmoothing, collisionInformation.isBelow ? AccelerationTimeGrounded : AccelerationTimeAirborn);

        if (collisionInformation.isRight || collisionInformation.isLeft) {
            velocity.x = 0;
        }

        // Move the player using the input velocity vector.
        Move(velocity * Time.deltaTime, isStandingOnPlatform);
    }

    public void ApplyGravity(ref Vector3 inputVelocity, bool isStill = false) {

        // If we have a collision above the player or below then we reset the velocity (stops velocity from being increased constantly even when grounded)
        if (collisionInformation.isAbove || collisionInformation.isBelow) {
            velocity.y = 0;
        }

        velocity.y += gravity * Time.deltaTime;

        if (isStill) {
            ApplyMovement(inputVelocity);
        }
    }

    public void Jump(ref Vector3 inputVelocity) {

        if (collisionInformation.isBelow) {

            inputVelocity.y = jumpVelocity;
            velocity.y = inputVelocity.y;
        }
    }

    public void Bounce() {

        if (collisionInformation.isBelow) {
            velocity.y = jumpVelocity / 2;
        }
    }

    public void Crouch(bool newIsCrouching) {

        if (IsCrouching != newIsCrouching) {
            if (newIsCrouching) {
                if (collisionInformation.isBelow) {
                    IsCrouching = true;
                }
            } else {
                IsCrouching = false;
            }
        }
    }

    /// <summary>
    /// Determines if a collision has occurred on the horizontal axes. Adjusts the velocity vector's appropriate axis if 
    /// it it's distance between itself and the object is zero (or close to).
    /// </summary>

    void HorizontalCollisions(ref Vector3 inputVelocity) {

        // Determine if the direction is positive or negative
        float directionX = Mathf.Sign(inputVelocity.x);

        // Determine how far the length of the ray needs to be.
        float rayLength = Mathf.Abs(inputVelocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {

            // Detemrine where to start shooting the rays from (bottom left of player or bottom right)
            Vector2 rayOrigin = (directionX == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.bottomRight;

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            // Shoot a ray that is looking for objects in the correct layermask.
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.right * directionX, rayLength, layerMask);

            foreach (RaycastHit2D hit in hits) {
                if (hit.transform == transform) {
                    continue;
                } else {

                    if (hit) {

                        onCollision?.Invoke(hits);

                        bool isIgnoringCollisions = ignoringCollisions != null ? ignoringCollisions.Invoke(hit, directionX) : false;

                        if (isIgnoringCollisions) {
                            continue;

                        } else {

                            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                            if (i == 0 && slopeAngle <= maxClimbAngle) {
                                float distanceToStart = 0;

                                if (slopeAngle != collisionInformation.slopeAngleOld) {
                                    distanceToStart = hit.distance - skinWidth;
                                    //inputVelocity.x -= distanceToStart - directionX;
                                }
                                ClimbSlope(ref inputVelocity, slopeAngle);
                                inputVelocity.x += distanceToStart * directionX;
                            }

                            if (!collisionInformation.isClimbingSlope || slopeAngle > maxClimbAngle) {
                                // Reduce velocity vector based on its distance from the obstacle collided with. 
                                inputVelocity.x = (hit.distance - skinWidth) * directionX;
                                rayLength = hit.distance;

                                if (collisionInformation.isClimbingSlope) {
                                    inputVelocity.y = Mathf.Tan(collisionInformation.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(inputVelocity.x);
                                }

                                // Update the collision information struct to indicate that a collision has occurred.
                                collisionInformation.isLeft = directionX == -1;
                                collisionInformation.isRight = directionX == 1;

                                if (collisionInformation.isLeft || collisionInformation.isRight) {

                                    onHorizontalCollision?.Invoke(hit);
                                }
                            }
                        }
                    }
                }
            }
            // Draw a ray for the purposes of debugging
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
        }
    }

    /// <summary>
    /// Determines if a collision has occurred on the vertical axes. Adjusts the velocity vector's appropriate axis if 
    /// it it's distance between itself and the object is zero (or close to).
    /// </summary>

    void VerticalCollisions(ref Vector3 velocity) {

        // Determine if the direction is positive or negative
        float directionY = Mathf.Sign(velocity.y);

        // Determine how far the length of the ray needs to be.
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {

            // Detemrine where to start shooting the rays from (bottom left of player or bottom right)
            Vector2 rayOrigin = (directionY == -1) ? rayCastOrigins.bottomLeft : rayCastOrigins.topLeft;

            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            // Shoot a ray that is looking for objects in the correct layermask.
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.up * directionY, rayLength, layerMask);

            foreach (RaycastHit2D hit in hits) {

                if (hit.transform == transform) {
                    continue;
                } else {

                    if (hit) {

                        onCollision?.Invoke(hits);

                        bool isIgnoringCollisions = ignoringCollisions != null ? ignoringCollisions.Invoke(hit, directionY, IsCrouching) : false;

                        if (isIgnoringCollisions) {

                            continue;

                        } else {
                            // Reduce velocity vector based on its distance from the obstacle collided with. 
                            velocity.y = (hit.distance - skinWidth) * directionY;
                            rayLength = hit.distance;

                            if (collisionInformation.isClimbingSlope) {
                                velocity.x = velocity.y / Mathf.Tan(collisionInformation.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                            }

                            // Update the collision information struct to indicate that a collision has occurred.
                            collisionInformation.isBelow = directionY == -1;
                            collisionInformation.isAbove = directionY == 1;

                            if (collisionInformation.isAbove || collisionInformation.isBelow) {
                                onVerticalCollision?.Invoke(hit);
                            }
                        }
                    }
                }
            }
            // Draw a ray for the purposes of debugging
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
        }
    }

    /// <summary>
    /// Calculates the angle of a slope and moves up if the slope can be asceneded.
    /// </summary>
    /// <param name="inputVelocity"> The current velocity being used to move </param>
    /// <param name="slopeAngle"> The angle of the slope being searched for </param>

    void ClimbSlope(ref Vector3 inputVelocity, float slopeAngle) {

        float moveDistance = Mathf.Abs(inputVelocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (inputVelocity.y <= climbVelocityY) {
            inputVelocity.y = climbVelocityY;
            inputVelocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(inputVelocity.x);

            collisionInformation.isBelow = true;
            collisionInformation.isClimbingSlope = true;
            collisionInformation.slopeAngle = slopeAngle;
        }
    }
}




