﻿using JetBrains.Annotations;

using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace pdxparyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Platform : NetworkBehaviour, IGrabbable
    {
        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        private PlatformWaypoint[] _waypoints;

        [CanBeNull]
        private PlatformWaypoint _targetWayponit;

        private int _waypointIterator;

        public Collider Collider { get; private set; }

#region Unity Lifecycle
        private void Awake()
        {
            Collider = GetComponent<Collider>();

            TargetWaypoint(0);
        }

        private void FixedUpdate()
        {
            if(null == _targetWayponit) {
                return;
            }

            if((_targetWayponit.transform.position - transform.position).sqrMagnitude < float.Epsilon) {
                _waypointIterator = (_waypointIterator + 1) % _waypoints.Length;

                TargetWaypoint(_waypointIterator);
                if(null == _targetWayponit) {
                    return;
                }
            }

            float step = _speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetWayponit.transform.position, step);
        }
#endregion

        private void TargetWaypoint(int index)
        {
            if(index >= _waypoints.Length || index < 0) {
                _targetWayponit = null;
                return;
            }

            _targetWayponit = _waypoints[index];
            if(null == _targetWayponit) {
                return;
            }

            transform.LookAt(_targetWayponit.transform);
        }
    }
}
