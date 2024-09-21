using System;
using System.Globalization;
using Fusion;
using GNW2.Input;
using GNW2.Projectile;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace GNW2.Player
{
    public class Player : NetworkBehaviour
    {
        public GameObject playerHUD;
        [SerializeField] InformationText informationText;
        [SerializeField] private GameObject nearestTarget;
        public int randomValue;
        public string playerName { get; set; }
        public Camera camera;

        public static Player Local { get; set; }
        private NetworkCharacterController _cc;
        private Vector3 _bulletSpawnLocation = Vector3.forward * 2;
        [SerializeField] private float speed = 8f;
        [SerializeField] private float projSpeed = 8f;
        [SerializeField] public float fireRate = 0.1f;
        [Networked] public TickTimer fireDelayTimer {  get; set; }
        public float delay;
        [SerializeField] private BulletScript bulletPrefab;
        //start

        //private bool canFire = true;
        //private event Action OnButtonPressed;

        void Update()
        {
            FindOtherPlayers();
            if (fireDelayTimer.ExpiredOrNotRunning(Runner))
            {
                informationText.readyToFire = true;
            }
            else
            {
                informationText.readyToFire = false;
            }
        }

        private void FindOtherPlayers()
        {
            GameObject[] targets;

            targets = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject target in targets)
                {

                    if (target != this.gameObject)
                    {
                        nearestTarget = target;
                    }
                    
                }
            
        }
        //end
        private void Awake()
        {

            informationText.readyToFire = true;
            _cc = GetComponent<NetworkCharacterController>();
            _cc.maxSpeed = speed;
        }

        public void RPC_Configure(string name, Color color)
        {
            playerName = name;
        }

        public override void Spawned()
        {
            randomValue = UnityEngine.Random.Range(0, 10000);

            if (HasInputAuthority)
            {
                playerHUD.gameObject.SetActive(true);
                Local = this;
                Camera.main.gameObject.SetActive(false);
            }
            else
            {
                Camera localCamera = GetComponentInChildren<Camera>();
                localCamera.enabled = false;
                playerHUD.gameObject.SetActive(false);
            }
        }


        public override void FixedUpdateNetwork()
        {

            if (!GetInput(out NetworkInputData data)) return;
            
            data.Direction.Normalize();
            _cc.Move(speed * data.Direction * Runner.DeltaTime);

            if (data.canJump)
            {
                _cc.Jump();
            }
            if (!HasStateAuthority || !fireDelayTimer.ExpiredOrNotRunning(Runner)) return;
            


            if (data.Direction.sqrMagnitude > 0 )
            {
                _bulletSpawnLocation = data.Direction * 2f;
            }

            if (!data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0)) return;
            
                fireDelayTimer = TickTimer.CreateFromSeconds(Runner, fireRate);
                Runner.Spawn(bulletPrefab, transform.position + _bulletSpawnLocation, Quaternion.LookRotation(_bulletSpawnLocation), Object.InputAuthority, OnBulletSpawned);
            
        }

        private void OnBulletSpawned(NetworkRunner runner, NetworkObject ob)
        {
            //float range = Mathf.Infinity;
            informationText.readyToFire = false;
            BulletScript bulletScript = ob.GetComponent<BulletScript>();

            bulletScript.bulletSpeed = projSpeed;
            if (nearestTarget != null)
            {
                bulletScript.target = nearestTarget;
            }
            bulletScript.Init();
        }

    }
}
