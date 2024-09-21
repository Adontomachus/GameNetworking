using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
