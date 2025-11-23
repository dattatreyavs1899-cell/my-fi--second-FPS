using System.Collections.Generic;
using UnityEngine;

public class KeypadInteractable : Interactable2
{
    [SerializeField] private GameObject door;
    private bool doorOpen;

    [SerializeField] private Color newColour = Color.greenYellow;
    [SerializeField] private List<GameObject> doorObjects;

    public override void Interact()
    {
        ChangeDoorColour();
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        Debug.Log("Interacted with " + gameObject.name);
    }

    private void ChangeDoorColour()
    {
        foreach (GameObject obj in doorObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColour;
            }
        }
    }
}
