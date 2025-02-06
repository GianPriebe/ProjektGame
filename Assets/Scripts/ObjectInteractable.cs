using System;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    [SerializeField] public string typeForInteract;
    [SerializeField] public bool inPlayer;
    [SerializeField] public GameObject itemPointSticked;
    [SerializeField] public int numberSlot = 0; //If this number is zero, it means that is not in any slot
    public GameObject uiPlayer;
    public LayerMask layerGround;
    private Renderer cruxi2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //typeForInteract = "CommomItem";
        typeForInteract = "Seat";
        //Debug.Log(GetInstanceID());
        //Transform cruxi2Temp = transform.Find("Cruxifice (1)");
        //cruxi2 = cruxi2Temp.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (typeForInteract == "CommomItem")
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
            {
                transform.position = new Vector3(transform.position.x, hit.transform.position.y+transform.localScale.y/2, transform.position.z);
            }
            if (inPlayer)
            {
                PlayerInventory uiPlayerComponent = uiPlayer.GetComponent<PlayerInventory>();
                int itemSelected = uiPlayerComponent.itemSelectedInventory;
                transform.position = itemPointSticked.transform.position;
                transform.rotation = Quaternion.Euler(itemPointSticked.transform.rotation.eulerAngles.x-75f, itemPointSticked.transform.rotation.eulerAngles.y-0f, itemPointSticked.transform.rotation.eulerAngles.z+50f);
                if (numberSlot == itemSelected) { gameObject.GetComponent<Renderer>().enabled = true; cruxi2.enabled = true; } else { gameObject.GetComponent<Renderer>().enabled = false; cruxi2.enabled = false; }
            } else {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                gameObject.GetComponent<Renderer>().enabled = true;
                cruxi2.enabled = true;
            }
        }
    }
}
