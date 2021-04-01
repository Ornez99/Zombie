using UnityEngine;
using UnityEngine.UI;

public class MainQuest : MonoBehaviour {

    public static MainQuest Instance;

    [SerializeField]
    private Text text;
    [SerializeField]
    private Text endGameText;
    [SerializeField]
    private GameObject endGameGameObject;

    public int ActiveZombieSpawners { get; set; }
    public int MaxZombieSpawners { get; set; }

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError($"There can be only one instance of {ToString()} script!");
            Destroy(this);
        }

        ActiveZombieSpawners = 0;
        MaxZombieSpawners = 5;
        Instance = this;
    }

    public void UpdateQuest() {
        if (text != null)
            text.text = $"Zniszcz wszystki gniazda potworów (pozostało {ActiveZombieSpawners}).";

        if (ActiveZombieSpawners == 0) {
            endGameText.text = "Gratulacje! Wygrałeś.";
            endGameGameObject.SetActive(true);
        }
    }

    public void QuestLost() {
        endGameText.text = "Przegrałeś :(";
        endGameGameObject.SetActive(true);
    }

}
