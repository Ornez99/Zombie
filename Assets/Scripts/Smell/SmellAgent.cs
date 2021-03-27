using UnityEngine;

public class SmellAgent : MonoBehaviour {

    [SerializeField]
    private int smellValue = 0;

    private void Start() {
        SmellManager.Instance.AddAgent(this);
    }

    public void UpdateSmell() {
        int currentX = Mathf.FloorToInt(transform.position.x);
        int currentZ = Mathf.FloorToInt(transform.position.z);
        if (SmellManager.Instance.SmellMap[currentX, currentZ] < smellValue)
            SmellManager.Instance.SmellMap[currentX, currentZ] = smellValue;
    }
}
