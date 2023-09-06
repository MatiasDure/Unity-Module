using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Assets.Scripts
{

    /// <summary>
    /// Class <c>NpcInteraction</c> is used for Npc objects.
    /// Contains methods to simplify interaction with them.
    /// </summary>
    public class NpcInteraction : EventListener
    {
        [SerializeField] private List<string> texts;
        [SerializeField] private GameObject canvasGameObject;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private MeshRenderer[] meshRenderers;

        private Canvas canvas;
        private int currentIndex;
        private Color startingColor;
        private Color newMessageColor;
        private Material mat;
        private bool firstNewMessage = true;

        public static event Action OnNewMessage;

        private void Awake()
        {
            //Retrieving the canvas to manipulate the display
            if (canvasGameObject == null) canvasGameObject = GameObject.Find("Canvas").gameObject;
            canvas = canvasGameObject.GetComponent<Canvas>();

            //Retrieving the text element to update the message
            if (text == null) text = canvas.GetComponentInChildren<TextMeshProUGUI>();

            //Retrieving the material of the icon to change the color
            if (meshRenderers.Length == 0) meshRenderers = GetComponentsInChildren<MeshRenderer>();
            
        }

        protected override void Start()
        {
            //subscribing to events
            base.Start();

            //hiding the canvas
            HideText();

            //Assigning the same material to all renderers to be able to modify them
            startingColor = meshRenderers[0].material.color;
            newMessageColor = Color.red;

            mat = new Material(Shader.Find("Standard"));

            foreach (MeshRenderer mr in meshRenderers) mr.material = mat;

            //Setting the text to display
            currentIndex = texts.Count - 1;
            SettingText(currentIndex);
            NewMessageAvailable();
        }

        /// <summary>
        /// This method displays the text on screen
        /// </summary>
        private void DisplayText()
        {
            mat.color = startingColor;
            canvas.enabled = true;
        }

        /// <summary>
        /// This method hides the text
        /// </summary>
        private void HideText() => canvas.enabled = false;

        /// <summary>
        /// This method changes the text to the previous text 
        /// on the list
        /// </summary>
        /// <remarks>
        /// It resembles a stack
        /// </remarks>
        private void ChangeText()
        {
            firstNewMessage = false;
            NewMessageAvailable();
            if (texts.Count <= 1) return;
            texts.RemoveAt(currentIndex);
            SettingText(--currentIndex);
        }

        /// <summary>
        /// This method sets the last string in 
        /// the list to the current text
        /// </summary>
        /// <param name="index">
        /// The index of the next message to display 
        /// from the list
        /// </param>
        private void SettingText(int index) => text.SetText(texts[index]);

        /// <summary>
        /// This method activates just when the message has 
        /// change to let know there has been a change
        /// </summary>
        private void NewMessageAvailable()
        {
            mat.color = newMessageColor;
            if (firstNewMessage) return;
            OnNewMessage?.Invoke();
        }

        /// <summary>
        /// This method listens to an event sent by the player
        /// which lets them know if the player is colliding with the object
        /// or has left the object
        /// </summary>
        /// <param name="sender">The object that triggered the event</param>
        /// <param name="onExit">
        /// Whether the object colliding left 
        /// or is still colliding with the npc
        /// </param>
        private void InteractedWithPlayer (System.Object sender, bool onExit)
        {
            if (onExit) HideText();
            else DisplayText();
        }

        protected override void SubscribeToEvent()
        {
            PlayerCollisionDetection.OnNpcInteraction += InteractedWithPlayer;
            PurchaseButtonBehavior.OnCollectEnoughMoney += UnsubscribeAndChangeText;
            PurchaseButtonBehavior.OnHousePurchased += ChangeText;
            DoorControls.OnDoorOpen += ChangeText;
        }

        protected override void UnsubscribeFromEvent()
        {
            PlayerCollisionDetection.OnNpcInteraction -= InteractedWithPlayer;
            PurchaseButtonBehavior.OnHousePurchased -= ChangeText;
            DoorControls.OnDoorOpen -= ChangeText;
        }

        private void UnsubscribeAndChangeText()
        {
            PurchaseButtonBehavior.OnCollectEnoughMoney -= UnsubscribeAndChangeText;
            ChangeText();
        }

    }
}
