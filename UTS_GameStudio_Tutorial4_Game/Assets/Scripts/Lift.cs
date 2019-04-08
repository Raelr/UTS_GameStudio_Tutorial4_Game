using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField]
    MovingPlatformController platformController;

    [SerializeField]
    BoxCollider2D liftBounds;

    [SerializeField]
    BoxCollider2D elevatorShaftBounds;

    Bounds liftDimensions;

    float liftMaxHeight;
    float liftMinHeight;

    // Start is called before the first frame update
    void Start()
    {
        platformController = GetComponent<MovingPlatformController>();

        CalculateLiftBounds();
    }

    // Update is called once per frame
    void Update()
    {
        Move(Input.GetAxisRaw("Vertical"));
    }

    void CalculateLiftBounds() {

        liftDimensions = liftBounds.bounds;

        liftMaxHeight = elevatorShaftBounds.bounds.max.y - liftDimensions.max.y * 1.86f;

        liftMinHeight = elevatorShaftBounds.bounds.min.y - liftDimensions.min.y / 8f;

    }

    void Move(float movement){

        Vector3 velocity = Vector3.up * movement * speed * Time.deltaTime;

        float projectedY = transform.position.y + velocity.y;

        if (projectedY  <=  liftMaxHeight && projectedY >= liftMinHeight) {

            platformController.MovePlatform(velocity);
        }
    }
}
