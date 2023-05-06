using System;
using System.Collections;
using Phantom.Scripts.Configuration;
using UnityEditor;
using UnityEngine;

public class Spotter : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectionDistance = 20f;
    [SerializeField] private float detectionInterval = 0.5f;
    [SerializeField] private float detectionAngle = 120f;

    [Header("Gizmo")] 
    [SerializeField] private bool debugFieldOfView;
    [SerializeField] private Color lineColor;
    
    public event Action<GameObject> TargetFound;
    
    private Collider[] _collidersHit;
    
    protected void Awake()
    {
        _collidersHit = new Collider[Constants.MaxPlayers];
        StartCoroutine(SeekTargets());
    }

    private void OnEnable()
    {
        StartCoroutine(SeekTargets());
    }

    protected void OnDisable()
    {
        StopCoroutine(SeekTargets());
    }

    IEnumerator SeekTargets()
    {
        while (true)
        {
            var numberOfHits =
                Physics.OverlapSphereNonAlloc(transform.position, detectionDistance, _collidersHit, targetLayer);
            for (int i = 0; i < numberOfHits; i++)
            {
                GameObject target = _collidersHit[i].gameObject;
                if (IsInFieldOfView(target) && !LineOfSightBlocked(target))
                {
                    TargetFound?.Invoke(target);
                }
            }

            yield return new WaitForSeconds(detectionInterval);
        }
    }

    private bool IsInFieldOfView(GameObject otherObject)
    {
        Vector3 directionToOther = otherObject.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToOther);

        return angle <= detectionAngle * 0.5f;
    }

    private bool LineOfSightBlocked(GameObject otherObject)
    {
        var rayDirection = (otherObject.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, rayDirection, out var hitInfo, detectionDistance + 1))
        {

#if UNITY_EDITOR
            if (debugFieldOfView)
            {
                Debug.DrawRay(transform.position, rayDirection * detectionDistance, Color.red, 2f);
            }
#endif
            
            return hitInfo.transform.gameObject != otherObject;
        }

        Debug.LogWarning("Spotters raycast couldn't hit a collider");
        return true;
    }
    
#if UNITY_EDITOR
    // draw detection cone
    private void OnDrawGizmos()
    {
        if (!debugFieldOfView)
        {
            return;
        }
        
        Handles.color = lineColor;
        var up = transform.up;
        var position = transform.position;
        var forward = transform.forward;
        
        Quaternion rotationRight = Quaternion.AngleAxis(detectionAngle * 0.5f, up);
        Quaternion rotationLeft = Quaternion.AngleAxis(detectionAngle * -0.5f, up);
        
        Handles.DrawWireArc(position, up, rotationLeft * forward, detectionAngle, detectionDistance);
        Handles.DrawLine(position, position + rotationRight * forward * detectionDistance);
        Handles.DrawLine(position, position + rotationLeft * forward * detectionDistance);
        
    }
#endif
}