using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    public int maxHealth = 100;          // Máu tối đa của quái/boss
    public int currentHealth;            // Máu hiện tại
    public float speed = 2f;             // Tốc độ di chuyển của quái
    public float moveDistance = 5f;      // Khoảng cách di chuyển qua lại
    public float skillInterval = 5f;     // Thời gian giữa các lần gọi skill
    public int lowHealthThreshold = 30;  // Ngưỡng máu thấp để kích hoạt skill đặc biệt
    public GameObject player;             // Tham chiếu đến Player

    public Transform healthBarFill;      // Transform của thanh máu
    public GameObject pickUpDrop;        // Vật phẩm sinh ra khi boss bị đánh bại
    public int dropCount = 5;            // Số lượng vật phẩm sinh ra
    public float spread = 0.9f;           // Phân tán vị trí vật phẩm

    private Vector2 startingPosition;     // Vị trí ban đầu
    private float currentDistance = 0f;   // Khoảng cách hiện tại so với vị trí ban đầu
    private int direction = 1;            // Hướng di chuyển: 1 là phải, -1 là trái
    private bool isAttacking = false;     // Cờ trạng thái tấn công
    private float skillCooldown;           // Thời gian chờ giữa các lần dùng skill

    public int attackDamage = 10; // Sát thương mỗi lần tấn công
    public float attackRange = 1.5f; // Khoảng cách tấn công

    private DayTimeController dayTimeController; // Tham chiếu đến DayTimeController

    void Start()
    {
        animator = this.GetComponent<Animator>();
        currentHealth = maxHealth;        // Khởi tạo máu hiện tại
        startingPosition = transform.position;  // Lưu vị trí ban đầu
        skillCooldown = skillInterval;    // Bắt đầu với thời gian chờ giữa các lần gọi skill

        // Tìm đối tượng Player bằng tag
        player = GameObject.FindWithTag("Player");

        // Tìm DayTimeController
        dayTimeController = FindObjectOfType<DayTimeController>();
    }

    void Update()
    {
        // Kiểm tra nếu quái/boss chết
        if (currentHealth <= 0)
        {
            animator.SetTrigger("death"); // Gọi hoạt ảnh chết
            Destroy(gameObject, 1f); // Hủy đối tượng sau 1 giây
            return; // Không thực hiện các hành động khác nếu đã chết
        }

        // Di chuyển quái/boss qua lại
        MoveEnemy();

        // Kiểm tra xem có đủ điều kiện gọi skill không
        HandleSkillUsage();

        // Kiểm tra tấn công Player nếu trong khoảng cách tấn công
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange && !isAttacking)
        {
            AttackPlayer();
        }
        else if (player == null)
        {
            Debug.LogError("Player reference is null! Make sure the player is assigned in the EnemyController.");
        }
    }

    void MoveEnemy()
    {
        if (isAttacking) return;  // Không di chuyển khi đang tấn công

        // Di chuyển quái/boss qua lại
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Cập nhật khoảng cách hiện tại so với vị trí ban đầu
        currentDistance += direction * speed * Time.deltaTime;

        // Kiểm tra nếu quái/boss đã di chuyển đủ khoảng cách từ vị trí ban đầu
        if (Mathf.Abs(currentDistance) >= moveDistance)
        {
            direction *= -1;  // Đổi hướng di chuyển
            currentDistance = 0f; // Reset khoảng cách hiện tại

            // Quay đầu quái/boss
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // Đảo chiều X
            transform.localScale = localScale; // Cập nhật scale để quay đầu
        }

        // Kích hoạt hoạt ảnh di chuyển
        if (Mathf.Abs(currentDistance) > 0)
        {
            animator.SetTrigger("walk"); // Thay đổi thành hoạt ảnh đi
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;          // Giảm máu khi bị đánh
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không âm

        // Cập nhật thanh máu
        UpdateHealthBar();

        // Nếu máu thấp hơn ngưỡng, kích hoạt skill "phòng thủ"
        if (currentHealth <= lowHealthThreshold)
        {
            UseSkill("hit_2");
        }

        // Kiểm tra nếu chết
        if (currentHealth <= 0)
        {
            HandleDeath(); // Gọi hàm xử lý khi chết
        }
    }

    // Xử lý việc gọi skill
    void HandleSkillUsage()
    {
        skillCooldown -= Time.deltaTime; // Giảm thời gian chờ của skill

        if (skillCooldown <= 0)
        {
            // Gọi skill tấn công
            UseSkill("skill_1");

            // Reset thời gian chờ giữa các lần gọi skill
            skillCooldown = skillInterval;
        }
    }

    // Hàm gọi skill với trigger
    void UseSkill(string skill_1)
    {
        if (isAttacking) return; // Không gọi skill khi đang tấn công

        animator.SetTrigger(skill_1); // Gọi hoạt ảnh skill
        isAttacking = true;  // Đánh dấu trạng thái tấn công

        // Thời gian skill kéo dài, đặt lại trạng thái sau một khoảng thời gian
        StartCoroutine(ResetAttackState(1.5f));  // Giả sử mỗi skill kéo dài 1.5 giây
    }

    // Coroutine để reset trạng thái tấn công
    IEnumerator ResetAttackState(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;  // Kết thúc tấn công
    }

    // Cập nhật thanh máu
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth; // Tính tỉ lệ máu còn lại
        if (healthBarFill != null)
        {
            // Giữ nguyên tỉ lệ y và z, chỉ thay đổi tỉ lệ x theo phần trăm máu
            healthBarFill.localScale = new Vector3(450f * healthPercent, healthBarFill.localScale.y, healthBarFill.localScale.z);

            // Đảm bảo thanh máu không bị lật theo NPC
            Vector3 healthBarScale = healthBarFill.localScale;
            healthBarScale.x = Mathf.Abs(healthBarScale.x); // Luôn giữ giá trị dương cho x
            healthBarFill.localScale = healthBarScale;
        }
    }

    // Hàm xử lý khi boss bị đánh bại
    private void HandleDeath()
    {
        // Sinh ra vật phẩm (gỗ) khi boss bị đánh bại
        for (int i = 0; i < dropCount; i++)
        {
            // Tính vị trí rơi của vật phẩm
            Vector3 position = transform.position;
            position.x -= spread * Random.value - spread / 2;
            position.y -= spread * Random.value - spread / 2;
            GameObject log = Instantiate(pickUpDrop);
            log.transform.position = position;
        }

        Destroy(gameObject); // Hủy đối tượng boss sau khi bị đánh bại
        Debug.Log("Boss đã bị đánh bại!");
    }

    // Hàm tấn công Player
    private void AttackPlayer()
    {
        isAttacking = true; // Đánh dấu trạng thái tấn công
        animator.SetTrigger("skill_1"); // Kích hoạt hoạt ảnh tấn công
        animator.SetTrigger("skill_2");
        // Gây sát thương cho Player
        if (player != null) // Kiểm tra nếu player tồn tại
        {
            if (dayTimeController != null)
            {
                dayTimeController.ApplyDamageToPlayer(attackDamage); // Gọi phương thức giảm máu
            }
            Debug.Log("NPC đã tấn công Player và gây " + attackDamage + " sát thương!");
        }

        // Đặt lại trạng thái tấn công sau một khoảng thời gian
        StartCoroutine(ResetAttackState(1.5f)); // Giả sử mỗi lần tấn công kéo dài 1.5 giây
    }

    // Phương thức này sẽ được gọi từ Animation Event
    public void AttackStart()
    {
        // Logic khi bắt đầu tấn công, có thể để lại trống nếu không cần thêm logic nào
    }
}
