using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private float radius;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask wallLayer;

    private void Start()
    {
        StartCoroutine(DetectEnemies());
    }

    private IEnumerator DetectEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
        {
            yield return wait;
            EnemiesCheck();
        }
    }

    private void EnemiesCheck()
    {
        Collider2D[] enemyChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        foreach(Collider2D enemy in enemyChecks)
        {
            Vector2 directionToTarget = (enemy.transform.position - transform.position).normalized;
            float distanceToTarget = Vector2.Distance(transform.position, enemy.transform.position);

            if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer) || !Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallLayer))
            {
                enemy.GetComponent<Enemy>().CanSeePlayer();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
