﻿using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.ssjAug2018.GameState;

namespace pdxpartyparrot.ssjAug2018.Menu
{
    public sealed class MultiplayerMenu : MenuPanel
    {
        public NetworkConnect ConnectGameState { private get; set; }

        public GameState.Game GameState { private get; set; }

#region Event Handlers
        public void OnHost()
        {
            GameStateManager.Instance.PushSubState(ConnectGameState, state => {
                state.Initialize(NetworkConnect.ConnectType.Server, GameState);
            });
        }

        public void OnJoin()
        {
            GameStateManager.Instance.PushSubState(ConnectGameState, state => {
                state.Initialize(NetworkConnect.ConnectType.Client, GameState);
            });
        }

        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
