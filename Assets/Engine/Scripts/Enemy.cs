using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Movement behaviour:")]
    [SerializeField]
    private float collisionRaycastSpreadAxisX = 0.25f;
    
    [SerializeField]
    private float collisionRaycastDistance = 0.8f;
    
    [SerializeField]
    [FormerlySerializedAs("avoidDirectionMultiplier")] 
    private float avoidDirectionMoveMultiplier = 0.7f;

    [Header(Utils.Header.Separator)] 
    
    [SerializeField]
    private ResourceType dropType;

    [SerializeField] 
    [Range(0, 10)] 
    private int dropAmount = 1;
    
    [SerializeField]
    private float dropChance = 0.1f;
    
    [SerializeField] 
    private ParticleSystem deathParticleAnimation;
    
    
    public int hp = 2;
    public int damage = 1;
    public float moveForce = 5f; // SLOW try 0.4
    public float maxSpeed = 3f; // SLOW try 0.5
    
    private AttackPoint castleGate;
    public LayerMask obstacleMask;
    private Rigidbody2D rb;



    private void Start()
    {
        castleGate = FindFirstObjectByType<AttackPoint>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (castleGate)
        { 
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector2 dirToTarget = ((Vector2)castleGate.transform.position - rb.position).normalized;
        
        // DISABLED FOR NOW // dirToTarget = AdjustDirectionByAvoidingObstacles(dirToTarget);
        
        rb.AddForce(dirToTarget * moveForce, ForceMode2D.Force);
        
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public void Hurt(int damageDealt)
    {
        if (hp <= damageDealt)
        {
            Die();
        }
        else
        {
            hp -= damageDealt;
        }
    }

    private void Die()
    {
        AnimateDeath();
        DropAward();
        Destroy(this.gameObject);
    }

    private void AnimateDeath()
    {
        if (deathParticleAnimation)
        {
            Instantiate(deathParticleAnimation, transform.position, Quaternion.identity);
        }
    }

    private void DropAward()
    {
        if (dropType && dropAmount > 0 && UnityEngine.Random.Range(0f, 1f) <= dropChance)
        {
            DropAnimation.InstantiateAnimation(dropType, dropAmount, rb.position);
            dropType.amount += dropAmount;
        }
    }

    private Vector2 AdjustDirectionByAvoidingObstacles(Vector2 dirToTarget)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, dirToTarget, collisionRaycastDistance, obstacleMask);
        //Debug.DrawRay(rb.position,dirToTarget,Color.red);
        RaycastHit2D hitL = Physics2D.Raycast(rb.position, dirToTarget + Vector2.left * collisionRaycastSpreadAxisX, collisionRaycastDistance, obstacleMask);
        //Debug.DrawRay(rb.position,dirToTarget + Vector2.left * collisionRaycastSpreadAxisX,Color.red);
        RaycastHit2D hitR = Physics2D.Raycast(rb.position, dirToTarget + Vector2.right * collisionRaycastSpreadAxisX, collisionRaycastDistance, obstacleMask);
        //Debug.DrawRay(rb.position,dirToTarget + Vector2.right * collisionRaycastSpreadAxisX,Color.red);
        
        if (hitL.collider)
        {
            Vector2 avoidDir = Vector2.Perpendicular(hitL.normal).normalized;
            dirToTarget = (dirToTarget + -avoidDir * avoidDirectionMoveMultiplier).normalized;
        }
        
        if (hitR.collider)
        {
            Vector2 avoidDir = Vector2.Perpendicular(hitR.normal).normalized;
            dirToTarget = (dirToTarget + avoidDir * avoidDirectionMoveMultiplier).normalized;
        }
        
        if (!hitL.collider && !hitR.collider && hit.collider)
        {
            Vector2 avoidDir = Vector2.Perpendicular(hit.normal).normalized;
            dirToTarget = (dirToTarget + avoidDir * avoidDirectionMoveMultiplier*2).normalized;
        }

        return dirToTarget;
    }
}
