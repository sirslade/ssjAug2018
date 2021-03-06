﻿using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.Players.ControllerComponents
{
    public sealed class AimControllerComponent : PlayerControllerComponent
    {
        public class AimAction : CharacterActorControllerAction
        {
            public static AimAction Default = new AimAction();
        }

        [SerializeField]
        private Transform _aimFollowTarget;

        [SerializeField]
        [ReadOnly]
        private bool _isAiming;

        public bool IsAiming => _isAiming;

        private Transform _previousFollowTarget;

        public override bool OnAnimationMove(Vector3 axes, float dt)
        {
            if(!IsAiming) {
                return false;
            }

            Vector3 viewerForward = null != PlayerController.Player.Viewer
                                    ? PlayerController.Player.Viewer.transform.forward
                                    : PlayerController.Player.transform.forward;
            PlayerController.Player.transform.forward = new Vector3(viewerForward.x, 0.0f, viewerForward.z).normalized;

            return true;
        }

        public override bool OnPhysicsMove(Vector3 axes, float dt)
        {
            if(!IsAiming) {
                return false;
            }

            // TODO: this is bullshit, but until we get slopes fixed in CharacterActorController
            // we need this in order to not slide down stuff
            Vector3 velocity = Controller.Rigidbody.velocity;
            velocity.x = velocity.z = 0.0f;
            Controller.Rigidbody.velocity = velocity;

            return true;
        }

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            _isAiming = true;

            PlayerController.Player.PlayerViewer.Aim(_aimFollowTarget);

            return true;
        }

        public override bool OnCancelled(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            PlayerController.Player.PlayerViewer.ResetTarget();

            _isAiming = false;

            return true;
        }
    }
}
