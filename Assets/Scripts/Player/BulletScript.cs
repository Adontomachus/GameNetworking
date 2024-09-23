using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using GNW2.Player;
//using Mono.Cecil;

namespace GNW2.Projectile
{
    public class BulletScript : NetworkBehaviour
    {
        public MeshRenderer renderer;
        public Color color { get; set; }
        public float bulletSpeed = 10f;
        [SerializeField] private float redValue = 1f;
        public GameObject target;
        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private int damage = 45;
        [Networked] private TickTimer life { get; set; }

        public override void Spawned()
        {
            Init();
        }
        public void Init()
        {
            life = TickTimer.CreateFromSeconds(Runner, lifeTime);
        }
        // Start is called before the first frame update

        public override void FixedUpdateNetwork()
        {
            
            redValue += 1f * Runner.DeltaTime;
            color = new Color(redValue, 1.5f - redValue/2, 1.5f - redValue / 2, 1f);
            if (life.Expired(Runner))
            {
                Runner.Despawn(Object);
            }
            else if (target == null)
            {
        
                transform.position += bulletSpeed * transform.forward * Runner.DeltaTime;
            }
            else if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, bulletSpeed * Runner.DeltaTime);
            }
        renderer.material.color = color;
        }
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log($"Hit Collider {other.collider.name}");
            if (Object.HasStateAuthority)
            {



                var combatInterface = other.collider.GetComponent<ICombat>();
                if (combatInterface != null)
                {
                    combatInterface.TakeDamage(damage);
                }
                else
                {
                    Debug.LogError("Combat Interface Found");
                }

                Runner.Despawn(Object);
            }
        }

    }
}
