using UnityEngine;



public class FPShooterScript : MonoBehaviour
{
    public Camera Camera;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30;
    public float arcRange = 1f;
    public LayerMask raycastLayers;

    private InputManager inputManager;
    private Vector3 destination;
    void Start()
    {
        inputManager = GetComponent<InputManager>();

    }

    
    void Update()
    {
        
        if (inputManager.onFoot.Shoot.triggered)
        {
            ShootProjectile();
        }

    }

    void ShootProjectile()
    {
        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayers))
        { destination = hit.point;

            if (hit.collider.CompareTag("target"))
            {
                ScoreManager.Instance.AddScore(10);
            }

            var interact = hit.collider.GetComponent<Interactable>();
            if (interact != null)
            {
                
                    interact.BaseInteract();
                    
                
            }
        }
        else { destination = ray.GetPoint(1000); }

        InstantiateProjectile();
    }

    void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().linearVelocity = (destination - firePoint.position).normalized * projectileSpeed;

        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 2f));
    
    }

}
