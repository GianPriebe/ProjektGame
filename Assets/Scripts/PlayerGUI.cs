using TMPro;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    //private float healthPoints = 10f;
    public GameObject player;
    private PlayerMotorConfig playerConfig;
    public TextMeshProUGUI staminaText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerConfig = player.GetComponent<PlayerMotorConfig>();
    }

    // Update is called once per frame

    private void Update() {
        staminaText.text = "Stamina = "+ playerConfig.StaminaPoints;
    }
    void OnGUI()
    {
        GUI.color = Color.red;
        Rect healthRect = new Rect(10, 10, 200, 100);
        //GUI.Box(healthRect, "oi");
    }
}
