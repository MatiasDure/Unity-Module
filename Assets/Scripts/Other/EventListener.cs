using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>EventKustener</c> is an abstract class that
    /// contains default code for other classes that are going to be 
    /// listening for events
    /// </summary>
    public abstract class EventListener : MonoBehaviour
    {

        /// <summary>
        /// This method subscribes subclasses to events
        /// </summary>
        protected virtual void Start() => SubscribeToEvent();

        protected virtual void SubscribeToEvent() { }
        protected virtual void UnsubscribeFromEvent() { }

        /// <summary>
        /// This method unsubscribes subclasses from events 
        /// when they get destroyed
        /// </summary>
        protected virtual void OnDestroy() => UnsubscribeFromEvent();

    }
}
