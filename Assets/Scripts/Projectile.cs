using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool collided;
   void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player" && !collided)
        {
            collided = true;
            Destroy(gameObject);
        }
    }
}
