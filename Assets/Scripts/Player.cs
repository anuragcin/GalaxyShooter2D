using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //take the current pos = new position(0,0,0);
        transform.position = new Vector3(0, 0, 0);

        //find the GameObject and get the Component
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager cannot be null");
        }
    }

    // Update is called once per  frame i e., 60 frames/sec
    void Update()
    {
        CaclulateMovement();    

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        //Instantiate(_laserPrefab, transform.position,Quaternion.identity);
        // spawning laser at position on y axis at 0.8
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
        }
    }

    private void CaclulateMovement()
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
        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed  * Time.deltaTime);

        //Creating variable direction of type Vector3
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        //Moves to left..
        //transform.Translate(Vector3.left);
        //transform.Translate(new Vector3(-1,0,0)); //-1 x left

        //Moves to up..
        //transform.Translate(Vector3.up); 
        //transform.Translate(new Vector3(0,1,0)); // +1 y up

        //Moves to down..
        //transform.Translate(Vector3.down); 
        //transform.Translate(new Vector3(0,-1,0)); // -1 y donwn

        //Restricting on Y-Axis
        //if player position  on y >= 0 then y position =0
        //else if player position on y <-5.0f then set y position =-5.0f so it not move down further
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -5.0f)
        {
            transform.position = new Vector3(transform.position.x, -5.0f, 0);
        }

        //Restricting on X-Axis
        //if player position  on x >= 11 then x position = -11 so , it comes from another left x direction
        //else if player position on X <= -11 then set x position = 11 so it move from another right x direction
        if (transform.position.x >= 11.0f)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.0f)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
    }

    /// <summary>
    /// Damaging the Player Lives.
    /// </summary>
    public void Damage()
    {
         _lives--;

        //Check if dead & destroy the object
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDead();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        //Starts the coroutune
        StartCoroutine(TripleShotPowerDownRoutine());

    }


    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
}
