using UnityEngine;

public class ChickenHp1 : MonoBehaviour
{
    private Animator animator;
    public int maxHealth = 100;          // Máu t?i ða c?a quái
    public int currentHealth;            // Máu hi?n t?i
    public float speed = 2f;             // T?c ð? di chuy?n c?a quái
    public float moveDistance = 5f;      // Kho?ng cách di chuy?n qua l?i

    public Transform healthBarFill;      // Thanh máu
    public GameObject pickUpDrop;        // V?t ph?m sinh ra khi boss b? ðánh b?i
    public int dropCount = 5;            // S? lý?ng v?t ph?m sinh ra
    public float spread = 0.9f;          // Phân tán v? trí v?t ph?m

    private Vector2 startingPosition;    // V? trí b?t ð?u
    private float currentDistance = 0f;  // Kho?ng cách hi?n t?i so v?i v? trí ban ð?u
    private int direction = 1;           // Hý?ng di chuy?n: 1 là ph?i, -1 là trái

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;        // Kh?i t?o máu hi?n t?i
        startingPosition = transform.position;  // Lýu v? trí ban ð?u
    }

    void Update()
    {
        // Ki?m tra n?u quái ð? ch?t
        if (currentHealth <= 0)
        {
            animator.SetTrigger("death"); // G?i ho?t ?nh ch?t
            Destroy(gameObject, 1f); // H?y ð?i tý?ng sau 1 giây
            return; // Không th?c hi?n các hành ð?ng khác n?u ð? ch?t
        }

        // Di chuy?n quái qua l?i
        MoveEnemy();
    }

    void MoveEnemy()
    {
        // Di chuy?n quái qua l?i
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // C?p nh?t kho?ng cách hi?n t?i so v?i v? trí ban ð?u
        currentDistance += direction * speed * Time.deltaTime;

        // Ki?m tra n?u quái ð? di chuy?n ð? kho?ng cách t? v? trí ban ð?u
        if (Mathf.Abs(currentDistance) >= moveDistance)
        {
            direction *= -1;  // Ð?i hý?ng di chuy?n
            currentDistance = 0f; // Reset kho?ng cách hi?n t?i

            // Quay ð?u quái/boss b?ng cách thay ð?i scale tr?c X
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * direction; // Ð?m b?o scale ðúng hý?ng
            transform.localScale = localScale; // C?p nh?t scale ð? quay ð?u
        }




    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;          // Gi?m máu khi b? ðánh
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ð?m b?o máu không âm

        // C?p nh?t thanh máu
        UpdateHealthBar();

        // Ki?m tra n?u ch?t
        if (currentHealth <= 0)
        {
            HandleDeath(); // G?i hàm x? l? khi ch?t
        }
    }

    // C?p nh?t thanh máu
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth; // Tính t? l? máu c?n l?i
        if (healthBarFill != null)
        {
            // Gi? nguyên t? l? y và z, ch? thay ð?i t? l? x theo ph?n trãm máu
            healthBarFill.localScale = new Vector3(1.1709f * healthPercent, healthBarFill.localScale.y, healthBarFill.localScale.z);
            // Ð?m b?o thanh máu không b? l?t theo NPC
            healthBarFill.localScale = new Vector3(Mathf.Abs(healthBarFill.localScale.x), healthBarFill.localScale.y, healthBarFill.localScale.z); // Gi? giá tr? dýõng cho x

        }
    }

    // Hàm x? l? khi boss b? ðánh b?i
    private void HandleDeath()
    {
        // Sinh ra v?t ph?m khi boss b? ðánh b?i
        for (int i = 0; i < dropCount; i++)
        {
            // Tính v? trí rõi c?a v?t ph?m
            Vector3 position = transform.position;
            position.x -= spread * Random.value - spread / 2;
            position.y -= spread * Random.value - spread / 2;
            GameObject log = Instantiate(pickUpDrop);
            log.transform.position = position;
        }

        Destroy(gameObject); // H?y ð?i tý?ng boss sau khi b? ðánh b?i
        Debug.Log("Boss ð? b? ðánh b?i!");
    }
}
