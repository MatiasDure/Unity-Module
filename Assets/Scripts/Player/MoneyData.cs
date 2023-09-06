using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyData : MonoBehaviour
{
    /// <summary>
    /// Class <c>MoneyData</c> contains the data for 
    /// the money
    /// </summary>
    [SerializeField] private int _amountWorth;

    public int AmountWorth { get => _amountWorth; }
}
