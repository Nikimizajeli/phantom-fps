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
    [SerializeField] private float provokedDistanceMultiplier = 3f;
    [SerializeField] private float provokedAngleMultiplier = 3f;

    [Header("Gizmo")] 
    [SerializeField] private bool debugFieldOfView;
    [SerializeField] private Color lineColor;
    
    public event Action<GameObject> TargetFound;
    
    private Collider[] _collidersHit;
    private bool _provoked;
    private float DetectionDistance => _provoked ? detectionDistance * provokedDistanceMultiplier : detectionDistance;
    private float DetectionAngle => _provoked ? detectionAngle * provokedAngleMultiplier : detectionAngle; 
    
    
    protected void Awake()
    {
        _collidersHit = new Collider[Constants.MaxPlayers];
    }

    private void OnEnable()
    {
        StartCoroutine(SeekTargets());
    }

    protected void OnDisable()
    {
        StopCoroutine(SeekTargets());
    }

    public void Provoke()
    {
        _provoked = true;
    }
    
    private IEnumerator SeekTargets()
    {
        while (true)
        {
            var numberOfHits =
                Physics.OverlapSphereNonAlloc(transform.position, DetectionDistance, _collidersHit, targetLayer);
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

        return angle <= DetectionAngle * 0.5f;
    }

    private bool LineOfSightBlocked(GameObject otherObject)
    {
        var rayDirection = (otherObject.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, rayDirection, out var hitInfo, DetectionDistance + 1))
        {

#if UNITY_EDITOR
            if (debugFieldOfView)
            {
                Debug.DrawRay(transform.position, rayDirection * DetectionDistance, Color.red, 2f);
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
        
        Quaternion rotationRight = Quaternion.AngleAxis(DetectionAngle * 0.5f, up);
        Quaternion rotationLeft = Quaternion.AngleAxis(DetectionAngle * -0.5f, up);
        
        Handles.DrawWireArc(position, up, rotationLeft * forward, DetectionAngle, DetectionDistance);
        Handles.DrawLine(position, position + rotationRight * forward * DetectionDistance);
        Handles.DrawLine(position, position + rotationLeft * forward * DetectionDistance);
        
    }
#endif
}