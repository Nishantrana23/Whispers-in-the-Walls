using UnityEngine;

public class UseChest : MonoBehaviour
{
    public GameObject handUI;
    public GameObject objToActivate;

    private bool inReach = false;

    void Start()
    {
        if (handUI != null) handUI.SetActive(false);
        if (objToActivate != null) objToActivate.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        inReach = true;
        if (handUI != null) handUI.SetActive(true);
        Debug.Log("Something entered chest trigger: " + other.name);
    }

    void OnTriggerExit(Collider other)
    {
        inReach = false;
        if (handUI != null) handUI.SetActive(false);
        Debug.Log("Something exited chest trigger: " + other.name);
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed inside trigger.");

            if (handUI != null) handUI.SetActive(false);
            if (objToActivate != null) objToActivate.SetActive(true);

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("open", true);
                Debug.Log("Animator set to open.");
            }
            else
            {
                Debug.LogError("No Animator component found on chest.");
            }

            BoxCollider col = GetComponent<BoxCollider>();
            if (col != null) col.enabled = false;
        }
    }
}
