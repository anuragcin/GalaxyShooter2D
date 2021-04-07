using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //moves down 4 meters per sec.
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if bottom of screen
        if (transform.position.y < -6.4f)
        {
            // re-spawm at top of screen at random x - position
            float posXRandom = Random.Range(-10.0f, 10.0f);
            transform.position = new Vector3(posXRandom , 6.3f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if Other is Player
        //destroy the player and then destroy the enemy- ( us )
        if (other.tag == "Player")
        {
            //Damaging the Player..
            Player player =  other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
        //if Other is laser
        //destroy the laser and then destrot the enemy - ( us )
        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        Debug.Log("Collide With " + other.transform.name);
    }
}
