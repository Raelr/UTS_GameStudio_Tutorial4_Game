using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : Enemy
{
    private void Awake() {

        Init();
        direction = -Vector3.right;
    }

    private void Update() {

        Move();
    }



}
