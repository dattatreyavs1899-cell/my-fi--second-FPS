using UnityEngine;

public class ParentPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRB != null)
        {
            collision.transform.SetParent(null);
        }
    }
}
