using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Unit thisUnit;
    public List<Transform> targets = new List <Transform> ();
    public Transform currentTarget;
    public Transform bottomLeftEscapePoint, bottomRightEscapePoint, topLeftEscapePoint, topRightEscapePoint;

    private void Awake() {
        thisUnit = GetComponent <Unit> ();
    }

    private void Start() {
        Collectable[] collectables = FindObjectsOfType<Collectable> ();

        foreach (Collectable collectable in collectables)
        {
            targets.Add (collectable.transform);
        }

        SetTarget();
    }

    public void SetTarget () {
        float minDistanceToTarget = float.MaxValue;

        if (targets.Count > 0) {
            foreach (Transform target in targets) {
                float targetDistance = Vector3.Distance(transform.position, target.position);

                if (targetDistance < minDistanceToTarget) {
                    minDistanceToTarget = targetDistance;
                    currentTarget = target;
                }
            }

            thisUnit.target = currentTarget;
        }
        else {
            thisUnit.enabled = false;
        }
    }

    public void SetEscapePoint(Transform seeker) {
        Vector3 relativePosition = transform.position - seeker.position;

        if (relativePosition.x > 0) {
            if (relativePosition.z > 0) {
                thisUnit.target = topRightEscapePoint;
                return;
            }
            else {
                thisUnit.target = bottomRightEscapePoint;
                return;
            }
        }
        else {
            if (relativePosition.z > 0) {
                thisUnit.target = topLeftEscapePoint;
                return;
            }
            else {
                thisUnit.target = bottomLeftEscapePoint;
                return;
            }
        }
    }
}
