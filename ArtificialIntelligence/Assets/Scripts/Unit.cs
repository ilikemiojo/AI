using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;
    
    public Transform target;
    public float speed = 20f;
    public float turnSpeed = 3f;
    public float turnDistance = 5f;
    public float stoppingDistance = 10f;

    Path path;

    private void Start() {
        StartCoroutine (UpdatePath());
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            path = new Path (waypoints, transform.position, turnDistance, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath () {

        if (Time.timeSinceLevelLoad < 0.3f) {
            yield return new WaitForSeconds (0.3f);
        }
        PathRequestManager.RequestPath (new PathRequest (transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPositionOld = target.position;

        while (true) {
            yield return new WaitForSeconds (minPathUpdateTime);
            if ((target.position - targetPositionOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath (new PathRequest (transform.position, target.position, OnPathFound));
                targetPositionOld = target.position;
            }
            
        }
    }

    IEnumerator FollowPath() {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt (path.lookPoints [0]);

        float speedPercent = 1f;

        while (followingPath) {
            Vector2 position2D = new Vector2 (transform.position.x, transform.position.z);

            while (path.turnBoundaries[pathIndex].HasCrossedLine (position2D)) {
                if (pathIndex == path.finishLineIndex) {
                    followingPath = false;
                    break;
                }
                else {
                    pathIndex++;
                }
            }

            if (followingPath) {

                if (pathIndex >= path.slowDownIndex && stoppingDistance > 0) {
                    speedPercent = Mathf.Clamp01 (path.turnBoundaries[path.finishLineIndex].DistanceFromPoint (position2D) / stoppingDistance);
                    if (speedPercent < 0.01f) {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation (path.lookPoints [pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            yield return null;
        }
    }

    private void OnDrawGizmos() {
        if (path != null) {
            path.DrawWithGizmos();
        }
    }
}
