using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Interface <c>IHasCooldown</c> is used for objects
    /// that need a cooldown.
    /// </summary>
    /// <example>
    /// Used to spawn objects with a cooldown in 
    /// between each spawn
    /// </example>
    public interface IHasCooldown
    {
        int Id { get; }
        float CooldownDuration { get; }
    }
}
