using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;
    [SerializeField]
    private bool _enemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (_enemyLaser == false)
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

            if (transform.position.y > 8)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        else if (_enemyLaser == true)
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

            if (transform.position.y < -8)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
