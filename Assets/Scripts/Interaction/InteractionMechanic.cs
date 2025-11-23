using UnityEngine;

public class InteractionMechanic : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public KeyCode interactKey = KeyCode.E;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
    }
}
