using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{

    /// <summary>
    /// Class <c>PopUps</c> is used to 
    /// display a pop up on screen
    /// </summary>
    public class PopUps : EventListener
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private Canvas canvas;
        [SerializeField] private int durationOfPopUp;
        [SerializeField] private string messageToDisplay;

        private void Awake()
        {
            if(canvas == null) canvas = GetComponent<Canvas>();

            if (textField == null) textField = GetComponentInChildren<TextMeshProUGUI>();

            //hiding the canvas
            HideNotification();
        }

        void Update()
        {
            if (textField.text == "") return;

            //Displays the notification for a certain amount of time
            StartCoroutine(DisplayNotification());
        }

        /// <summary>
        /// This method is used to display a notification for a certain
        /// amount of seconds. Uses the yield return keyword to indicate that
        /// it is an iterator.
        /// </summary>
        private IEnumerator DisplayNotification()
        {
            ShowNotification();

            //yields a new WaitForSeconds until the timer goes down
            yield return new WaitForSeconds(durationOfPopUp);

            //resets the text and hides the notification
            textField.text = "";
            HideNotification();
        }

        /// <summary>
        /// This method changes the text to the text passed 
        /// in the inspector
        /// </summary>
        private void ChangeText() => textField.text = messageToDisplay;

        /// <summary>
        /// This method hides the pop up
        /// </summary>
        private void HideNotification() => canvas.enabled = false;

        /// <summary>
        /// This method shows the pop up
        /// </summary>
        private void ShowNotification() => canvas.enabled = true;

        protected override void SubscribeToEvent() => NpcInteraction.OnNewMessage += ChangeText;

        protected override void UnsubscribeFromEvent() => NpcInteraction.OnNewMessage -= ChangeText;
    }
}
