using UnityEngine;

public class ChickenHp1 : MonoBehaviour
{
    private Animator animator;
    public int maxHealth = 100;          // M�u t?i �a c?a qu�i
    public int currentHealth;            // M�u hi?n t?i
    public float speed = 2f;             // T?c �? di chuy?n c?a qu�i
    public float moveDistance = 5f;      // Kho?ng c�ch di chuy?n qua l?i

    public Transform healthBarFill;      // Thanh m�u
    public GameObject pickUpDrop;        // V?t ph?m sinh ra khi boss b? ��nh b?i
    public int dropCount = 5;            // S? l�?ng v?t ph?m sinh ra
    public float spread = 0.9f;          // Ph�n t�n v? tr� v?t ph?m

    private Vector2 startingPosition;    // V? tr� b?t �?u
    private float currentDistance = 0f;  // Kho?ng c�ch hi?n t?i so v?i v? tr� ban �?u
    private int direction = 1;           // H�?ng di chuy?n: 1 l� ph?i, -1 l� tr�i

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;        // Kh?i t?o m�u hi?n t?i
        startingPosition = transform.position;  // L�u v? tr� ban �?u
    }

    void Update()
    {
        // Ki?m tra n?u qu�i �? ch?t
        if (currentHealth <= 0)
        {
            animator.SetTrigger("death"); // G?i ho?t ?nh ch?t
            Destroy(gameObject, 1f); // H?y �?i t�?ng sau 1 gi�y
            return; // Kh�ng th?c hi?n c�c h�nh �?ng kh�c n?u �? ch?t
        }

        // Di chuy?n qu�i qua l?i
        MoveEnemy();
    }

    void MoveEnemy()
    {
        // Di chuy?n qu�i qua l?i
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // C?p nh?t kho?ng c�ch hi?n t?i so v?i v? tr� ban �?u
        currentDistance += direction * speed * Time.deltaTime;

        // Ki?m tra n?u qu�i �? di chuy?n �? kho?ng c�ch t? v? tr� ban �?u
        if (Mathf.Abs(currentDistance) >= moveDistance)
        {
            direction *= -1;  // �?i h�?ng di chuy?n
            currentDistance = 0f; // Reset kho?ng c�ch hi?n t?i

            // Quay �?u qu�i/boss b?ng c�ch thay �?i scale tr?c X
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * direction; // �?m b?o scale ��ng h�?ng
            transform.localScale = localScale; // C?p nh?t scale �? quay �?u
        }




    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;          // Gi?m m�u khi b? ��nh
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // �?m b?o m�u kh�ng �m

        // C?p nh?t thanh m�u
        UpdateHealthBar();

        // Ki?m tra n?u ch?t
        if (currentHealth <= 0)
        {
            HandleDeath(); // G?i h�m x? l? khi ch?t
        }
    }

    // C?p nh?t thanh m�u
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth; // T�nh t? l? m�u c?n l?i
        if (healthBarFill != null)
        {
            // Gi? nguy�n t? l? y v� z, ch? thay �?i t? l? x theo ph?n tr�m m�u
            healthBarFill.localScale = new Vector3(1.1709f * healthPercent, healthBarFill.localScale.y, healthBarFill.localScale.z);
            // �?m b?o thanh m�u kh�ng b? l?t theo NPC
            healthBarFill.localScale = new Vector3(Mathf.Abs(healthBarFill.localScale.x), healthBarFill.localScale.y, healthBarFill.localScale.z); // Gi? gi� tr? d��ng cho x

        }
    }

    // H�m x? l? khi boss b? ��nh b?i
    private void HandleDeath()
    {
        // Sinh ra v?t ph?m khi boss b? ��nh b?i
        for (int i = 0; i < dropCount; i++)
        {
            // T�nh v? tr� r�i c?a v?t ph?m
            Vector3 position = transform.position;
            position.x -= spread * Random.value - spread / 2;
            position.y -= spread * Random.value - spread / 2;
            GameObject log = Instantiate(pickUpDrop);
            log.transform.position = position;
        }

        Destroy(gameObject); // H?y �?i t�?ng boss sau khi b? ��nh b?i
        Debug.Log("Boss �? b? ��nh b?i!");
    }
}
