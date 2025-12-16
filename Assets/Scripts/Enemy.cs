using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    private float _randomXpostion;

    private Player _player;
    private UIManager _uiManager;

    private Animator _anim;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    float _randomShootRange; 


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _anim = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiManager == null)
        {
            Debug.LogError("UI_Manager was not found");
        }

        if (_player == null)
        {
            Debug.LogError("Player was not found");
        }

        if (_anim == null)
        {
            Debug.LogError("Anim Component was not found");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source Component was not found");
        }

        StartCoroutine(LaserEnemyShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            _randomXpostion = Random.Range(-11.0f, 11.0f);
            transform.position = new Vector3(_randomXpostion, 8, 0);
        }
    }

    IEnumerator LaserEnemyShootRoutine()
    {
        _randomShootRange = Random.Range(3.0f, 5.0f);

        while (true)
        {
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_randomShootRange);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("onEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(gameObject, 2.8f);   
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            _anim.SetTrigger("onEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }
}
