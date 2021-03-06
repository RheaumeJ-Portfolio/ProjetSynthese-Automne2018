﻿using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CollectablesUI : MonoBehaviour
    {
        [SerializeField] private Text numberText;
        private GameController gameController;

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
            gameController.OnLevelChange += UpdateCollectableUI;
        }

        public void UpdateCollectableUI()
        {
            numberText.text = gameController.CollectableAquiered.ToString();
        }
    }
}