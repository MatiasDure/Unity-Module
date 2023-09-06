using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>EnemyData</c> contains data for the enemies
    /// </summary>
    public class EnemyData : MonoBehaviour
    {
        [SerializeField] private int _amountMoneyGrab;
        public int AmountMoneyGrab { get => _amountMoneyGrab; }
    }
}
