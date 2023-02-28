using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool showPathGizmos;
    //[SerializeField] private Transform target;
    float speed = 5;
    private Vector3[] path;
    private int targetIndex;
    private bool stopped = false;
    public bool isStopped { get { return stopped; } }

    public void PathFind(Vector3 targetPosition)
    {
        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
    }

    public void PathFind(Transform target)
    {
        Vector3 targetPosition = target.position;
        PathFind(targetPosition);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        Debug.Log(pathSuccessful);
        if(pathSuccessful && newPath.Length > 0)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        stopped = false;
        Vector3 currentWaypoint = path[0];

        while(true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    stopped = true;
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(path != null && showPathGizmos)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.1f);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
