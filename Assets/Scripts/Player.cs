using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    
    // Start is called before the first frame update
    void Start()
    {
        //take the current pos = new position(0,0,0);
        transform.position = new Vector3(0, 0, 0);

    }

    // Update is called once per  frame i e., 60 frames/sec
    void Update()
    {

        //Movement using horizontal
        float horizontalInput = Input.GetAxis("Horizontal");

        //Movement using Vertical
        float verticalInput = Input.GetAxis("Vertical");

        //Moves to right..
        //transform.Translate(Vector3.right);
        //transform.Translate(new Vector3(1,0,0)); // +1 x right (1 meter per frame in real world..)

        // To move from 1 meter per frame to 1 meter per sec need to incorporate using time.deltaTime
        //transform.Translate(Vector3.right * Time.deltaTime); // now moving 1 meter per second.
        //transform.Translate(Vector3.right * 5 * Time.deltaTime); // now moving 5 meter per second.

        // To move based on horizontaly->> new Vector3(1, 0, 0) * _speed * horizontalInput *Time.deltaTime
        //transform.Translate(new Vector3(1, 0, 0) * _speed * horizontalInput *Time.deltaTime);

        // To move based on vertical->> new Vector3(0, 1, 0) * _speed * verticalInput *Time.deltaTime
        //transform.Translate(new Vector3(0, 1, 0) * _speed * verticalInput * Time.deltaTime);

        //For horizontal/vettical movement
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed  * Time.deltaTime);



        //Moves to left..
        //transform.Translate(Vector3.left);
        //transform.Translate(new Vector3(-1,0,0)); //-1 x left

        //Moves to up..
        //transform.Translate(Vector3.up); 
        //transform.Translate(new Vector3(0,1,0)); // +1 y up

        //Moves to down..
        //transform.Translate(Vector3.down); 
        //transform.Translate(new Vector3(0,-1,0)); // -1 y donwn

    }
}
