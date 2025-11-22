using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    protected GameObject player;
    protected Rigidbody enemyRb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        //Gets distance between enemy and player and moves towards player
        //normalized makes the vector return a conistent direction rather than continually add force depending on distance
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce( lookDirection * speed * Time.deltaTime);
    }
}
