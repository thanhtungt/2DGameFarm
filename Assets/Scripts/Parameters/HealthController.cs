using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HealthController : MonoBehaviour
{
    public Slider healthSlider;  // Thanh máu của player
    public float maxHealth = 100f; // Máu tối đa của player
    public static float currentHealth; // Máu hiện tại của player
    public Image fillSlider; // Thành phần hình ảnh của thanh máu

    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu hiện tại
        fillSlider = healthSlider.GetComponentsInChildren<Image>().FirstOrDefault(t => t.name == "Fill");
        healthSlider.maxValue = maxHealth; // Thiết lập giá trị tối đa cho thanh máu
        healthSlider.value = currentHealth; // Thiết lập giá trị hiện tại cho thanh máu
    }

    void Update()
    {
        healthSlider.value = currentHealth; // Cập nhật giá trị thanh máu

        // Thay đổi màu của thanh máu dựa trên lượng máu hiện tại
        if (currentHealth >= 50)
        {
            fillSlider.color = Color.green; // Màu xanh khi máu nhiều hơn 50%
        }
        else if (currentHealth < 50 && currentHealth >= 30)
        {
            fillSlider.color = new Color(1f, 0.5f, 0f); // Màu cam khi máu giữa 30% và 50%
        }
        else if (currentHealth < 30)
        {
            fillSlider.color = Color.red; // Màu đỏ khi máu dưới 30%
        }
    }

    // Phương thức nhận sát thương từ NPC
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Giảm máu hiện tại
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo máu không vượt quá giới hạn
        Debug.Log("Player nhận " + damage + " sát thương! Máu hiện tại: " + currentHealth);

        // Kiểm tra nếu Player chết
        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm xử lý khi player chết
        }
    }

    // Hàm xử lý khi Player chết
    void Die()
    {
        Debug.Log("Player đã chết!");
        // Bạn có thể thêm xử lý khác như reload scene hoặc game over tại đây
    }
}
