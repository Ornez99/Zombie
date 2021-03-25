using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public void LoadNewScene(string sceneName) {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
