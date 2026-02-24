using UnityEngine;

public class Target : MonoBehaviour
{
    public float HP = 50f;

    public void TakeDamge (float amount)
    {
       HP -= amount;
        if (HP <= 0f )
        {
            Death();
        }

    }
    void Death()
    {
        Destroy(gameObject);
    }

}
