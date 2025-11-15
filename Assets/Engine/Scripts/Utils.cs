using UnityEngine;

public static class Utils
{
    public static class Header
    {
        public const string Separator = " \r \r \r ";
        public const string Debug = prefix + "DEBUG" + suffix;

        private const string prefix = "                 . . : : ";
        private const string suffix = " : : . . ";
    }

    public static Enemy GetClosestEnemy(Vector2 origin, float range)
    {
        return GetClosest<Enemy>(origin, range, LayerMask.GetMask("Enemy"));
    }
    
    
    /// <summary>
    /// Finds the closest component of type T within a given radius and layer mask.
    /// </summary>
    /// <typeparam name="T">The component type to search for (e.g. Enemy, Collectible, etc.)</typeparam>
    /// <param name="center">The position to search from.</param>
    /// <param name="circleRange">The search radius.</param>
    /// <param name="colliderLayerMask">Which layers to include in the search.</param>
    /// <returns>The closest component of type T, or null if none found.</returns>
    public static T GetClosest<T>(Vector2 center, float circleRange, LayerMask colliderLayerMask) where T : Component
    // TODO : Could be speed up?
    {
        Collider2D[] overlapCircleAllCollidersHits = Physics2D.OverlapCircleAll(center, circleRange, colliderLayerMask);

        float closestDistance = Mathf.Infinity;
        T closestHit = null;

        foreach (var hit in overlapCircleAllCollidersHits)
        {
            if (hit.TryGetComponent(out T targetTypeHit))
            {
                float dist = Vector2.SqrMagnitude((Vector2)hit.transform.position - center);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestHit = targetTypeHit;
                }
            }
        }

        return closestHit;
    }
}