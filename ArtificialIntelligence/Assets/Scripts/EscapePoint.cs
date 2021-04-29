using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePoint : MonoBehaviour
{
    public LayerMask seekerMask;
    public Player player;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            Collider[] seekers = Physics.OverlapSphere(transform.position, 10f, seekerMask);
            if (seekers.Length > 0) {
                player.SetEscapePoint(seekers[0].transform);
            }
            else {
                player.SetTarget();
            }
        }
    }
}
