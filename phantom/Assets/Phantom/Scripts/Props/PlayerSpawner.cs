using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawningPosition;

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(PlayerSpawnedEvent ev)
    {
        ev.PlayerObject.transform.position = spawningPosition.position;
        ev.PlayerObject.transform.rotation = spawningPosition.rotation;

        // TODO: move it out of spawner
        StartCoroutine(UnlockPlayerMovement(ev.PlayerObject));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} entered collider");
    }

    private IEnumerator UnlockPlayerMovement(GameObject playerObject)
    {
        yield return new WaitForSeconds(1f);
        playerObject.GetComponent<PlayerController>()?.EnableMovement(true);
        
    }
}