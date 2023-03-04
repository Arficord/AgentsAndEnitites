using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace.Pooling;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace MyNamespace.Units
{
    public class UnitsManager : MonoBehaviour
    {
        
        [SerializeField] private UnitController unitPrefab;
        [SerializeField] private float radius = 10f;
        [SerializeField] private Vector3 center = Vector3.zero;

        //TODO: delete. Currently here for debug purpose
        [SerializeField] private Transform debugMoveTarget;

        private ObjectPool<UnitController> unitsPool;

        
        private void Awake()
        {
            unitsPool = new ObjectPool<UnitController>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                OnDestroyPoolObject);
        }

        public void SpawnUnit()
        {
            var takenUnit = unitsPool.Get();
            var spawnPoint = GetRandomSpawnPointOnCircle();
            var rotation = Quaternion.identity;
            takenUnit.SetPositionAndRotation(spawnPoint, rotation);
            takenUnit.DebugTarget = debugMoveTarget;
        }

        private Vector3 GetRandomSpawnPointOnCircle()
        {
            var pointOnCircle = Random.insideUnitCircle.normalized * radius;
            return new Vector3(center.x + pointOnCircle.x, center.y, center.z + pointOnCircle.y);
        }

        #region Pooling
        private UnitController CreatePooledItem()
        {
            return GameObject.Instantiate(unitPrefab);
        }

        private void OnTakeFromPool(UnitController unit)
        {
            unit.OnTakeFromPoolRequest();
        }

        private void OnReturnedToPool(UnitController unit)
        {
            unit.OnReturnedToPoolRequest();
        }

        private void OnDestroyPoolObject(UnitController unit)
        {
            unit.OnDestroyRequest();
        }

        #endregion

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(center, Vector3.up, radius);
        }
        #endif
    }
}
