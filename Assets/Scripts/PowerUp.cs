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
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _audioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at a speed of 3
        //when we leave the screen, destroy this object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.97)
        {
            Destroy(this.gameObject);
        }
        
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
                    default:
                        Debug.Log("Default..");
                        break;
                }
            }
                

            Destroy(this.gameObject);
        }
    }
}
