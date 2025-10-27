using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private float delayBeforeRestart = 25f;

    private bool hasLoaded = false;

    void Start()
    {
        StartCoroutine(ReturnToMainSceneAfterDelay());
    }

    void Update()
    {
        if (!hasLoaded && Input.GetKeyDown(KeyCode.Return)) // Return is the Enter key
        {
            LoadMainMenu();
        }
    }

    private System.Collections.IEnumerator ReturnToMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeRestart);
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        hasLoaded = true;
        SceneManager.LoadScene("MainMenu");
    }
}
