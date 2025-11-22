using UnityEngine;

public class HarderEnemy : Enemy
{
    public float pushForce = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromEnemy =  other.gameObject.transform.position - transform.position;
            playerRigidbody.AddForce(awayFromEnemy.normalized * pushForce, ForceMode.Impulse);
        }
    }
}
