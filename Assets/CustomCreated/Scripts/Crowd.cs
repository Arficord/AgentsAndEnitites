using System.Collections;
using System.Collections.Generic;
using MyNamespace.Units;
using MyNamespace.Utils.Math;
using Pathfinding;
using UnityEngine;

public class Crowd
{
    private Vector3 _center;
    private float _radius;
    
    public Crowd(Vector3 center, float radius)
    {
        _center = center;
        _radius = radius;
    }
    
    public void SetNewDestination(UnitController unit)
    {
        var destination = RandomMathUtils.GetRandomPointInCircleXZ(_center, _radius);
        unit.SetDestination(destination);
    }

    public void OnPathError(UnitController unit, Path path)
    {
        Debug.LogError(path.error);
        SetNewDestination(unit);
    }
    
    public void OnReachedDestination(UnitController unit)
    {
        SetNewDestination(unit);
    }
}
