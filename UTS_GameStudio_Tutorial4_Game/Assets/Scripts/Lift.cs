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

    Player player;

    Bounds liftDimensions;

    float liftMaxHeight;
    float liftMinHeight;

    // Start is called before the first frame update
    void Start()
    {
        platformController = GetComponent<MovingPlatformController>();

        CalculateLiftBounds();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerOnMe())
            Move(Input.GetAxisRaw("Vertical"));
    }

    public bool IsPlayerOnMe(){
        Vector3 extent = gameObject.GetComponent<Collider2D>().bounds.extents;
        return player.transform.position.x > transform.position.x - extent.x&&
           player.transform.position.x < transform.position.x + extent.x;
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
