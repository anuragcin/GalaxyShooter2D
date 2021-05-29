using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private float _baseSpeed = 3.5f;
    
    [SerializeField]
    private float _speedMultipler = 2.0f;

    [SerializeField]
    private float _increasedRate = 5.0f;

    private int _shieldStrength;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _secMissilePrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    public bool _isSpeedPowerUpActive = false;
    private bool _isShieldPowerUpActive = false;
    public bool _isAmmoPowerUpActive = false;
    public bool _isHealthPowerUpActive = false;
    public bool _isSecondaryPowerUpActive = false;
    public bool _isNegativePowerUpActive = false;

    [SerializeField]
    private GameObject _ShieldVisualer;

    [SerializeField]
    private GameObject _rightEngine,_leftEngine;


    [SerializeField]
    private int _score;

    [SerializeField]
    private int _ammoCount;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserAudioClip;

    private AudioSource _audioSource;

    private bool _isThrusterActive = false;
    private bool _isThrusterDowm = false;

    private float _thrusterFullScale = 10f;
    private float _thrusterElaspedTime = 0;




    // Start is called before the first frame update
    void Start()
    {
        //find the GameObject and get the Component
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _baseSpeed = 3.5f;
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager cannot be null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager cannot be null");
        }
        else
        {
            _ammoCount = _uiManager.TotalAmmoCount();
            _uiManager.UpdateThrusterScale(_thrusterElaspedTime, _thrusterFullScale);
        }

        if (_audioSource==null)
        {
            Debug.LogError("Audio Source cannot be null on player");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }


    }

    // Update is called once per  frame i e., 60 frames/sec
    void Update()
    {
        CaclulateMovement();    

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount>0)
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
            AmmoFire(3); //three lasers shot
        }
        else if (_isSecondaryPowerUpActive)
        {
            Instantiate(_secMissilePrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
            //AmmoFire(1); //one lasers shot
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
            AmmoFire(1); //one laser shot
        }

        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    private void CaclulateMovement()
    {
        //Movement using horizontal
        float horizontalInput = Input.GetAxis("Horizontal");

        //Movement using Vertical
        float verticalInput = Input.GetAxis("Vertical");
    
        //Creating variable direction of type Vector3
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //When LeftShift key is pressed call ActivateThruster method
        //else when release called DeactivateThruster method
        if (Input.GetKeyDown(KeyCode.LeftShift) && (!_isThrusterDowm))
        {
            ActivateThruster();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DeactivateThruster();
        }

        UpdateThrusterScale(); 

        transform.Translate(direction * _speed * Time.deltaTime);
        
        

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

    private void UpdateThrusterScale()
    {
        if (_isThrusterActive)
        {
            _thrusterElaspedTime = _thrusterElaspedTime + Time.deltaTime;
            if (_thrusterElaspedTime > _thrusterFullScale)
            {
                _thrusterElaspedTime = _thrusterFullScale;
                _speed = _baseSpeed;
                _isThrusterDowm = true;
                _isThrusterActive = false;
            }
            _uiManager.UpdateThrusterScale(_thrusterElaspedTime, _thrusterFullScale);
        }
        else if (_isThrusterActive == false && _thrusterElaspedTime > 0)
        {
            _thrusterElaspedTime = _thrusterElaspedTime - Time.deltaTime;
            if (_thrusterElaspedTime < 0)
            {
                _thrusterElaspedTime = 0;
                _isThrusterDowm = false;
      
            }
            _uiManager.UpdateThrusterScale(_thrusterElaspedTime, _thrusterFullScale);
        }
    }

    /// <summary>
    /// Damaging the Player Lives.
    /// </summary>
    public void Damage()
    {
        if (_isShieldPowerUpActive)
        {
            _shieldStrength--;

            switch (_shieldStrength)
            {
                case 2:
                    _ShieldVisualer.GetComponent<Renderer>().material.color = Color.green;
                    break;
                case 1:
                    _ShieldVisualer.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 0:
                    _isShieldPowerUpActive = false;
                    _ShieldVisualer.SetActive(false);
                    break;
                default:
                    break;
            }
            return;
        }
        _lives--;

        Debug.Log("Player lives"+ _lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        //Update Lives on UI Display
        _uiManager.UpdateLives(_lives);

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

    public void SecondaryPowerUpActive()
    {
        _isSecondaryPowerUpActive = true;

        //Starts the coroutune
        StartCoroutine(SecondaryPowerDownRoutine());

    }


    IEnumerator SecondaryPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSecondaryPowerUpActive = false;
    }

    public void SpeedPowerUpActive()
    {
        _isSpeedPowerUpActive = true;
        _speed *= _speedMultipler;
        //Starts the coroutune
        StartCoroutine(SpeedPowerUpDownRoutine());

    }

    IEnumerator SpeedPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedPowerUpActive = false;
        _speed /= _speedMultipler;
    }

    public void ShieldPowerUpActive()
    {
        _isShieldPowerUpActive = true;
        _shieldStrength = 3;
        _ShieldVisualer.GetComponent<Renderer>().material.color = Color.gray;
        _ShieldVisualer.SetActive(true);
        
        
    }

    /// <summary>
    /// Update the Ammo Count back to 15 
    /// </summary>
    public void AmmoPowerUpActive()
    {
        _isAmmoPowerUpActive = true;
        _ammoCount = 100;
        _uiManager.UpdateAmmoCount(_ammoCount);
    }

    /// <summary>
    /// Update the health of the player
    /// </summary>
    public void HealthPowerUpActive()
    {
        _isHealthPowerUpActive = true;
        if (_lives < 3)
        {
            _lives++;
            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(false);
            }
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }
            else
            {
                _leftEngine.SetActive(false);
                _rightEngine.SetActive(false);
            }
            _uiManager.UpdateLives(_lives);
        }
        
    }

    /// <summary>
    /// Negative PowerUp
    /// </summary>
    public void NegativePowerUpActive()
    {
        _isNegativePowerUpActive = true;
        _speed = _baseSpeed / 2;
        //Starts the coroutune
        StartCoroutine(NegativePowerUpDownRoutine());
    }

    IEnumerator NegativePowerUpDownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        _isNegativePowerUpActive = false;
        _speed = _baseSpeed;
    }
    /// <summary>
    /// Add Score
    /// </summary>
    /// <param name="points"></param>
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    /// <summary>
    /// Increase the speed with increaseRate
    /// </summary>
    public void ActivateThruster()
    {
        _speed *= _increasedRate;
        _isThrusterActive = true;


    }

    /// <summary>
    /// Reduce back to the normal Speed 
    /// </summary>
    public void DeactivateThruster()
    {

        _speed = _baseSpeed;
        _isThrusterActive = false;
    }
    /// <summary>
    /// Ammo Fire and updated ammocount to uiManager
    /// </summary>
    /// <param name="LaserCount"></param>
    public void AmmoFire(int LaserCount)
    {
        _ammoCount = _ammoCount - LaserCount;
        _uiManager.UpdateAmmoCount(_ammoCount);
       
    }

} 
