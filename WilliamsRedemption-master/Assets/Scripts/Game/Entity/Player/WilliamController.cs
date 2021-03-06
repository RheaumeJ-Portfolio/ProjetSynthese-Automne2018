﻿using System.Collections;
using Game.Controller.Events;
using UnityEngine;

namespace Game.Entity.Player
{
    public class WilliamController : EntityController
    {
        [Tooltip("Distance travelled by the player during a dash.")] [SerializeField]
        private float dashDistance;

        [Tooltip("Speed at witch the player dashes.")] [SerializeField]
        private float dashSpeed;

        [SerializeField] private GameObject projectile;

        [Tooltip("Amount of time between bullets.")] [SerializeField]
        private float fireRate;

        [Tooltip("Amount of time between dashes.")] [SerializeField]
        private float DashCoolDown;

        private PlayerController player;
        private bool capacityCanBeUsed;
        private float? lastTimeAttack = null;
        private float timerStartTime;
        private Animator animator;
        private PlayerShootEventChannel shootEventChannel;

        private void Start()
        {
            player = GetComponentInParent<PlayerController>();
            timerStartTime = 0;
            capacityCanBeUsed = true;
            animator = GetComponent<Animator>();
            capacityCanBeUsed = true;
            shootEventChannel = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<PlayerShootEventChannel>();


        }

        public override void UseCapacity()
        {
            StartCoroutine(Dash(player.playerHorizontalDirection));
            capacityCanBeUsed = false;
            timerStartTime = Time.time;
        }

        public override bool CapacityUsable()
        {
            if (capacityCanBeUsed)
            {
                return true;
            }

            if (!capacityCanBeUsed && (Time.time - timerStartTime) >= DashCoolDown)
            {
                capacityCanBeUsed = true;
                return true;
            }

            return false;
        }

        private IEnumerator Dash(Vector2 direction)
        {
            animator.SetTrigger(Values.AnimationParameters.Player.Dash);
            player.LockTransformation();
            player.IsDashing = true;

            Transform root = transform.parent;

            RaycastHit2D hit =
                Physics2D.Raycast(
                    root.position,
                    direction, dashDistance,
                    player.WilliamLayerMask);
            Debug.DrawLine(root.position,
                new Vector3(root.position.x + dashDistance * direction.x, root.position.y, root.position.z),
                Color.yellow,
                10);

            if (hit.collider == null)
            {
                hit.point = new Vector2(dashDistance * direction.x + transform.position.x, transform.position.y);
            }

            float distance = Vector2.Distance(hit.point - GetComponent<BoxCollider2D>().size, transform.position);

            float duration = distance / dashSpeed;

            float time = 0;


            while (duration > time)
            {
                Vector2 temp = Vector2.right * direction.x *
                               dashSpeed;
                time += Time.deltaTime;
                player.kRigidBody.VelocityModifier =
                    temp; //set our rigidbody velocity to a custom velocity every frame.
                yield return new WaitForFixedUpdate();
            }

            player.kRigidBody.VelocityModifier = Vector2.zero;
            player.IsDashing = false;
            player.UnlockTransformation();
            animator.SetTrigger(Values.AnimationParameters.Player.DashEnd);
            OnAttackFinish();
        }

        public override void UseBasicAttack()
        {
            animator.SetTrigger(Values.AnimationParameters.Player.Attack);
            Quaternion angle = Quaternion.identity;

            if (player.playerHorizontalDirection == Vector2.left)
                angle = Quaternion.AngleAxis(180, Vector3.up);

            if (player.playerHorizontalDirection == Vector2.down && !player.IsOnGround)
                angle = Quaternion.AngleAxis(-90, Vector3.forward);
            else if (player.playerHorizontalDirection == Vector2.up)
                angle = Quaternion.AngleAxis(90, Vector3.forward);

            GameObject projectileObject = Instantiate(projectile, gameObject.transform.position, angle);
            shootEventChannel.Publish(new OnPlayerShoot());
        }
    }
}