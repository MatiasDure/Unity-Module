using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>CooldownData</c> is used for cases where a 
    /// cooldown is required
    /// </summary>
    public class CooldownData
    {

        /// <summary>
        /// Class <c>CooldownData</c> takes in a class
        /// which inherits from the interface IHasCooldown
        /// </summary>
        public CooldownData(IHasCooldown cooldown)
        {
            Id = cooldown.Id;
            RemainingTime = cooldown.CooldownDuration;
        }

        public int Id { get; }
        public float RemainingTime { get; private set; }
        public bool destroy = false;

        /// <summary>
        /// This method decreases the cooldown time
        /// </summary>
        public bool DecreaseCooldown(float deltaTime)
        {
            RemainingTime -= deltaTime;
            return RemainingTime <= 0f;
        }
    }
}
