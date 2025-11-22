using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    private Transform target;
    private float speed = 10f;
    private bool homing;

    private float rocketStrength = 5.0f;
    private float aliveTimer = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if(homing && target != null)
        {
            //Gets the direction from the rocket to the target
            Vector3 direction = (target.position - transform.position).normalized;
            //Moves the rocket in that direction
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        Destroy(gameObject, aliveTimer);
    }

    //adding force to whatever is hit
    void OnCollisionEnter(Collision col)
    {
        if (target != null)
        {
            //If whatever object we hit has the same tag as the target
            //This is to avoid hitting other objects that are not the intended target
            if (col.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
                //gets the direction away from the collision point of rocket
                Vector3 away = -col.contacts[0].normal;
                //adds force to that away direction
                targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
