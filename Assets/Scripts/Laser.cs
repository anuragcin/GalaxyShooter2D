using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed of the laser
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    [SerializeField]
    private int _LaserID; //0 for Laser and 1 for Secondary Missile
    private Player _player;
    private Enemy _enemy;
    private enum _enemyTypes
    {
        Alpha,
        Beta,
        SmartEnemy
    }

    private Rigidbody2D _rb;
    private _enemyTypes _enemyTypesSelected;

    void Start()
    {
        try
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
            {
                Debug.LogError("Laser RigidBody cannot be null");
            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log($"Object was not set in the inspector {ex.ToString()}"); ;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (_LaserID == 0)
        {
            if (!_isEnemyLaser)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }
        else if (_LaserID==1)
        {
            MoveUp();
        }
        
    }

    
    private void MoveUp()
    {
        //Move Upwards -> translate up..
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
            //if laser poistion >= 6 on y, destroy the laser..
            if (transform.position.y >= 6.0f)
            {

                //Destroy the parent
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(this.gameObject);
            }
    }

    private void MoveDown()
    {
        //Debug.Log(_enemyTypes);
        //Move Downward -> translate down..
        if (_enemyTypesSelected == _enemyTypes.Alpha) 
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            //if laser poistion <= -6 on y, destroy the laser..
            if (transform.position.y <= -6.0f)
            {

                //Destroy the parent
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        else if (_enemyTypesSelected == _enemyTypes.Beta)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (_player != null)
            {
                Vector2 directionToFace = (_player.transform.position - transform.position).normalized;
                Vector2 moveDirection = directionToFace * _speed/2;
                _rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
                 Destroy(this.gameObject, 3f); // destroy the laser after 3 sec
            }
           
            //if laser poistion <= -3 on y, destroy the laser..
            if (transform.position.y <= -3.0f)
            {
                //Destroy the parent
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        else if (_enemyTypesSelected == _enemyTypes.SmartEnemy)
        {
            _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (_enemy != null && _player != null)
            {

                if (_enemy.transform.position.y > _player.transform.position.y)
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                }
                else
                {
                    //transform.Translate(Vector3.up * _speed * Time.deltaTime);
                    Vector2 directionToFace = (_player.transform.position - transform.position).normalized;
                    Vector2 moveDirection = directionToFace * _speed / 2;
                    _rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
                    Destroy(this.gameObject, 3f); // destroy the laser after 3 sec
                }

                //if laser poistion <= -6 on y, destroy the laser..
                if (transform.position.y <= -6.0f)
                {

                    //Destroy the parent
                    if (transform.parent != null)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                    Destroy(this.gameObject);
                }
            }

            
        }

    }

    public void AssignEnemyLaser(string enemyTypes)
    {
        _isEnemyLaser = true;
        _enemyTypesSelected = (_enemyTypes)Enum.Parse(typeof(_enemyTypes), enemyTypes);
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
