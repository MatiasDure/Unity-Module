using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>ModifyUIText</c> is used update text in the 
    /// game
    /// </summary>
    public class ModifyUIText : EventListener
    {
        [SerializeField] private TextMeshProUGUI uiText;
        [SerializeField] private string textToDisplayInfront;
        [SerializeField] private string textToDisplayBehind;
        [SerializeField] private Type typeOfUI;

        private string text = "";

        /// <summary>
        /// This enum declares the type of ui
        /// </summary>
        /// <remarks>
        /// Can be expanded
        /// </remarks>
        private enum Type
        {
            lives,
            money,
            keys
        }

        private void Awake()
        {
            if (uiText == null) GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            SetText();
            UpdateText(text);
        }

        /// <summary>
        /// This method sets the new text to update on screen
        /// </summary>
        private void SetText()
        {
            switch (typeOfUI)
            {
                case Type.money:
                    text = ""+PlayerMoney.Money.AmountMoney;
                    break;
                case Type.lives:
                    text = ""+GameManager.Manager.Lives;
                    break;
                case Type.keys:
                    text = ""+Inventory.Locker.KeysInLocker;
                    break;
                default:
                    break;
            }
            text = textToDisplayInfront + text + textToDisplayBehind;
        }

        /// <summary>
        /// This method updates the text on the screen
        /// </summary>
        /// <param name="text">Text to output on screen</param>
        private void UpdateText(string text) => uiText.SetText(text);

        /// <summary>
        /// This method modifies the text passed through
        /// the inspector in case of an event
        /// </summary>
        /// <remarks>
        /// Currently only for the money, but can be expanded
        /// </remarks>
        private void ModifyTextFromInspector()
        {
            if (typeOfUI == Type.money) textToDisplayBehind = "";
        }

        protected override void SubscribeToEvent() => PurchaseButtonBehavior.OnHousePurchased += ModifyTextFromInspector;

        protected override void UnsubscribeFromEvent() => PurchaseButtonBehavior.OnHousePurchased += ModifyTextFromInspector;

    }
}
