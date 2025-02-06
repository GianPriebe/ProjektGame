using System;
using UnityEngine;

public class ThingInteractable : MonoBehaviour
{
    [SerializeField] public string typeForInteract;
    public GameObject uiPlayer;
    public LayerMask layerGround;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        typeForInteract = "SeatThing";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
