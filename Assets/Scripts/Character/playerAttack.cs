using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public int damagePerHit = 20;      // S�t th��ng m?c �?nh c?a player
    public float attackRange = 2f;     // Ph?m vi t?n c�ng
    public LayerMask enemyLayers;      // L?p c?a enemy
    private Item equippedTool;         // C�ng c? hi?n t?i c?a ng�?i ch�i
    private ToolbarController toolbarController;

    void Start()
    {
        animator = GetComponent<Animator>();
        // G�n ToolbarController
        toolbarController = FindObjectOfType<ToolbarController>();
    }

    void Update()
    {
        // L?y item t? toolbar
        equippedTool = toolbarController.GetItem;

        // Ki?m tra khi nh?n chu?t �? t?n c�ng
        if (Input.GetMouseButtonDown(0) && equippedTool != null)  // Nh?n chu?t tr�i v� ph?i c� c�ng c?
        {
            Attack();
        }
    }

    void Attack()
    {
        // T?m c�c qu�i v?t trong ph?m vi t?n c�ng
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        // L?y s�t th��ng d?a tr�n c�ng c?
        int attackDamage = GetDamageBasedOnTool(equippedTool);

        // G�y s�t th��ng cho t?ng qu�i v?t
        foreach (Collider2D enemy in hitEnemies)
        {
            // Ki?m tra n?u NPC c� script EnemyController
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(attackDamage); // G?i ph��ng th?c TakeDamage n?u c�
            }
            else
            {
                // N?u NPC kh�ng c� EnemyController, ki?m tra script kh�c
                var capyparaScript = enemy.GetComponent<CapyparaNp1>();
                if (capyparaScript != null)
                {
                    capyparaScript.TakeDamage(attackDamage); // G?i TakeDamage cho NPC d�ng script CapyparaNp1
                }
                else
                {
                    // Ki?m tra n?u NPC c� script ChickenHp1
                    var chickenScript = enemy.GetComponent<ChickenHp1>();
                    if (chickenScript != null)
                    {
                        chickenScript.TakeDamage(attackDamage); // G?i TakeDamage cho NPC d�ng script ChickenHp1
                    }
                }
            }
        }
    }

    // H�m t�nh s�t th��ng d?a tr�n c�ng c?
    private int GetDamageBasedOnTool(Item tool)
    {
        if (tool == null) return damagePerHit; // S�t th��ng m?c �?nh n?u kh�ng c� c�ng c?

        switch (tool.Name)
        {
            case "Kiem": // V� d? c�ng c? ki?m g�y s�t th��ng nhi?u h�n
                return 30;
            case "Axe": // V� d? c�ng c? r?u g�y s�t th��ng v?a ph?i
                return 15;
            default:
                return 0; // C�c c�ng c? kh�c kh�ng g�y s�t th��ng
        }
    }

    // V? ph?m vi t?n c�ng trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
