using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();  
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        MouseManger.Instance.OnMouseClicked += MoveToTarget;
    }
    void Update()
    {
        SwitchAnimation();
    }
    private void SwitchAnimation()
    {
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
    }
    public void MoveToTarget(Vector3 target)
    {
        agent.destination = target;
    }
}

