using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWakeUp : MonoBehaviour
{
    [SerializeField][Min(1)] private float width, height;
    [SerializeField] private LayerMask enemyMask;
    private Vector2 minPos, maxPos;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DetectEnemies());        
    }

    private IEnumerator DetectEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            EnemiesCheck();
        }
    }

    private void EnemiesCheck()
    {
        minPos = new Vector2(transform.position.x - (width / 2), transform.position.y - (height / 2));
        maxPos = new Vector2(transform.position.x + (width / 2), transform.position.y + (height / 2));

        Collider2D[] enemies = Physics2D.OverlapAreaAll(minPos, maxPos, enemyMask);

        foreach(Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().AwakenEnemy();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //bottom and top line
        Gizmos.DrawLine(new Vector3(transform.position.x - (width / 2), transform.position.y - (height / 2), 0), new Vector3(transform.position.x + (width / 2), transform.position.y - (height / 2), 0));
        Gizmos.DrawLine(new Vector3(transform.position.x - (width / 2), transform.position.y + (height / 2), 0), new Vector3(transform.position.x + (width / 2), transform.position.y + (height / 2), 0));
        //left and right lines
        Gizmos.DrawLine(new Vector3(transform.position.x - (width / 2), transform.position.y - (height / 2), 0), new Vector3(transform.position.x - (width / 2), transform.position.y + (height / 2), 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + (width / 2), transform.position.y - (height / 2), 0), new Vector3(transform.position.x + (width / 2), transform.position.y + (height / 2), 0));
    }
}
