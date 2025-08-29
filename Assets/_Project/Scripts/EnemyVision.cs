using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private LayerMask _obstaclesLayer;
    public Transform target;

    public bool CanSeeTarget()
    {
        if (target == null) return false;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToTarget);
        float cosAngle = Mathf.Cos(_viewAngle * Mathf.Deg2Rad);
        if (dot < cosAngle)
        {
            return false;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > _viewDistance)
        {
            return false;
        }

        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToTarget))
        {
            if (hit.transform.gameObject.layer == _obstaclesLayer)
            {
                return false;
            }

            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;
    }
}
