using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    [SerializeField]
    public bool canMove;

    
    public float speed;
    public int startPoint;
    [SerializeField] Transform[] points;

    int i;
    float j;
    bool reverse;
    

    void Start()
    {
        
        transform.position = points[startPoint].position;
        i = startPoint;
        j = startPoint;
    }

    void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, points[i].position) < 0.01f)
        {
            canMove = false;
            if (i == points.Length - 1)
            {
                reverse = true;
                i--;
                return;
            }

            else if (i == 0)
            {
                reverse = false;
                i++;
                return;
            }

            if (reverse)
            {
                i--;
            }
            else
            {
                i++;
            }
            j = i;

        }

        if (canMove)
        {
            float timed = speed * Time.deltaTime;
           // timed *= timed;
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, timed);
        }

    }

    public void Move()
    {
        canMove = true;
    }
}
