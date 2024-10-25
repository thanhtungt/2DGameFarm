using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public int damagePerHit = 20;      // Sát thýõng m?c ð?nh c?a player
    public float attackRange = 2f;     // Ph?m vi t?n công
    public LayerMask enemyLayers;      // L?p c?a enemy
    private Item equippedTool;         // Công c? hi?n t?i c?a ngý?i chõi
    private ToolbarController toolbarController;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Gán ToolbarController
        toolbarController = FindObjectOfType<ToolbarController>();
    }

    void Update()
    {
        // L?y item t? toolbar
        equippedTool = toolbarController.GetItem;

        // Ki?m tra khi nh?n chu?t ð? t?n công
        if (Input.GetMouseButtonDown(0) && equippedTool != null)  // Nh?n chu?t trái và ph?i có công c?
        {
            Attack();
        }
    }

    void Attack()
    {
        // T?m các quái v?t trong ph?m vi t?n công
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        // L?y sát thýõng d?a trên công c?
        int attackDamage = GetDamageBasedOnTool(equippedTool);

        // Gây sát thýõng cho t?ng quái v?t
        foreach (Collider2D enemy in hitEnemies)
        {
            // Ki?m tra n?u NPC có script EnemyController
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(attackDamage); // G?i phýõng th?c TakeDamage n?u có
            }
            else
            {
                // N?u NPC không có EnemyController, ki?m tra script khác
                var capyparaScript = enemy.GetComponent<CapyparaNp1>();
                if (capyparaScript != null)
                {
                    capyparaScript.TakeDamage(attackDamage); // G?i TakeDamage cho NPC dùng script CapyparaNp1
                }
                else
                {
                    // Ki?m tra n?u NPC có script ChickenHp1
                    var chickenScript = enemy.GetComponent<ChickenHp1>();
                    if (chickenScript != null)
                    {
                        chickenScript.TakeDamage(attackDamage); // G?i TakeDamage cho NPC dùng script ChickenHp1
                    }
                }
            }
        }
    }

    // Hàm tính sát thýõng d?a trên công c?
    private int GetDamageBasedOnTool(Item tool)
    {
        if (tool == null) return damagePerHit; // Sát thýõng m?c ð?nh n?u không có công c?

        switch (tool.Name)
        {
            case "Kiem": // Ví d? công c? ki?m gây sát thýõng nhi?u hõn
                return 30;
            case "Axe": // Ví d? công c? r?u gây sát thýõng v?a ph?i
                return 15;
            default:
                return 0; // Các công c? khác không gây sát thýõng
        }
    }

    // V? ph?m vi t?n công trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
