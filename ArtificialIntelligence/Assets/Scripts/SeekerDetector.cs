using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerDetector : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Seeker")) {
            player.SetEscapePoint(other.transform);
        }
            
    }
}
