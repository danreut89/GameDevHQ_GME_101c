using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 3.5f;
    [SerializeField]
    private float _boostSpeed = 2.0f;
    [SerializeField]
    private float _boostSpeedNumebr = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShootPrefab;
    [SerializeField]
    private float _laserOffset = 0.8f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private float _horizontalValue,_verticalValue;
    private bool _isTripleShootActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisual;
    private UIManager _uimanager;
    [SerializeField]
    private GameObject[] _damageFire;
    [SerializeField]
    private AudioClip _audioShoot;
    [SerializeField]
    private AudioClip _audioExpolosion;
    private AudioSource _audioSoruce;
    [SerializeField]
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uimanager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _audioSoruce = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uimanager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSoruce == null)
        {
            Debug.LogError("The Audio Source Component is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _boostSpeed;
        } else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= _boostSpeed;
        }
    }

    void CalculateMovement()
    {
        _horizontalValue = Input.GetAxis("Horizontal");
        _verticalValue = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * _horizontalValue * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * _verticalValue * _speed * Time.deltaTime);
        
        transform.position = new Vector3 (transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
       
        if (_isTripleShootActive)
        {
            Instantiate(_tripleShootPrefab,transform.position, Quaternion.identity);
                    
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }

        _audioSoruce.clip = _audioShoot;
        _audioSoruce.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }

        _lives -= 1;

        if (_lives == 2)
        {
            _damageFire[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            _damageFire[1].SetActive(true);
        }

        _uimanager.UpdateLeives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uimanager.TurnGameOverScreen();
            _audioSoruce.clip = _audioExpolosion;
            _audioSoruce.Play();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser")
        {
            Damage();
        }
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
    }
    
    public void TripleShotActive()
    {
        _isTripleShootActive = true;
        StartCoroutine(TripleShotCoolDown());
    }

    IEnumerator TripleShotCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShootActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed += _boostSpeedNumebr;
        StartCoroutine(SppedBoostCoolDown());
    }

    IEnumerator SppedBoostCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5.0f;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uimanager.UpdateScore(_score);
    }

}
