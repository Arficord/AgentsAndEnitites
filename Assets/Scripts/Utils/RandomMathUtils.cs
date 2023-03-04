using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace.Utils.Math
{
    public static class RandomMathUtils
    {
        public static Vector3 GetRandomPointOnCircleXZ(Vector3 center, float radius)
        {
            var pointOnCircle = Random.insideUnitCircle.normalized * radius;
            return new Vector3(center.x + pointOnCircle.x, center.y, center.z + pointOnCircle.y);
        }
        
        public static Vector3 GetRandomPointInCircleXZ(Vector3 center, float radius)
        {
            var pointOnCircle = Random.insideUnitCircle* radius;
            return new Vector3(center.x + pointOnCircle.x, center.y, center.z + pointOnCircle.y);
        }
    }
}
