using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace.Pooling;
using MyNamespace.Utils.Math;
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
        
        public int ActiveUnitsCount => unitsPool.CountActive;
        public float Radius => radius;
        public Vector3 Center => center;

        public UnitLookRule UnitLookRule
        {
            get => _unitLookRule;
            set
            {
                _unitLookRule = value;
                foreach (var unit in activeUnits)
                {
                    unit.ApplyLookRule(_unitLookRule);
                }
            }
        }
        
        private ObjectPool<UnitController> unitsPool;
        private List<UnitController> activeUnits = new List<UnitController>();
        
        private Crowd _unitCrowd;
        private UnitLookRule _unitLookRule;
        
        private void Awake()
        {
            unitsPool = new ObjectPool<UnitController>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                OnDestroyPoolObject);
            _unitCrowd = new Crowd(center, radius);
        }

        public void SpawnUnit(Vector3 position, Quaternion rotation)
        {
            var unit = unitsPool.Get();
            activeUnits.Add(unit);
            unit.SetPositionAndRotation(position, rotation);
            unit.SetCrowd(_unitCrowd);
            unit.ApplyLookRule(_unitLookRule);
        }

        public void RemoveAllUnits()
        {
            for (int i = ActiveUnitsCount - 1; i >= 0; i--)
            {
                RemoveUnit(i);
            }
        }
        
        public void RemoveUnit(int index)
        {
            if (index >= ActiveUnitsCount || index < 0)
            {
                Debug.LogError($"Tried to remove unit with index [{index}], while elements count is {ActiveUnitsCount}");
                return;
            }

            var unit = activeUnits[index];
            activeUnits.RemoveAt(index);
            unitsPool.Release(unit);
        }

        #region Pooling
        private UnitController CreatePooledItem()
        {
            return Instantiate(unitPrefab);
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
