using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    Player _player;

    Animator _anim;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player cannot be null");
        }

        if (_anim == null)
        {
            Debug.LogError("Animator cannot be null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source cannot be null on Enemy");
        }
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
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();
            Destroy(this.gameObject,2.4f);
        }
        //if Other is laser
        //destroy the laser and then destrot the enemy - ( us )
        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);

            //Random Score Points and Call AddScore Method on Enemy using player object.
            int randScorePoints = Random.Range(1, 30);
            _player.AddScore(10);

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();
            Destroy(this.gameObject,2.4f);
        }
        Debug.Log("Collide With " + other.transform.name);
    }
}
