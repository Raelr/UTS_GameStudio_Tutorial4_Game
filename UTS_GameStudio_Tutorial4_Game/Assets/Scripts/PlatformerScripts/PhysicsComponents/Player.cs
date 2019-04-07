﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CollisionUser {

    Vector3 input;

    // Delegate for anything which needs to know whether the player is moving
    public delegate void PlayerMovedHandler();

    public event PlayerMovedHandler playerMoved;

    private void Awake() {

        Initialise();

        controller.onCollision += OnEnemyHit;
        controller.onCollision += OnCoinCollision;
    }

    private void Start() {

        GameManager.instance.Player = this;
    }

    void Update() {

        MoveByInput();
    }

    /// <summary>
    /// Maps player inputs into movement and calls appropriate commands from the controller.
    /// </summary>

    void MoveByInput() {

        // Get the current axis values and add them to a vector.
        FindAxes();

        bool crouching;

        if (Input.GetKey("s") || Input.GetKey("down")) {
            crouching = true;
        } else {
            crouching = false;
        }

        controller.Crouch(crouching);

        if (IsStill() && !SpacePressed()) {
            // If we arent moving then just apply gravity normally.
            controller.ApplyGravity(ref input, true);

        } else {

            controller.ApplyGravity(ref input);

            if (SpacePressed()) {

                controller.Jump(ref input);

            }

            controller.ApplyMovement(input);

        }

        // If anything is listening for player movement then invoke the delegate.
        playerMoved?.Invoke();
    }

    /// <summary>
    /// Modifies the input vector to have the horizontal and vertical axes values.
    /// </summary>

    void FindAxes() {

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.z = 0;
    }

    bool SpacePressed() {
        return Input.GetKeyDown(KeyCode.Space);
    }

    bool IsStill() {
        return input.x == 0 && input.y == 0;
    }

    public void OnEnemyHit(RaycastHit2D hit) {

        if (hit.transform.tag == "Enemy") {

            GameManager.instance.KillPlayer();
        }
    }

    public void OnCoinCollision(RaycastHit2D hit) {
        if (hit.transform.tag == "Coin") {
            Coin coin = hit.transform.GetComponent<Coin>();
            coin.OnCoinPickUp();
        }
    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0, bool isCrouching = false) {

        bool success = false;

        bool CheckingDirection = direction != 0;

        if (currentPlatform != null) {

            success = currentPlatform.AllowedToJumpThrough(direction, true) || isCrouching && currentPlatform.CanFallThrough() || hit.distance == 0 || hit.transform.tag == "Enemy";

        } else {

            success = hit.distance == 0 || hit.transform.tag == "Enemy" || hit.transform.tag == "Coin";
        }
        
        return success;
    }
}
