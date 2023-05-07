using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Spotter spotter;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float chasingRange = 5f;

    private bool _playerDetected;
    private float _distanceToTarget = Mathf.Infinity;
    private Transform _target;

    protected void Start()
    {
    }

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<PlayerDeathEvent>(OnTargetDeath);
        spotter.TargetFound += OnTargetFound;
        
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<PlayerDeathEvent>(OnTargetDeath);
        spotter.TargetFound -= OnTargetFound;
    }

    protected void Update()
    {
        if (_playerDetected)
        {
            EngageTarget();
        }
    }

    private void EngageTarget()
    {
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
        var inChasingRange = _distanceToTarget < chasingRange; 
        if (inChasingRange && _distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (_distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }
    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(_target.position);
    }

    private void AttackTarget()
    {
        Debug.Log($"Attacking {_target.transform.name}");
    }

    private void OnTargetFound(GameObject target)
    {
        if (!_playerDetected
            || (Vector3.Distance(transform.position, _target.position) >
                Vector3.Distance(transform.position, target.transform.position)))
        {
            _playerDetected = true;
            _target = target.GetComponent<Transform>();
        }
    }

    private void OnTargetDeath(PlayerDeathEvent ev)
    {
        if (ev.PlayerObject != _target.gameObject)
        {
            return;
        }
        
        _target = null;
        _playerDetected = false;
    }
}