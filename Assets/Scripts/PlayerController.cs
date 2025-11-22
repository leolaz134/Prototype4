using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float pushbackStrength = 15.0f;
    private float pushbackDuration = 7f;
    private float rocketFireRate = 0.5f;
    private float rocketPowerUpDuration = 5.0f;
    private float smashPowerUpDuration = 3.0f;
    public float speed = 5.0f;
    public bool hasPowerUp = false;
    public GameObject powerUpIndicatorPushback;
    public GameObject powerUpIndicatorRockets;
    public GameObject powerUpIndicatorSmash;

    public PowerUpType currentPowerUp = PowerUpType.None;

    public GameObject rocketPrefab;
    private GameObject tmpRocket;

    //Smash power variables
    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;

    float floorY;
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forawrdInput = Input.GetAxis("Vertical");
        //goes forward with respect to the focal point
        playerRb.AddForce(focalPoint.transform.forward * forawrdInput * speed * Time.deltaTime);

        //Gets locations for each power up indicator
        powerUpIndicatorPushback.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerUpIndicatorRockets.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerUpIndicatorSmash.transform.position = transform.position + new Vector3(0, 3f, 0);
    }

    //BUG: if you pick up a powerup while having one, the previous one doesn't get cleared properly
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            Destroy(other.gameObject);
            if (currentPowerUp == PowerUpType.Pushback)
            {
                powerUpIndicatorPushback.SetActive(true);
                StartCoroutine(PushbackRoutine());
            }
            else if (currentPowerUp == PowerUpType.Rockets)
            {
                powerUpIndicatorRockets.SetActive(true);
                StartCoroutine(LaunchRocketsRoutine());
            }
            else if (currentPowerUp == PowerUpType.Smash)
            {
                powerUpIndicatorSmash.SetActive(true);
                StartCoroutine(SmashRoutine());
            }

        }


    }

    IEnumerator PushbackRoutine()
    {
        yield return new WaitForSeconds(pushbackDuration);
        powerUpIndicatorPushback.SetActive(false);
        currentPowerUp = PowerUpType.None;
        hasPowerUp = false;
        Debug.Log("Pushback powerup ended");
    }

    IEnumerator LaunchRocketsRoutine()
    {
        float elapsedTime = 0f;
        //This checks the time passed in the coroutine against the duration we want
        //and adds each interval of rocket fire rate to the elapsed time
        while (elapsedTime < rocketPowerUpDuration)
        {
            LaunchRockets();
            yield return new WaitForSeconds(rocketFireRate);
            elapsedTime += rocketFireRate;
        }
        powerUpIndicatorRockets.SetActive(false);
        currentPowerUp = PowerUpType.None;
        hasPowerUp = false;
        Debug.Log("Rockets powerup ended");
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        //Store y position before taking off
        floorY = transform.position.y;

        //Calculate time to reach apex
        float jumpTime = Time.time + hangTime;

        while(Time.time < jumpTime)
        {
            //move the player up while still keeping z and x velocity
            playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, smashSpeed, playerRb.linearVelocity.z);
            yield return null;
        }

        //Now move the player down
        while (transform.position.y > floorY)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, -smashSpeed * 2);
            yield return null;
        }

        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            }
        }
    }

    IEnumerator SmashRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < smashPowerUpDuration)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yield return StartCoroutine(Smash());
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        powerUpIndicatorSmash.SetActive(false);
        currentPowerUp = PowerUpType.None;
        hasPowerUp = false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidBody.AddForce(awayFromPlayer * pushbackStrength, ForceMode.Impulse);
        }
    }

    void LaunchRockets()
    {
        //This is a for loop to go through all enemies
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            //Spawn the rocket and use the fire method to send it to the enemy
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);
        }
    }
}
