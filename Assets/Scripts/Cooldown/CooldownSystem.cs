using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>CooldownSystem</c> is used to process objects
    /// that are in cooldown
    /// </summary>
    public class CooldownSystem:MonoBehaviour
    {
        private readonly List<CooldownData> cooldowns = new List<CooldownData>();

        private void Update()
        {
            ProcessCooldown();
        }

        /// <summary>
        /// This method updates the time remaining in the cooldown object
        /// </summary>
        private void ProcessCooldown()
        {
            float deltaTime = Time.deltaTime;

            for (int i = cooldowns.Count - 1; i >= 0; i--)
            {
                if (cooldowns[i].DecreaseCooldown(deltaTime))
                {
                    cooldowns.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// This method check whether an object is on cooldown
        /// </summary>
        /// <param name="id">The cooldown unique id of the object to check</param>
        public bool IsOnCoolDown(int id)
        {
            foreach (CooldownData cooldown in cooldowns)
            {
                if(cooldown.Id == id) return true;
            }
            return false;
        }

        /// <summary>
        /// This method puts an IHasCooldown object on cooldown
        /// </summary>
        /// <param name="cooldown">The cooldown unique id of the object to check</param>
        public void PutOnCooldown(IHasCooldown cooldown)
        {
            cooldowns.Add(new CooldownData(cooldown));
        }
    }
}
