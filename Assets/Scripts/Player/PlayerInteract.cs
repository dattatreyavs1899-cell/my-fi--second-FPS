using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCamera;
    public InputActionReference interactAction;
    public float interactRange = 3f;

    void OnEnable() { interactAction.action.Enable(); }
    void OnDisable() { interactAction.action.Disable(); }

    void Update()
    {
        if (interactAction.action.WasPressedThisFrame())
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                SupplyCrate crate = hit.transform.GetComponent<SupplyCrate>();
                if (crate != null)
                {
                    crate.GiveReward(GetComponent<PlayerHealth>(), GetComponent<PlayerCombat>());
                }
            }
        }
    }
}