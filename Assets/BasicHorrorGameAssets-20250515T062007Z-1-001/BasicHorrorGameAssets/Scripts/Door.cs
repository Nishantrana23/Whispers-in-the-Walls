using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject handUI;
    public GameObject UIText;
    public GameObject invKey;
    public GameObject fadeFX;

    public string nextSceneName; // Name of the next scene to load

    private bool inReach = false;

    void Start()
    {
        if (handUI) handUI.SetActive(false);
        if (UIText) UIText.SetActive(false);
        if (invKey) invKey.SetActive(false);
        if (fadeFX) fadeFX.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = true;
            if (handUI) handUI.SetActive(true);
            Debug.Log("Player is in reach of the door.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = false;
            if (handUI) handUI.SetActive(false);
            if (UIText) UIText.SetActive(false);
            Debug.Log("Player left the door area.");
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (invKey != null && invKey.activeInHierarchy)
            {
                Debug.Log("Player has the key. Opening door...");
                if (handUI) handUI.SetActive(false);
                if (UIText) UIText.SetActive(false);
                if (fadeFX) fadeFX.SetActive(true);
                StartCoroutine(ending());
            }
            else
            {
                Debug.Log("Player does not have the key.");
                if (UIText) UIText.SetActive(true);
            }
        }
    }

    IEnumerator ending()
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(nextSceneName);
    }
}
