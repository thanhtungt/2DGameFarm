using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHit1 : ToolHit
{
    //[SerializeField] GameObject pickUpMap;
    [SerializeField] GameObject pickUpKiem;


    [SerializeField] int dropCount = 1;
    [SerializeField] float spread = 0.9f;

    List<GameObject> items;

    //Kiểm tra người dùng có kry chưa
    public static bool hasPickUpKey = false;

    private void Start()
    {
        items = new List<GameObject>();
        //items.Add(pickUpMap);
        items.Add(pickUpKiem);

    }

    public override void Hit()
    {
        if (hasPickUpKey)
        {

            // spawning objects
            while (dropCount > 0)
            {
                dropCount -= 1;

                // calculating where items will drop
                Vector3 position = transform.position;
                position.x -= spread * UnityEngine.Random.value - spread / 2;
                position.y -= spread * UnityEngine.Random.value - spread / 2;

                GameObject newObject = Instantiate(items[dropCount]);
                newObject.transform.position = position;
            }

            Destroy(gameObject);

        }
        else
        {
            Debug.Log("You need the key to open this chest !");
        }
    }


}

