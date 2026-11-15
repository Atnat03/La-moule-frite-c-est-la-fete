using MyPrint;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceForce = 8f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        if (rb == null)
            return;

        ABPrint.Print("Collision détectée !");

        rb.constraints = RigidbodyConstraints.None;
        
        Vector3 bounceDirection = transform.up;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
    }
}