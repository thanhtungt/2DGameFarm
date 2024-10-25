using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject toolbarPanel;
    public bool isOpen = false;
    GameObject dialogue;

    private void Start()
    {
        dialogue = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.CompareTag("startDialogue"));
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (dialogue != null && panel != null && toolbarPanel != null) // Kiểm tra các đối tượng có null không
        {
            // Mở/đóng inventory sau khi nhấn phím E
            if (Input.GetKeyDown(KeyCode.E) && !dialogue.activeSelf)
            {
                panel.SetActive(!panel.activeInHierarchy);
                toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
                isOpen = panel.activeInHierarchy;
            }
        }
        else
        {
            Debug.LogError("Một hoặc nhiều đối tượng chưa được gán trong Inspector");
        }
    }

}
