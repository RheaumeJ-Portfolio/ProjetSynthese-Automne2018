﻿using System.Collections;
using System.Runtime.Remoting.Messaging;
using Game.Controller;
using Game.Controller.Events;
using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private int scoreValue = 0;
        [SerializeField] private GameObject dmgEffect;

        protected Health health;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        protected PlayerController player;
        protected HitSensor hitSensor;
        private GameController gameController;
        private EnemyDeathEventChannel deathEventChannel;
        public int ScoreValue => scoreValue;
        public bool IsInvulnerable { get; set; }

        protected void Awake()
        {
            player = GameObject.FindWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            deathEventChannel = gameController.GetComponent<EnemyDeathEventChannel>();
            health = GetComponent<Health>();
            health.OnDeath += OnDeath;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            hitSensor = GetComponent<HitSensor>();
            hitSensor.OnHit += OnHit;

            Init();
        }

        protected virtual bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type != HitStimulus.DamageType.Enemy)
            {
                health.Hit(hitStimulus.gameObject);
                StartCoroutine(OnDamageTakenRoutine());
                Bleed(hitStimulus);
                return true;
            }

            return false;
        }

        protected void Bleed(HitStimulus hitStimulus)
        {
            if (dmgEffect != null)
                Destroy(Instantiate(dmgEffect, transform.position, hitStimulus.transform.rotation), 5);
        }

        protected abstract void Init();


        protected virtual void OnDeath(GameObject receiver, GameObject attacker)
        {
            HitStimulus attackerStimulus = attacker.GetComponent<HitStimulus>();

            if (attackerStimulus != null &&
                (attackerStimulus.Type == HitStimulus.DamageType.Darkness ||
                 attackerStimulus.Type == HitStimulus.DamageType.Physical))
            {
                deathEventChannel.Publish(new OnEnemyDeath(this));
                gameController.AddScore(scoreValue);
            }

            Destroy(this.gameObject);
        }

        IEnumerator OnDamageTakenRoutine()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.3f);
            spriteRenderer.color = Color.white;
        }
    }
}