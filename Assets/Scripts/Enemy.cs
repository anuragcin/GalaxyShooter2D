using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    Player _player;
    Animator _anim;
    AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1;


    private WaveConfig _waveConfig;

    private List<Transform> _wayPoints;
    private int _wayPointIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        if (_waveConfig != null)
        {
           _wayPoints = _waveConfig.GetWayPoints();
            if (_wayPoints.Count > 0)
            {
                //transform.position = _wayPoints[_wayPointIndex].transform.position;
            }
        }
       

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
        NewEnemyMovement();

        //CaculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

    }

    private void NewEnemyMovement()
    {
        if(_wayPointIndex <= _wayPoints.Count-1)
        {
            var targetPosition = _wayPoints[_wayPointIndex].transform.position;
            var moveThisFrame = _waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveThisFrame);

            if(transform.position == targetPosition)
            {
                _wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this._waveConfig = waveConfig;
    }



    private void CaculateMovement()
    {
        //moves down 4 meters per sec.
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if bottom of screen
        if (transform.position.y < -6.4f)
        {
            // re-spawm at top of screen at random x - position
            float posXRandom = Random.Range(-10.0f, 10.0f);
            transform.position = new Vector3(posXRandom, 6.3f, 0);
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

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.4f);
        }
        Debug.Log("Collide With " + other.transform.name);
    }
}
