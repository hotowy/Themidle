using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField] 
    private ParticleSystem hitEffect;

    [FormerlySerializedAs("movespeed")] 
    [SerializeField] 
    private float moveSpeed = 0.7f;
    
    [SerializeField] 
    private float maxMoveSpeed = 40f;

    [SerializeField] 
    private int damage = 1;
    
    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionCurve;
    private AnimationCurve speedAnimationCurve;
    
    private Vector3 trajectoryStartPoint;
    private float trajectoryMaxHeight;

    [SerializeField]
    private float distanceToTargetToDestroyProjectile = 0.004f;

    private Vector3 lastKnownTargetLocation;
    private GameObject lastKnownTarget;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trajectoryStartPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (!lastKnownTarget)
            {
                lastKnownTarget = target.gameObject;
                lastKnownTargetLocation = target.position;   
            }
            else if (lastKnownTarget == target.gameObject)
            {
                lastKnownTargetLocation = target.position;
            }
        }
        
        UpdateProjectilePosition();

        var distance = Vector3.Distance(transform.position, lastKnownTargetLocation);

        if (distance < distanceToTargetToDestroyProjectile)
        {
            if (target)
            {
                var enemy = target.GetComponent<Enemy>();
                if (enemy)
                {
                    Instantiate(hitEffect, enemy.transform.position, Quaternion.FromToRotation(transform.position,lastKnownTargetLocation));
                    enemy.Hurt(damage);
                }  
            }
            else
            {
                var enemyInRangeWhenProjectileDies = Utils.GetClosestEnemy(transform.position, distanceToTargetToDestroyProjectile);
                if (enemyInRangeWhenProjectileDies) 
                {
                    Instantiate(hitEffect, enemyInRangeWhenProjectileDies.transform.position, Quaternion.FromToRotation(transform.position,lastKnownTargetLocation));
                    enemyInRangeWhenProjectileDies.Hurt(damage);
                }
            }
            
            Destroy(gameObject);
        }
    }

    private void UpdateProjectilePosition()
    {
        Vector3 trajectoryRange = lastKnownTargetLocation - trajectoryStartPoint;

        if (trajectoryRange.x < 0) // means "other side"
        {
            moveSpeed = -moveSpeed;
        }
        
        float nextPositionX = transform.position.x + moveSpeed * Time.deltaTime;
        float nextPositionXNormalized = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;
        
        float nextPositionYNormalized = trajectoryAnimationCurve.Evaluate(nextPositionXNormalized);
        
        float nextPositionYCorrectionNormalized = axisCorrectionCurve.Evaluate(nextPositionXNormalized);
        float nextPositionYCorrectionAbsolute = nextPositionYCorrectionNormalized * trajectoryRange.y;

        float nextPositionY = trajectoryStartPoint.y + nextPositionYNormalized * trajectoryMaxHeight + nextPositionYCorrectionAbsolute;
        
        Vector3 nextPosition = new Vector3(nextPositionX, nextPositionY, 0);

        CalculateProjectileSpeed(nextPositionXNormalized);
        
        transform.position = nextPosition;
    }

    private void CalculateProjectileSpeed(float nextPositionXNormalized)
    {
        float nextMoveSpeedNormalized = speedAnimationCurve.Evaluate(nextPositionXNormalized);
        moveSpeed = nextMoveSpeedNormalized * maxMoveSpeed; // / (target.position.y - trajectoryStartPoint.y);
    }

    public void InitializeProjectile(Transform target, float maxMoveSpeed, float trajectoryMaxHeight)
    {
        this.target = target;
        this.maxMoveSpeed = maxMoveSpeed;
        this.trajectoryMaxHeight = trajectoryMaxHeight;
    }

    public void InitializeTrajectoryAnimationCurve(AnimationCurve curve, AnimationCurve axisCorrectionCurve, AnimationCurve speedAnimationCurve)
    {
        trajectoryAnimationCurve = curve;
        this.axisCorrectionCurve = axisCorrectionCurve;
        this.speedAnimationCurve = speedAnimationCurve;
    }
}
