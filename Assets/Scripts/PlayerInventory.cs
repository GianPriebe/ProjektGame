using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class PlayerInventory : MonoBehaviour
{
    public RectTransform itemSelectedImage;
    public RectTransform itemBarImage1;
    [SerializeField] public int itemSelectedInventory = 1;
    private int maximumCapacityInventory = 5;
    private float startX = -297;
    private float widthItemBarImage;
    public Dictionary<int, int> slots = new Dictionary<int, int>();
    private Dictionary<string, string> textToInteract = new Dictionary<string, string>();
    [SerializeField] private float distanceInteract;
    public Camera mainCamera;
    public Vector3 forwardCamera;
    private GameObject gui;
    private GameObject hotbar;
    private GameObject crosshair;
    private GameObject itemInteractableText;
    private TextMeshProUGUI itemInteractableTextMesh;
    private GameObject itemPoint;    
    private GameObject seatWherePlayerIs = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void CreateVariables()
    {
        gui = transform.gameObject;
        Transform hotbarTemp = gui.transform.Find("Hotbar");
        hotbar = hotbarTemp.gameObject;
        Transform crosshairTemp = gui.transform.Find("Crosshair");
        crosshair = crosshairTemp.gameObject;
        Transform itemInteractableTextTemp = gui.transform.Find("ItemInteractableText");
        itemInteractableText = itemInteractableTextTemp.gameObject;
        itemInteractableTextMesh = itemInteractableText.GetComponent<TextMeshProUGUI>();
        itemPoint = (this.transform.parent).gameObject;
        itemPoint = (itemPoint.transform.parent).gameObject;
        itemPoint = (itemPoint.transform.Find("ItemPoint")).gameObject;
        textToInteract.Add("CommomItem", "Aperte [E] para pegar");
        textToInteract.Add("Seat", "Aperte [E] para sentar");
        distanceInteract = 2.7f;
    }
    void Start()
    {
        CreateVariables();
        widthItemBarImage = itemBarImage1.rect.width;
        slots.Add(1, 0);
        slots.Add(2, 0);
        slots.Add(3, 0);
        slots.Add(4, 0);
        slots.Add(5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float scrollMouse = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scrollMouse > 0.0f && itemSelectedInventory < maximumCapacityInventory) itemSelectedInventory++;
        else if (scrollMouse < 0.0f && itemSelectedInventory > 1f) itemSelectedInventory--;
        itemSelectedImage.anchoredPosition3D = new Vector3(startX+((1.35f*widthItemBarImage)*(itemSelectedInventory-1)), -330f, itemSelectedImage.position.z);
        //itemSelectedImage.position = new Vector3(
        //Debug.Log(itemSelectedImage.position);
        foreach (KeyValuePair<int, int> slotN in slots)
        {
            int slotNum = slotN.Key;
            if (slotN.Value != 0) {
                SetTextAndImageItemInventory(slots, slotNum);
            }
        }
        InteractWithObject();
        DropItem();
    }

    public void SetTextAndImageItemInventory(Dictionary<int, int> slotsList, int slotNumber)
    {
        GameObject setThisGameObject = EditorUtility.InstanceIDToObject(slotsList[slotNumber]) as GameObject;
        Transform itemBarTransform = hotbar.transform.Find("ItemBar"+Convert.ToString(slotNumber));
        GameObject itemBarGameObject = itemBarTransform.gameObject;
        Transform textItemTransform = itemBarGameObject.transform.Find("ItemBar"+Convert.ToString(slotNumber)+"Name");
        GameObject textItemGameObject = textItemTransform.gameObject;
        TextMeshProUGUI textMesh = textItemGameObject.GetComponent<TextMeshProUGUI>();
        if (slotsList[slotNumber] != 0) { textMesh.text = Convert.ToString(setThisGameObject.name); }
        else { textMesh.text = ""; }
    }
    private void InteractWithObject()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        PlayerGUI playerGUI = gameObject.GetComponent<PlayerGUI>();
        if (Physics.Raycast(ray, out hit, distanceInteract) && hit.collider != null && hit.collider.gameObject.CompareTag("Interactable"))
        {
            ObjectInteractable hitInteractableComponent = hit.collider.gameObject.GetComponent<ObjectInteractable>();
            itemInteractableTextMesh.text = textToInteract[hitInteractableComponent.typeForInteract];
            if (!hitInteractableComponent.inPlayer && hitInteractableComponent.typeForInteract == "CommomItem") {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    itemInteractableTextMesh.text = "";
                    Vector3 hitPosition = hit.collider.gameObject.transform.position;
                    int numberSlot = -1;
                    for (int i=1;i<=slots.Count;i++)
                    {
                        if (slots[itemSelectedInventory] == 0 && i == 1)
                        {
                            numberSlot = itemSelectedInventory;
                            break;
                        }
                        if (slots[i] == 0)
                        {
                            numberSlot = i;
                            break;
                        }
                    }
                    GameObject hitName = hit.collider.gameObject;
                    if (numberSlot != -1) { slots[numberSlot] = hitName.GetInstanceID(); } //Set the name of object in the first empty item bar
                    //Destroy(hit.collider.gameObject);
                    hitInteractableComponent.itemPointSticked = itemPoint;
                    hitInteractableComponent.inPlayer = true;
                    hitInteractableComponent.uiPlayer = this.gameObject;
                    hitInteractableComponent.numberSlot = numberSlot;
                }
            } else if (hitInteractableComponent.typeForInteract == "Seat" && (playerGUI.player.gameObject.GetComponent<PlayerMotor>()).inSitting == false) {
                SeatConfig seatConfig = hit.collider.gameObject.GetComponent<SeatConfig>();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    seatConfig.used = true;
                    seatConfig.playerSitting = playerGUI.player.gameObject;
                    seatWherePlayerIs = hit.collider.gameObject;
                    itemInteractableTextMesh.text = "";
                }
            } else if (hitInteractableComponent.typeForInteract == "Seat" && (playerGUI.player.gameObject.GetComponent<PlayerMotor>()).inSitting == true) {
                SeatConfig seatConfig = seatWherePlayerIs.GetComponent<SeatConfig>();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    seatConfig.used = false;
                    seatConfig.playerSitting = null;
                    playerGUI.player.gameObject.GetComponent<PlayerMotor>().inSitting = false;
                    itemInteractableTextMesh.text = "";
                }
                else
                {
                    itemInteractableTextMesh.text = "";
                }
            } else
            {
                itemInteractableTextMesh.text = "";
            }
        } 
        else if ((playerGUI.player.gameObject.GetComponent<PlayerMotor>()).inSitting == true) 
        {
            SeatConfig seatConfig = seatWherePlayerIs.GetComponent<SeatConfig>();
            if (Input.GetKeyDown(KeyCode.E) && seatConfig.used == true)
            {
                seatConfig.used = false;
                seatConfig.playerSitting = null;
                playerGUI.player.gameObject.GetComponent<PlayerMotor>().inSitting = false;
                itemInteractableTextMesh.text = "";
            }
        }
        else
        {
            itemInteractableTextMesh.text = "";
        }
        Debug.DrawRay(ray.origin, ray.direction * distanceInteract);
    }

    private void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.G) && EditorUtility.InstanceIDToObject(slots[itemSelectedInventory]) is UnityEngine.Object)
        {
            UnityEngine.Object interactable = EditorUtility.InstanceIDToObject(slots[itemSelectedInventory]);
            Debug.Log(slots[itemSelectedInventory]);
            ObjectInteractable objectInteract = interactable.GetComponent<ObjectInteractable>();
            objectInteract.inPlayer = false;
            objectInteract.numberSlot = 0;
            slots[itemSelectedInventory] = 0;
            SetTextAndImageItemInventory(slots, itemSelectedInventory);
        }
    }
}
