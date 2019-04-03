using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : Enemy
{

    private void Start() {

        controller = GetComponent<Controller2D>();
        direction = -Vector3.right;
    }

    private void Update() {
        Move();
    }
}
