﻿using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ssjAug2018.GameState
{
    public sealed class Game : pdxpartyparrot.Game.State.GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            InitializeManagers();

            NetworkManager.Instance.ServerChangedScene();
            NetworkManager.Instance.LocalClientReady(GameManager.Instance.NetworkClient?.connection, 0);
        }

        public override void OnExit()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.Disable();
            }

            if(NetworkManager.HasInstance) {
                NetworkManager.Instance.Stop();
            }

            if(GameManager.HasInstance) {
                GameManager.Instance.NetworkClient = null;
            }

            base.OnExit();
        }

        private void InitializeManagers()
        {
            ViewerManager.Instance.FreeViewers();
            ViewerManager.Instance.AllocateViewers(1);

            InputManager.Instance.Controls.game.Enable();
        }
    }
}
