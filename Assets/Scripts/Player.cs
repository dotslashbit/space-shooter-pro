using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;


    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private float _speedMultiplier = 2; 

    [SerializeField]
    private GameObject _shieldVisualizer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // take the current position  = new position (0, 0, 0)
       transform.position = new Vector3(0, 0, 0);
       _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

       if (_spawnManager == null) {
        Debug.LogError("The spawn manager is null");
       }
       
       
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) {
            FireLaser();
        }

        
    }


    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        

        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f) {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        } else if (transform.position.x < -11.3f) {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser() {
        
        // Debug.Log("Space key pressed");
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive) {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0,1.05f,0), Quaternion.identity);
        } else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0,1.05f,0), Quaternion.identity);
        }

        
    
    }

    public void Damage() {
        if (_isShieldsActive == true) {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }


        _lives -= 1;

        if (_lives < 1) {

            // communiate with spawn manager
            _spawnManager.onPlayerDeath();

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive() {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
        
    }

    IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive() {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBostPowerDownRoutine());
    }

    IEnumerator SpeedBostPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive() {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }
}
