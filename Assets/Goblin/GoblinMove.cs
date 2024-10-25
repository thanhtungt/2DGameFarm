using UnityEngine;
using System;
using System.Collections;

public class GoblinMove : MonoBehaviour
{

    private Animator animator;
    public int maxHealth = 100;          // Máu tối đa của quái
    public int currentHealth;             // Máu hiện tại
    public float speed = 1f;              // Tốc độ di chuyển của quái
    public float moveDistance = 4f;       // Khoảng cách di chuyển qua lại
    public float minX = -5f;   // Giới hạn bên trái
    public float maxX = 5f;    // Giới hạn bên phải
    private Vector2 startingPosition;      // Vị trí ban đầu
    private float currentDistance = 0f;    // Khoảng cách hiện tại so với vị trí ban đầu
    private int direction = 1;             // Hướng di chuyển: 1 là phải, -1 là trái

    void Start()
    {
        animator = this.GetComponent<Animator>();
        currentHealth = maxHealth;        // Khởi tạo máu hiện tại
        startingPosition = transform.position;  // Lưu vị trí ban đầu
    }

    void Update()
    {
        // Di chuyển quái qua lại
        MoveEnemy();

        // Kiểm tra nếu quái chết
        if (currentHealth <= 0)
        {
            animator.SetTrigger("die"); // Gọi hoạt ảnh chết
            Destroy(gameObject, 1f); // Hủy đối tượng sau 1 giây
        }
    }

    void MoveEnemy()
    {
        // Di chuyển quái qua lại
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Cập nhật khoảng cách hiện tại so với vị trí ban đầu
        currentDistance += direction * speed * Time.deltaTime;

        // Kiểm tra nếu quái đã di chuyển đủ khoảng cách từ vị trí ban đầu
        if (Mathf.Abs(currentDistance) >= moveDistance)
        {
            direction *= -1;  // Đổi hướng di chuyển
            currentDistance = 0f; // Reset khoảng cách hiện tại

            // Quay đầu quái vật
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
    }
}
