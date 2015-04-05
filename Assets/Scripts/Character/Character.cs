using UnityEngine;
using System.Collections;

public class Character : Unit 
{
    public enum CareerType
    {
        Melee,
        Caster,
        Archer,
    }

    public CareerType careerType;

    protected StateMachine stateMachine;

    public float runSpeed = 1;
    public float attackRange = 2;
    public float attackInterval = 1.0f;
    [HideInInspector]
    public float lastAttackTime = 0;
     [HideInInspector]
    public bool hitBack = false;
    private Vector2 velocity = Vector2.zero;
    public int characterIdex;
    [HideInInspector]
    public SkillCastInfo currentSkillCast;


    public void SetVelocity(Vector2 v)
    {
        velocity = v;
    }

    [SerializeField, SetProperty("Facing")]
    private int facing = 1; //-1 <- ; 1 ->
    public int Facing
    {
        get
        {
            return facing;
        }
        set
        {
            int val = value >= 0 ? 1 : -1;
            facing = val;
            if (facing == 1)
            {
                transform.localEulerAngles = new Vector3(0, -180, 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    void FixedUpdate()
    {
        if(!hitBack)
            rigidbody2D.velocity = velocity;
    }


    public override void init()
    {
        base.init();
        stateMachine = GetComponent<StateMachine>();
        if (stateMachine == null)
        {
            gameObject.AddComponent<StateMachine>();
        }
        stateMachine.AddState(new Idle());
        stateMachine.AddState(new Run());
        stateMachine.AddState(new Attack());
        stateMachine.AddState(new SkillCast());

        if (GetComponent<SkillController>() == null)
        {
            gameObject.AddComponent<SkillController>();
        }
    }



    public void Idle()
    {
        stateMachine.ChangeState("Idle", null);
    }

    public void Run(bool forth)
    {
        stateMachine.ChangeState("Run", forth);
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime <= attackInterval)
            return;

        lastAttackTime = Time.time;
        stateMachine.ChangeState("Attack", null);
    }

    public void SkillCast(SkillCastInfo info)
    {
        currentSkillCast = info;
        stateMachine.ChangeState("SkillCast", info);
    }

    public void Update()
    {

    }

	public bool drawGizmo = true;

    public void OnDrawGizmos()
    {
		if(!drawGizmo)
			return;
        Vector3 myPos = transform.position;
        Gizmos.DrawWireSphere(myPos, attackRange);
    }
}
