using UnityEngine;
using UnityEngine.UI;

public class MainQuest : MonoBehaviour {

    public static MainQuest Instance;

    [SerializeField]
    private Text text;

    public int ActiveZombieSpawners { get; set; }
    public int MaxZombieSpawners { get; set; }

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError($"There can be only one instance of {ToString()} script!");
            Destroy(this);
        }

        ActiveZombieSpawners = 0;
        MaxZombieSpawners = 5;
        UpdateQuestText();
        Instance = this;
    }

    public void UpdateQuestText() {
        if (text != null)
            text.text = $"Zniszcz wszystki gniazda potworów (pozostało {ActiveZombieSpawners}).";
    }

}
