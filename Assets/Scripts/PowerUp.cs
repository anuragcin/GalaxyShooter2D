using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //IDs System for PowerUp
    //0-  TripleShot
    //1 - Speed
    //2 - Shields
    //3- Ammo
    //4-Health PowerUp
    //5-Sec PowerUp
    //6-Negative PowerUp
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _audioClip;

    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            PickUpMoveToPlayer();
        }
        else
        {
            //move down at a speed of 3
            //when we leave the screen, destroy this object
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -6.97)
        {
            Destroy(this.gameObject);
        }
        
    }

    /// <summary>
    /// Postion the Pickup towards the player..
    /// </summary>
    public void PickUpMoveToPlayer()
    {
        Vector3 direction = (this.transform.position - _player.transform.position).normalized;
        this.transform.position -= direction * Time.deltaTime * (_speed * 2);

    }

    /// <summary>
    /// Only Player Collect TripleShot & show destroy
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            //PowerUp Clip
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        //Triple Shot PowerUp
                        Debug.Log("Triple Shot PowerUp Collected");
                        player.TripleShotActive();
                        break;
                    case 1:
                        //Speed PowerUp
                        player.SpeedPowerUpActive();
                        Debug.Log("Speed PowerUp Collected");
                        break;
                    case 2:
                        // Shield PowerUp
                        player.ShieldPowerUpActive();
                        Debug.Log("Shield PowerUp Collected");
                        break;
                    case 3:
                        // Ammo PowerUp
                        player.AmmoPowerUpActive();
                        Debug.Log("Ammo PowerUp Collected");
                        break;
                    case 4:
                        // Health PowerUp
                        player.HealthPowerUpActive();
                        Debug.Log("Health PowerUp Collected");
                        break;
                    case 5:
                        // Secondary PowerUp
                        player.SecondaryPowerUpActive();
                        Debug.Log("Secondary PowerUp Collected");
                        break;
                    case 6:
                        // Negative PowerUp
                        player.NegativePowerUpActive();
                        Debug.Log("Negative PowerUp Collected");
                        break;
                    default:
                        Debug.Log("Default..");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
