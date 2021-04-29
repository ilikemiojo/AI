using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Player player;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player.targets.Remove (player.currentTarget);
            player.SetTarget();
            Destroy(gameObject);
        }
    }
}
