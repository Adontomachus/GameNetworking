using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using GNW2.Input;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon.StructWrapping;

namespace GNW2.GameManager
{
    public class GameManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [NonSerialized] public NetworkRunner networkRunnerPrefab;
        private static GameManager _instance;
        [NonSerialized] public NetworkRunner _runner;
        public static bool changeColors = false;
        private bool _isMouseButton0Pressed;

        public static NetworkRunner Runner => _instance._runner;
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();

        #region NetworkRunner Callbacks

        void Start()
        {
            
        }
        private void Update()
        {
            _isMouseButton0Pressed = _isMouseButton0Pressed || UnityEngine.Input.GetMouseButton(0);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                changeColors = true;
                Vector3 customLocation = new Vector3(1 * runner.SessionInfo.PlayerCount, 0, 0);
                NetworkObject playerNetworkObject = runner.Spawn(_playerPrefab, customLocation, Quaternion.identity, player);
                playerNetworkObject.AssignInputAuthority(player);

                _spawnedPlayers.Add(player, playerNetworkObject);
                Debug.Log("Change Color:" + changeColors);

            }
           // else if (player == runner.LocalPlayer)
           // {
            //    Vector3 customLocation2 = new Vector3(1 * runner.SessionInfo.PlayerCount, 0, 0);
           //     runner.Spawn(_playerPrefab, customLocation2, Quaternion.identity, player);
           // }
        }
        
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedPlayers.TryGetValue(player, out NetworkObject playerNetworkObject))
            {
                runner.Despawn(playerNetworkObject);
                _spawnedPlayers.Remove(player);
                Debug.Log("Change Color:" + changeColors);
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();
            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                data.Direction += Vector3.forward;
            }
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                data.Direction += Vector3.left;
            }
            if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                data.Direction += Vector3.back;
            }
            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                data.Direction += Vector3.right;
            }
            if ( UnityEngine.Input.GetKey(KeyCode.Space))
            {
                Debug.Log(data.canJump);
                //data.Direction += Vector3.up;
                data.canJump = true;
            }
            if (_isMouseButton0Pressed)
                data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _isMouseButton0Pressed);
            _isMouseButton0Pressed = false;

            input.Set(data);
 

        }
        
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){ }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){ }
        public void OnConnectedToServer(NetworkRunner runner){ }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason){ }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){ }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){ }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){ }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){ }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }

        public void OnSceneLoadDone(NetworkRunner runner){ }

        public void OnSceneLoadStart(NetworkRunner runner){ }
        #endregion

        public async void StartGame(GameMode mode)
        {
            // lets fusion know that we will be sending input
            _runner = this.gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true; 

            //create the scene info to send to fusion
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            await _runner.StartGame(new StartGameArgs
                {
                    GameMode = mode,
                    SessionName = "TestRoom",
                    Scene = scene,
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                }
            );

        }

       /* private void OnGUI()
        {
            if (_runner == null)
            {
                if (GUI.Button(new Rect(0, 0, 200, 40), "Host Game"))
                {
                    StartGame(GameMode.Host);
                }
                if (GUI.Button(new Rect(0, 40, 200, 40), "Join Game"))
                {
                    StartGame(GameMode.Client);
                }
            }
        }
       */
        internal void SetActive(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
