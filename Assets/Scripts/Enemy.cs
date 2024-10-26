using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // transform.position = new Vector3(0,0,0);
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.7f) {
            transform.position = new Vector3(Random.Range(-8, 8),7.6f,0);
        }


    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Hit: " + other.transform.name);
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) {
                player.Damage();
            }
            
            Destroy(this.gameObject);
        }


        if(other.tag == "Laser") {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(this.gameObject);
        }
    }
}
