using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTowerLaser : MonoBehaviour
{

   private Vector3 targetPosition;
    public static void Create(Vector3 spawnPosition, Vector3 targetPosition, GameObject projectile)
    {
        spawnPosition.z = -1;
        GameObject towerLaser = Instantiate(projectile, spawnPosition, Quaternion.identity);
        ProjectileTowerLaser laser = towerLaser.GetComponent<ProjectileTowerLaser>();
        laser.Setup(targetPosition);
        //Instantiate(pf_towerLaser);
    }

    private void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        float moveSpeed = 30f;

        transform.position += direction * moveSpeed * Time.deltaTime;

        float destroySelfDistance = 1f;


        float angle = GetAngleFromVector(direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
        if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
        {
            Destroy(gameObject);
        }
    }

    public float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 350;
        return n;
    }

    







}
