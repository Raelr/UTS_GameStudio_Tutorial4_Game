using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(Input.GetAxisRaw("Vertical"));
    }

    void Move(float movement){
        transform.position += Vector3.up * movement * speed * Time.deltaTime;
    }
}
