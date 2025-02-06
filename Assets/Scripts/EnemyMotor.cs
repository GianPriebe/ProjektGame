using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMotor : MonoBehaviour
{
    //private Rigidbody rigidbodyEnemy;
    private NavMeshAgent agent;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rigidbodyEnemy = GetComponent<Rigidbody>();
        //rigidbodyEnemy.freezeRotation = true;
        agent = GetComponent<NavMeshAgent>();
        //player = player.transform.Find("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
