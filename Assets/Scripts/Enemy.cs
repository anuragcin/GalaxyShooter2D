using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField]
    private GameObject _ShieldVisualer;

    bool _isEnemyVisualized = false;

    int _shiledStrength = 2;
    float _accuracy = 1.0f;

    bool flag;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (_waveConfig != null)
            {
                _wayPoints = _waveConfig.GetWayPoints();
                if (_wayPoints.Count == 0)
                {
                    Debug.LogError("Waypoints cannot be zero.");
                }
            }
            else
            {
                Debug.LogError("WebConfig cannot be null");
            }


            _player = GameObject.FindWithTag("Player").GetComponent<Player>();

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
        catch(NullReferenceException ex)
        {
            Debug.Log($"Object was not set in the inspector {ex.Message.ToString()} ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        NewEnemyMovement();

        //CaculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2f, 5f);
            _canFire = Time.time + _fireRate;

            if (_waveConfig != null)
            {
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser(_waveConfig.GetEnemyTypes());
                }
            }
        }

    }

    private void NewEnemyMovement()
    {
        if(_wayPointIndex <= _wayPoints.Count-1)
        {
            Vector3 targetPosition;
            var moveThisFrame = _waveConfig.GetMoveSpeed() * Time.deltaTime;
            if (_waveConfig.GetEnemyTypes() == "Alpha")
            {
                targetPosition = _player.transform.position;
                Vector2 direction = targetPosition - this.transform.position;
                Debug.DrawRay(this.transform.position, direction, Color.red);
               

                if (direction.magnitude < 9.0f)
                {
                    Debug.Log("Enemy when around 0.9 " + direction.magnitude);
                    transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, moveThisFrame);

                    
                    if (direction.magnitude > 0.5f)
                    {
                        Debug.Log("Enemy When around 0.7 " + direction.magnitude);
                        Destroy(gameObject,3f);
                    }
                }
            }
            else
            {
                targetPosition = _wayPoints[_wayPointIndex].transform.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveThisFrame);
            }
            

            //Randomly  0.1 percentage changes enemy get visualized..

            float proabilityValue = Random.value;

            if (proabilityValue > 0.99) //0.1 percentage
            {
                _isEnemyVisualized = true;
                _ShieldVisualer.SetActive(true);
            }
            if (transform.position == targetPosition)
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
                Player player = other.transform.GetComponent<Player>();
                {
                    player.Damage();
                }
                if (_isEnemyVisualized)
                {
                    _shiledStrength--;

                    switch (_shiledStrength)
                    {
                        case 1:
                            _ShieldVisualer.GetComponent<Renderer>().material.color = Color.red;
                            _ShieldVisualer.SetActive(false);
                            break;
                        case 0:
                            _anim.SetTrigger("OnEnemyDeath");
                            _speed = 0;

                            _audioSource.Play();
                            Destroy(this.gameObject, 2.4f);
                            break;
                        default:
                            break;

                    }
                }
            }
        //if Other is laser
        //destroy the laser and then destrot the enemy - ( us )
        if (other.tag == "Laser")
        {
            if (_isEnemyVisualized)
            {
                _shiledStrength--;
                Destroy(other.gameObject);

                //Random Score Points and Call AddScore Method on Enemy using player object.
                int randScorePoints = Random.Range(1, 30);
                _player.AddScore(10);

                switch (_shiledStrength)
                {
                    case 1:
                        _ShieldVisualer.GetComponent<Renderer>().material.color = Color.red;
                        _ShieldVisualer.SetActive(false);
                        break;
                    case 0:
                        _anim.SetTrigger("OnEnemyDeath");
                        _speed = 0;

                        _audioSource.Play();

                        Destroy(GetComponent<Collider2D>());
                        Destroy(this.gameObject, 2.4f);
                        break;
                    default:
                        break;
                }
            }
            Debug.Log("Collide With " + other.transform.name);
        }
        
    }
}
