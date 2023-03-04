using System;
using System.Collections;
using System.Collections.Generic;
using MyNamespace.Units;
using MyNamespace.Utils.Math;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace MyNamespace.UI
{
    public class UnitsManagementUI : MonoBehaviour
    {
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] private TextMeshProUGUI unitsCountText;

        public void SpawnUnitsAtRandomCirclePosition(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                SpawnUnitAtRandomCirclePosition();   
            }

            UpdateUnitsCountText();
        }

        private void SpawnUnitAtRandomCirclePosition()
        {
            var position = MathUtils.GetRandomPointOnCircleXZ(unitsManager.Center, unitsManager.Radius);
            unitsManager.SpawnUnit(position, quaternion.identity);
        }

        private void UpdateUnitsCountText()
        {
            unitsCountText.text = unitsManager.ActiveUnitsCount.ToString();
        }
    }
}