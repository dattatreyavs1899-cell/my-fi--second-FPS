using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;

    private InputManager inputManager;

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Interactable2 interactable2 = hit.collider.GetComponent<Interactable2>();

            if (interactable2 != null)
            {
                playerUI.UpdateText("Press E to interact");
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable2.Interact();
                }
            }
        }
    }
}
