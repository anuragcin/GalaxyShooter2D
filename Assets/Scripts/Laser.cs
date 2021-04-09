using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed of the laser
    [SerializeField]
    private float _speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
}
