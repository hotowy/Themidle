using UnityEngine;

public class Utils
{
    public class Header
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
    /// <param name="origin">The position to search from.</param>
    /// <param name="range">The search radius.</param>
    /// <param name="layerMask">Which layers to include in the search.</param>
    /// <returns>The closest component of type T, or null if none found.</returns>
    public static T GetClosest<T>(Vector2 origin, float range, LayerMask layerMask) where T : Component
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, range, layerMask);

        float minDist = Mathf.Infinity;
        T closest = null;

        foreach (var hit in hits)
        {
            T comp = hit.GetComponent<T>();
            if (comp == null)
            {
                continue;
            }

            float dist = Vector2.SqrMagnitude((Vector2)hit.transform.position - origin);
            if (dist < minDist)
            {
                minDist = dist;
                closest = comp;
            }
        }

        return closest;
    }
}