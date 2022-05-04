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
    GameObject playerPrefab;

    public GameObject zombieRagdoll;
    void Start()
    {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
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
    int damageAmount = 5;
    public void DamagePlayer()
    {
        if ( playerPrefab!= null)
        {
            playerPrefab.GetComponent<PlayerMovement>().TakeHit(damageAmount);
        }
        //create a method name random sound,when player takes damageS   
    }

    public void KillZombie()
    {
        animator.SetBool("isDead", true);
        //this.gameObject.SetActive(false);
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

    public float walkingSpeed = 1f;
    public float runningSpeed = 3f;

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
    public float DistanceToPlayer()
    {
        return Vector3.Distance( zombie.transform.position, playerTransform.position);
    }
    public bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 12f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanNotSeePlayer()
    {
        if (DistanceToPlayer() > 12f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void TurnOffAllTriggerAnim()//All animation are off
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isDead", false);
    }
   
}

public class Idle : State
{
    public Idle(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform) : base(_zombie, _agent, _animator, _playerTransform)
    {
        stateName = STATE.IDLE;
        agent.enabled = true;

    }
    public override void EnterMethod()
    {
        animator.SetBool("isWalking", true);
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
        animator.SetBool("isWalking",false);
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
        animator.SetBool("isWalking",true);
        base.EnterMethod();

    }
    public override void UpdateMethod()
    {
            float randValueX = zombie.transform.position.x + Random.Range(-5f, 5f);
            float randValueZ = zombie.transform.position.z + Random.Range(-5f, 5f);
            float ValueY = zombie.transform.position.y;
            Vector3 destination = new Vector3(randValueX, ValueY, randValueZ);
            agent.SetDestination(destination);
            agent.stoppingDistance = 0f;
            agent.speed = walkingSpeed;
        
            if (CanSeePlayer())
            {
                nextState = new Chase(zombie, agent, animator, playerTransform);
                eventStage = EVENTS.EXIT;
            }
    }
    public override void ExitMethod()
    {
        animator.SetBool("isWalking",false);
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
        animator.SetBool("isRunning",true);
        base.EnterMethod();
    }
    public override void UpdateMethod()
    {
        agent.SetDestination(playerTransform.position);
        zombie.transform.LookAt(playerTransform.position);
        agent.stoppingDistance = 2f;
        agent.speed = runningSpeed;
       
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            nextState = new Attack(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
        if (CanNotSeePlayer())
        {
            nextState = new Wonder(zombie, agent, animator, playerTransform);
            agent.ResetPath();
            eventStage = EVENTS.EXIT;
        }
    }
    public override void ExitMethod()
    {
       animator.SetBool("isRunning",false);
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
        animator.SetBool("isAttacking", true);
        base.EnterMethod();
    }
    public override void UpdateMethod()
    {
        zombie.transform.LookAt(playerTransform.position);//Zombies should look at Player
        
        if (DistanceToPlayer() > agent.stoppingDistance + 1)
        {
            nextState = new Idle(zombie, agent, animator, playerTransform);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void ExitMethod()
    {
        animator.SetBool("isAttacking",false);
        base.ExitMethod();
    }
}

//public class Dead : State
//{
//    float time;
//    public Dead(GameObject _zombie, NavMeshAgent _agent, Animator _animator, Transform _playerTransform) : base(_zombie, _agent, _animator, _playerTransform)
//    {
//        stateName = STATE.DEATH;

//    }
//    public override void EnterMethod()
//    {
//        //animator.SetBool("isDead", true);
//        base.EnterMethod();
//    }
//    public override void UpdateMethod()
//    {
//        time = time + Time.deltaTime;
//        if (time > 4f)
//        {
//            time = 0f;
//            nextState = new Idle(zombie, agent, animator, playerTransform);
//            eventStage = EVENTS.EXIT;
//        }
//    }
//    public override void ExitMethod()
//    {
//        animator.SetBool("isDead", false);
//        base.ExitMethod();
//    }
//}

