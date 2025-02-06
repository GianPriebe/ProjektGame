using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarMotor : MonoBehaviour
{
    //public Vector3 LastPosition;
    public Transform _transform;
    public Dictionary<int, Transform> rbSeats = new Dictionary<int, Transform>();
    private Transform rbSeatChoosed;
    public Transform seatsGroup;
    public Transform rbSeat_1;
    public Transform rbSeat_2;
    public Transform rbSeat_3;
    public Transform rbSeat_4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _transform = transform;
        rbSeats.Add(1, rbSeat_1);
        rbSeats.Add(2, rbSeat_2);
        rbSeats.Add(3, rbSeat_3);
        rbSeats.Add(4, rbSeat_4);
    }
    // Update is called once per frame
    void Update()
    {
        ///Debug.Log(rbSeats[1].name);oi
        for (int i=1;i<=4;i++)
        {
            rbSeatChoosed = rbSeats[i];
            SeatConfig seatScript = rbSeatChoosed.gameObject.GetComponent<SeatConfig>();
            if (seatScript.used && seatScript.playerSitting != null)
            {
                seatScript.playerSitting.transform.position = rbSeatChoosed.position;
            }
        }
    }
}
