using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class Archer : MonoBehaviour
{
    [Header(Utils.Header.Debug)]
    
    [SerializeField]
    private float distanceToTarget;
    [SerializeField] 
    private float angle;
    [SerializeField] 
    private float angleDistance;
    [SerializeField] 
    private float calculatedProjectileMaxHeightBasedOnDistance;
    [SerializeField]
    private float calculatedProjectileMaxSpeedBasedOnDistance;
    
    [Header(Utils.Header.Separator)] 

    [SerializeField] 
    private Projectile projectilePrefab;

    [SerializeField] 
    private Transform target;

    [SerializeField]
    private float shootRate;

    [SerializeField] 
    private float range = 2f;

    [FormerlySerializedAs("projectileMoveSpeed")] 
    [SerializeField] 
    private float projectileMaxMoveSpeed = 1f;
    
    [SerializeField] 
    private float projectileMaxHeight = 1.5f;
    
    [SerializeField] 
    private float projectileMoveSpeedCap = 1f;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve speedAnimationCurve;

    private LineRenderer lr;

    private float shootTimer;
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootTimer = Random.Range(0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            shootTimer = shootRate;

            if (!target)
            {
                var closestEnemyFound = GetClosestEnemy();
                if (closestEnemyFound)
                {
                    target = closestEnemyFound.transform;
                }
                // Gizmos.color = Color.red;
                // Gizmos.DrawWireSphere(transform.position, range);
            }
            if(target)
            {
                ShootAtTarget();
            }
        }
    }

    private void ShootAtTarget()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        
        calculatedProjectileMaxHeightBasedOnDistance = projectileMaxHeight * distanceToTarget / 4;
        
        Vector2 direction = target.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angleDistance = Mathf.Abs(angle - 90f);
        
        calculatedProjectileMaxSpeedBasedOnDistance = projectileMaxMoveSpeed * angleDistance / 50;
        // if (calculatedProjectileMaxSpeedBasedOnDistance > projectileMoveSpeedCap)
        // {
        //     calculatedProjectileMaxSpeedBasedOnDistance = projectileMoveSpeedCap;
        // }
        
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.InitializeProjectile(target, calculatedProjectileMaxSpeedBasedOnDistance, calculatedProjectileMaxHeightBasedOnDistance);
        projectile.InitializeTrajectoryAnimationCurve(trajectoryAnimationCurve,axisCorrectionAnimationCurve, speedAnimationCurve);

    }
    
    Enemy GetClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));

        float minDist = Mathf.Infinity;
        Enemy closest = null;

        foreach (Collider2D hit in hits)
        {
            if (!hit.GetComponent<Enemy>())
            {
                continue;
            }
            
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.GetComponent<Enemy>();
            }
        }
        return closest;
    }
}