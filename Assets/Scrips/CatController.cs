using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CatController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    [Header("专属盆")]
    public LitterTrayController myTray;

    [Header("移动参数")]
    public float wanderRadius = 8f;
    public float minSitTime = 3f;
    public float maxSitTime = 6f;

    [Header("饱腹参数")]
    public float fullDuration = 60f;
    private bool isHungry = true;
    private float hungryTimer = 0f;

    [Header("进食距离")]
    public float eatingDistance = 1.2f;
    public float eatAnimationDuration = 1.5f;

    private bool isEating = false;
    private bool isSitting = false;

    [Header("重置位置（可选）")]
    public Transform startPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (myTray == null) Debug.LogError($"{name} 没有关联盆");
        StartCoroutine(StateMachine());
    }

    private void Update()
    {
        if (!isHungry)
        {
            hungryTimer -= Time.deltaTime;
            if (hungryTimer <= 0f)
            {
                isHungry = true;
                Debug.Log($"{name} 饿了");
                StopAllCoroutines();
                StartCoroutine(StateMachine());
            }
        }
        if (anim != null) anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    public void ResetState()
    {
        StopAllCoroutines();
        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = false;
            agent.velocity = Vector3.zero;
        }
        isEating = false;
        isSitting = false;
        isHungry = true;
        hungryTimer = 0f;
        if (startPoint != null) transform.position = startPoint.position;
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            if (isHungry && myTray != null && myTray.IsAvailable(this))
            {
                yield return StartCoroutine(GoToEat());
                if (!isHungry) continue;
            }
            yield return StartCoroutine(RandomWalk());
            yield return StartCoroutine(Sit());
        }
    }

    private IEnumerator RandomWalk()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;
        randomDir.y = transform.position.y;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            float timer = 0f;
            while (agent.pathPending || agent.remainingDistance > 0.3f)
            {
                if (timer > 8f) break;
                timer += Time.deltaTime;
                yield return null;
            }
            if (timer > 8f) agent.ResetPath();
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Sit()
    {
        if (isSitting) yield break;
        isSitting = true;
        agent.isStopped = true;
        agent.ResetPath();
        float sitDuration = Random.Range(minSitTime, maxSitTime);
        if (anim != null) anim.SetTrigger("Sit_start");
        yield return new WaitForSeconds(0.5f);
        if (anim != null) anim.SetBool("Sit_Idle", true);
        yield return new WaitForSeconds(sitDuration);
        if (anim != null)
        {
            anim.SetBool("Sit_Idle", false);
            anim.SetTrigger("Sit_Up");
        }
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;
        isSitting = false;
    }

    private IEnumerator GoToEat()
    {
        agent.SetDestination(myTray.transform.position);
        float timer = 0f;
        while (agent.pathPending || agent.remainingDistance > eatingDistance)
        {
            if (timer > 10f)
            {
                agent.ResetPath();
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(Eat());
    }

    private IEnumerator Eat()
    {
        if (isEating) yield break;
        isEating = true;
        agent.isStopped = true;
        agent.ResetPath();
        if (anim != null) anim.SetTrigger("Eat");
        yield return new WaitForSeconds(eatAnimationDuration);
        myTray.OnCatEat();
        isHungry = false;
        hungryTimer = fullDuration;
        yield return new WaitForSeconds(0.1f);
        agent.isStopped = false;
        isEating = false;
        StopAllCoroutines();
        StartCoroutine(StateMachine());
    }
}