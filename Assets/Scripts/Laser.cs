using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed of the laser
    [SerializeField]
    private float _speed = 8.0f;

    private bool _isEnemyLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else if (_isEnemyLaser)
        {
            MoveDown();
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
        //Move Downward -> translate down..
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

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
       
    }
}
