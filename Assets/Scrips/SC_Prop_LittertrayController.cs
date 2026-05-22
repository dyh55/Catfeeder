using UnityEngine;

public class LitterTrayController : MonoBehaviour
{
    [Header("专属猫")]
    public CatController ownerCat;   // 仅这只猫可以吃这个盆

    [Header("升级资源（01→02）")]
    public Mesh upgradedMesh;        // 升级后的 Mesh（02 版本）
    public Material upgradedMaterial;// 升级后的材质
    private Mesh originalMesh;
    private Material originalMaterial;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private bool isUpgraded = false;  // 是否已升级为02（可吃状态）
    private bool isUsed = false;      // 是否已被猫吃过（吃完后变为不可用）

    [Header("玩家交互")]
    private PlayerController player;
    private bool waitingForPlayer = false;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshFilter != null) originalMesh = meshFilter.mesh;
        if (meshRenderer != null) originalMaterial = meshRenderer.material;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // 等待玩家靠近后升级
        if (waitingForPlayer && player != null)
        {
            UnityEngine.AI.NavMeshAgent agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null && agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                Upgrade();
                waitingForPlayer = false;
            }
        }
    }
    public void ResetState()
    {
        isUpgraded = false;
        isUsed = false;
        // 恢复原始外观
        if (originalMesh != null && meshFilter != null)
            meshFilter.mesh = originalMesh;
        if (originalMaterial != null && meshRenderer != null)
            meshRenderer.material = originalMaterial;
        waitingForPlayer = false;
    }
    // 鼠标点击时，如果未升级，通知玩家移动过来
    private void OnMouseDown()
    {
        if (!isUpgraded && player != null)
        {
            player.MoveToTarget(transform.position);
            waitingForPlayer = true;
            Debug.Log($"玩家点击 {name}，前往升级");
        }
    }

    // 升级为02（可吃状态）
    public void Upgrade()
    {
        if (isUpgraded) return;
        if (upgradedMesh != null && meshFilter != null) meshFilter.mesh = upgradedMesh;
        if (upgradedMaterial != null && meshRenderer != null) meshRenderer.material = upgradedMaterial;
        isUpgraded = true;
        isUsed = false;
        Debug.Log($"{name} 已升级为02，可以被 {ownerCat?.name} 吃一次");
    }

    // 猫吃完后调用，标记为已用，并可选恢复为01外观
    public void OnCatEat()
    {
        if (!isUpgraded || isUsed) return;
        isUsed = true;
        // 吃完后变回01（可选，也可以保持02但标记不可吃）
        if (originalMesh != null && meshFilter != null) meshFilter.mesh = originalMesh;
        if (originalMaterial != null && meshRenderer != null) meshRenderer.material = originalMaterial;
        isUpgraded = false;
        Debug.Log($"{name} 已被 {ownerCat?.name} 吃掉，恢复为01且不可再用");
    }

    // 检查该盆对指定猫是否可用（必须已升级且未被吃过，且猫匹配）
    public bool IsAvailable(CatController cat)
    {
        return isUpgraded && !isUsed && ownerCat == cat;
    }
}