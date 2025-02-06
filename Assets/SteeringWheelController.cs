using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    public float angleRotate = 0f;
    private GameObject playerGO;
    public GameObject seatGO;
    public SeatConfig seatScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void FixedUpdate()
    {
        seatScript = seatGO.GetComponent<SeatConfig>();
        if (seatScript.playerSitting != null)
        {
            playerGO = seatScript.playerSitting;
            PlayerMotorConfig playerConfig = playerGO.GetComponent<PlayerMotorConfig>();
            if (playerConfig.lastMoveInt != 0f && angleRotate > -45 && angleRotate < 45)
            {
                angleRotate += playerConfig.lastMoveInt;
                transform.Rotate(new Vector3(0f, playerConfig.lastMoveInt, 0f));
            }
            else if (playerConfig.lastMoveInt == 0f && angleRotate <= -1 || angleRotate >= 1)
            {
                transform.Rotate(new Vector3(0f, -Mathf.Sign(angleRotate), 0f));
                angleRotate -= Mathf.Sign(angleRotate);
            }
            Debug.Log("Rotação do volante:"+angleRotate);
        }
    }
}