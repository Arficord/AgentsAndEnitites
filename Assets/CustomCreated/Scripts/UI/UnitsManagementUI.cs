using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace.Units;
using MyNamespace.Utils.Math;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace.UI
{
    public class UnitsManagementUI : MonoBehaviour
    {
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] private TextMeshProUGUI unitsCountText;
        [SerializeField] private Toggle useAnimationsToggle;
        
        private void Start()
        {
            useAnimationsToggle.isOn = unitsManager.UnitLookRule.UseAnimations;
            useAnimationsToggle.onValueChanged.AddListener(OnAnimatorToggleValueChanged);
        }

        public void SpawnUnitsAtRandomCirclePosition(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                SpawnUnitAtRandomCirclePosition();   
            }

            UpdateUnitsCountText();
        }

        public void RemoveLastUnits(int amount)
        {
            var unitsAmount = unitsManager.ActiveUnitsCount;
            var lastIndex = unitsAmount - amount;

            if (lastIndex < 0)
            {
                lastIndex = 0;
            }
            
            for (int i = unitsAmount - 1; i >= lastIndex; i--)
            {
                unitsManager.RemoveUnit(i);   
            }

            UpdateUnitsCountText();
        }

        public void RemoveAllUnits()
        {
            unitsManager.RemoveAllUnits();
            UpdateUnitsCountText();
        }

        private void SpawnUnitAtRandomCirclePosition()
        {
            var position = RandomMathUtils.GetRandomPointOnCircleXZ(unitsManager.Center, unitsManager.Radius);
            unitsManager.SpawnUnit(position, quaternion.identity);
        }

        private void UpdateUnitsCountText()
        {
            unitsCountText.text = unitsManager.ActiveUnitsCount.ToString();
        }

        private void OnAnimatorToggleValueChanged(bool flag)
        {
            var lookRule = unitsManager.UnitLookRule;
            lookRule.UseAnimations = flag;
            unitsManager.UnitLookRule = lookRule;
        }
    }
}