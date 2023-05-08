using System;
using System.Collections;
using UnityEngine;

public class FlagObjective : MonoBehaviour
{
    [SerializeField] private LayerMask seekersLayerMask;
    [SerializeField] private Collider pickupTrigger;
    [SerializeField] private float pickupCooldown = 3f;

    private bool _pickedUp;

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if that object's layer is included in seekers layer mask
        if ((seekersLayerMask.value & (1 << other.transform.gameObject.layer)) == 0)
        {
            return;
        }

        StickToHolder(other.gameObject.transform);
        EventDispatcher.Instance.Raise(new LevelFlagEvent { FlagPickedUp = true });
    }

    private void StickToHolder(Transform holder)
    {
        _pickedUp = true;
        transform.SetParent(holder);
        var newPosition = transform.position - holder.forward * 2f;
        transform.position = newPosition;
        pickupTrigger.enabled = false;
    }

    private void OnPlayerDeath(PlayerDeathEvent ev)
    {
        if (!_pickedUp)
        {
            return;
        }
        
        DropOnGround(ev.PlayerObject.transform.position);
        EventDispatcher.Instance.Raise(new LevelFlagEvent { FlagPickedUp = false });
    }

    private void DropOnGround(Vector3 newPosition)
    {
        _pickedUp = false;
        transform.parent = null;
        transform.position = newPosition;
        StartCoroutine(EnableTrigger());
    }

    private IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(pickupCooldown);
        pickupTrigger.enabled = true;
    }
}