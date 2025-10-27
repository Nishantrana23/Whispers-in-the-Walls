using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KillPlayer : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "GameOverScene"; // Set this in Inspector

    [Header("Effects")]
    public float delay = 0.5f;           // Delay before scene loads
    public GameObject fadeout;           // Assign your fadeout UI object in Inspector

    private bool hasDied = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.name); // Log what was touched

        // Check if we collided with the player and haven't already triggered death
        if (!hasDied && other.CompareTag("Player"))
        {
            hasDied = true;
            Debug.Log("Player hit! Starting death sequence.");

            if (fadeout != null)
            {
                fadeout.SetActive(true); // Enable fadeout effect
            }

            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }
}
