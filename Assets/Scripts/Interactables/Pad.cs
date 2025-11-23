using UnityEngine;

public class Pad : Interactable
{
    [SerializeField]
    private GameObject pad;
    private bool swtch;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        swtch = !swtch;
       pad.GetComponent<Animator>().SetBool("switch", swtch);
        Debug.Log("Interacted with " + gameObject.name);
    }

}
