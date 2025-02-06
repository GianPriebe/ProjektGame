using UnityEngine;

public class ItemPointRotation : MonoBehaviour
{
    public Transform playerBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform playerObjectTemp = transform.parent;
        //playerBody = playerObjectTemp.Find("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerBody.position + playerBody.forward*1f + playerBody.right*.77f;
        transform.position = new Vector3(transform.position.x, transform.position.y+.47f, transform.position.z);
        transform.rotation = playerBody.rotation;
    }
}
