using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Interface <c>ICanDrop</c> is used for objects
    /// that need to check whether they fall from 
    /// the boundary
    /// </summary>
    public interface ICanDrop
    {
        int DropLimit { get; }
        bool DropBoundary(float objPositionY) => objPositionY < DropLimit;
    }
}
