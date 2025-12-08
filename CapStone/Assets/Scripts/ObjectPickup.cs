using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{

    // When player collides with object destory object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the GameManager that this item was picked up
            if (GameManager.Instance != null)
                GameManager.Instance.AddCollectible();

            Destroy(transform.root.gameObject);
        }
    }
}
