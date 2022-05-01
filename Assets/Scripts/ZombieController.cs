using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    Transform player;
    State currentState;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = new Idle(this.gameObject, agent, animator, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
public class State
{
    public enum STATE {IDLE,WONDER,CHASE,ATTACK,DEATH}
    public enum EVENTS {ENTER,UPDATE,EXIT}
    public STATE stateName;
    public EVENTS eventStage;

    public GameObject zombie;
    public NavMeshAgent agent;
    public Animator animator;
    public Transform playerTransform;

    public float visualDistance = 10f;
    public float visualAngle = 30f;
    public float shootingDistance = 5f;

    public State nextState;

    public State(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform)
    {
        this.zombie = _zombie;
        this.agent = _agent;
        this.animator = _animator;
        this.playerTransform = _playerTransform;
        eventStage = EVENTS.ENTER;
        
    }
    public virtual void EnterMethod()
    {
        eventStage = EVENTS.UPDATE;
    }
    public virtual void UpdateMethod()
    {
        eventStage = EVENTS.UPDATE;
    }
    public virtual void ExitMethod()
    {
        eventStage = EVENTS.EXIT;
    }
    public State Process()
    {
        if (eventStage == EVENTS.ENTER)
        {
            EnterMethod();
        }
        if (eventStage == EVENTS.UPDATE)
        {
            UpdateMethod();
        }
        if (eventStage == EVENTS.EXIT)
        {
            ExitMethod();
            return nextState;
        }
        return this;
    }
    public bool CanSeePlayer()
    {
        Vector3 direction = playerTransform.position - zombie.transform.position;
        float angle = Vector3.Angle(direction, zombie.transform.forward);
        if (direction.magnitude < visualDistance && angle < visualAngle)
        {
            return true;
        }
        return false;
    }
    public bool EnemyCanAttackPlayer()
    {
        Vector3 direction = playerTransform.position - zombie.transform.position;
        if (direction.magnitude < shootingDistance)
        {
            return true;
        }
        return false;
    }
}

public class Idle : State
{
    public Idle(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform) : base(_zombie, _agent, _animator, _playerTransform)
    {
        stateName = STATE.IDLE;

    }
    public override void EnterMethod()
    {
        animator.SetTrigger("isWalking");
        base.EnterMethod();

    }
    public override void UpdateMethod()
    {
        if (CanSeePlayer())
        {
            nextState = new Chase(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
        else 
        {
            nextState = new Wonder(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void ExitMethod()
    {
        animator.ResetTrigger("isWalking");
        base.ExitMethod();
    }
}

public class Wonder : State
{
    public Wonder(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform) : base(_zombie, _agent, _animator, _playerTransform)
    {
        stateName = STATE.WONDER;

    }
    public override void EnterMethod()
    {
        animator.SetTrigger("isWalking");
        base.EnterMethod();

    }
    public override void UpdateMethod()
    {
        if (CanSeePlayer())
        {
            nextState = new Chase(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
        else
        {
            nextState = new Wonder(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void ExitMethod()
    {
        animator.ResetTrigger("isWalking");
        base.ExitMethod();
    }
}

public class Chase : State
{
    public Chase(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform) : base(_zombie, _agent, _animator, _playerTransform)
    {
        stateName = STATE.CHASE;

    }
    public override void EnterMethod()
    {
        animator.SetTrigger("isRunning");
        base.EnterMethod();

    }
    public override void UpdateMethod()
    {
        if (CanSeePlayer())
        {
            nextState = new Attack(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
        else
        {
            nextState = new Wonder(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void ExitMethod()
    {
        animator.ResetTrigger("isWalking");
        base.ExitMethod();
    }
}

public class Attack : State
{
    public Attack(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform) : base(_zombie, _agent, _animator, _playerTransform)
    {
        stateName = STATE.ATTACK;

    }
    public override void EnterMethod()
    {
        animator.SetTrigger("isAttacking");
        base.EnterMethod();

    }
    public override void UpdateMethod()
    {
        if (CanSeePlayer())
        {
            nextState = new Attack(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
        else
        {
            nextState = new Wonder(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void ExitMethod()
    {
        animator.ResetTrigger("isWalking");
        base.ExitMethod();
    }
}

