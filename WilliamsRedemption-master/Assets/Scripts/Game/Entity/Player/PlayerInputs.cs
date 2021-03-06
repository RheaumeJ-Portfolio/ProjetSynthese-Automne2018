﻿using Game.Controller;
using UnityEngine;
using XInputDotNetPure;

namespace Game.Entity.Player
{
    public class PlayerInputs : MonoBehaviour
    {
        private Mover player;
        private PlayerIndex controllerNumber;
        private PlayerController playerController;

        public GamePadState ControllerState => controllerState;

        private GamePadState controllerState;
        private bool jumpButtonPressed;
        private GameController gameController;
        private bool pauseButtonPressed;
        private void Start()
        {
            player = GetComponent<Mover>();
            playerController = GetComponent<PlayerController>();
            controllerNumber = PlayerIndex.One;
            controllerState = GamePad.GetState(controllerNumber);
            gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GameController).GetComponent<GameController>();
            jumpButtonPressed = false;
            pauseButtonPressed = false;

        }

        private void Update()
        {
            if (!controllerState.IsConnected)
            {
                ManageKeyBoardInputs();
            }
            else
            {
                ManageControllerInputs();
            }
        }

        private void ManageKeyBoardInputs()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.Jump();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameController.OnPauseButtonPressed();
            }
            if (Input.GetKey(KeyCode.A))
            {
                player.MoveLeft();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                player.MoveRight();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) &&
                playerController.CurrentController.CapacityUsable())
            {
                playerController.CurrentController.UseCapacity();
            }

            if (Input.GetKey(KeyCode.Return) && playerController.CurrentController.CanUseBasicAttack())
            {
                playerController.CurrentController.UseBasicAttack();
            }
        }

        private void ManageControllerInputs()
        {
            controllerState = GamePad.GetState(controllerNumber);

            if (controllerState.Buttons.A == ButtonState.Pressed && !jumpButtonPressed)
            {
                player.Jump();
                jumpButtonPressed = true;
            }

            if (controllerState.Buttons.Start == ButtonState.Pressed && !pauseButtonPressed)
            {
                gameController.OnPauseButtonPressed();
                pauseButtonPressed = true;
            }

            if (controllerState.Buttons.Start == ButtonState.Released)
            {
                pauseButtonPressed = false;
            }
            if (controllerState.Buttons.A == ButtonState.Released)
            {
                jumpButtonPressed = false;
            }

            if (controllerState.ThumbSticks.Left.X <= -0.5)
            {
                player.MoveLeft();
            }
            else if (controllerState.ThumbSticks.Left.X >= 0.5)
            {
                player.MoveRight();
            }

            if ((controllerState.Buttons.X == ButtonState.Pressed||controllerState.Triggers.Left>0) &&
                playerController.CurrentController.CapacityUsable())
            {
                playerController.CurrentController.UseCapacity();
                
            }

            if ((controllerState.Buttons.B == ButtonState.Pressed||controllerState.Triggers.Right >0.1) &&
                playerController.CurrentController.CanUseBasicAttack())
            {
                playerController.CurrentController.UseBasicAttack();
            }

        }
    }
}