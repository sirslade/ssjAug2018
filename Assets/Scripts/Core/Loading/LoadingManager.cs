﻿using System;
using System.Collections;

using DG.Tweening;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Scenes;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;

using UnityEngine;

namespace pdxpartyparrot.Core.Loading
{
    public abstract class LoadingManager<T> : SingletonBehavior<T> where T: LoadingManager<T>
    {
        [SerializeField]
        private LoadingScreen _loadingScreen;

        protected LoadingScreen LoadingScreen => _loadingScreen;

#region Manager Prefabs
        [SerializeField]
        private AudioManager _audioManagerPrefab;

        [SerializeField]
        private CameraManager _cameraManagerPrefab;

        [SerializeField]
        private InputManager _inputManagerPrefab;

        [SerializeField]
        private NetworkManager _networkManagerPrefab;

        [SerializeField]
        private SceneManager _sceneManagerPrefab;
#endregion

        protected GameObject ManagersContainer { get; private set; }

        // TODO: this should come from the game state
        [SerializeField]
        private string _defaultSceneName;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            ManagersContainer = new GameObject("Managers");
        }

        protected virtual void Start()
        {
            StartCoroutine(Load());
        }
#endregion

        private IEnumerator Load()
        {
            _loadingScreen.Progress.Percent = 0.0f;
            _loadingScreen.ProgressText = "Creating managers...";
            yield return null;

            CreateManagers();
            yield return null;

            _loadingScreen.Progress.Percent = 0.25f;
            _loadingScreen.ProgressText = "Initializing managers...";
            yield return null;

            InitializeManagers();
            yield return null;

            _loadingScreen.Progress.Percent = 0.75f;
            _loadingScreen.ProgressText = "Loading default scene...";
            SceneManager.Instance.LoadDefaultScene(() => {
                _loadingScreen.Progress.Percent = 1.0f;
                _loadingScreen.ProgressText = "Loading complete!";

// TODO:
                //GameStateManager.Instance.TransitionToInitialState();

                Destroy();
            });
        }

        protected virtual void CreateManagers()
        {
            // third party stuff
            DOTween.Init();

            // should always be the first of our stuff
            DebugMenuManager.Create(ManagersContainer);

            TimeManager.Create(ManagersContainer);
            AudioManager.CreateFromPrefab(_audioManagerPrefab, ManagersContainer);
            ObjectPoolManager.Create(ManagersContainer);
            CameraManager.CreateFromPrefab(_cameraManagerPrefab, ManagersContainer);
            InputManager.CreateFromPrefab(_inputManagerPrefab, ManagersContainer);
            Instantiate(_networkManagerPrefab, ManagersContainer.transform);
            SceneManager.CreateFromPrefab(_sceneManagerPrefab, ManagersContainer);
        }

        protected virtual void InitializeManagers()
        {
// TODO:
            SceneManager.Instance.DefaultSceneName = _defaultSceneName;
        }

        private void Destroy()
        {
            Destroy(_loadingScreen.gameObject);
            _loadingScreen = null;

            Destroy(gameObject);
        }
    }
}
