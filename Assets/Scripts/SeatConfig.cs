using UnityEngine;

public class SeatConfig : MonoBehaviour
{
    public bool used = false;
    public GameObject playerSitting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (used && playerSitting != null)
        {
            PlayerMotor playerMotor = playerSitting.GetComponent<PlayerMotor>();
            playerMotor.inSitting = true;
        }
    }
}
