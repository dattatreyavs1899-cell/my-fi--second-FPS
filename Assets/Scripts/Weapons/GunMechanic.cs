using UnityEngine;

public class GunMechanic : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.2f;
    public float range = 100f;

    [Header("References")]
    public Camera playerCamera;
    public InputManager inputManager;
    
    private float nextFireTime = 0f;

    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.onFoot.Shoot.triggered)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Interactable2 interactable2 = hit.collider.GetComponent<Interactable2>();
            if (interactable2 != null)
            {
                interactable2.Interact();
            }
        }
    }
}
