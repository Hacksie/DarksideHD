#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HackedDesign
{
    public class GameManager : MonoBehaviour
    {
        public const string gameVersion = "1.0";

        [Header("Game")]
        [SerializeField] private Camera? mainCamera = null;
        [SerializeField] private PlayerController? playerController = null;
        [SerializeField] private PlayerPreferences? preferences = null;
        [SerializeField] private LevelGenerator? levelGenerator = null;
        [SerializeField] private UnityEngine.Audio.AudioMixer masterMixer = null;
        [SerializeField] private int gameLength = 64;

        private IState currentState = new EmptyState();

#pragma warning disable CS8618
        public static GameManager Instance { get; private set; }
#pragma warning restore CS8618

        public Camera? MainCamera { get { return mainCamera; } private set { mainCamera = value; } }
        public PlayerController? Player { get { return playerController; } private set { playerController = value; } }
        public PlayerPreferences? PlayerPreferences { get { return preferences; } private set { preferences = value; } }

        public IState CurrentState
        {
            get
            {
                return this.currentState;
            }
            private set
            {
                this.currentState.End();

                this.currentState = value;

                this.currentState.Begin();
            }
        }

        private GameManager() => Instance = this;

        void Awake() => CheckBindings();
        void Start() => Initialization();

        void Update() => CurrentState?.Update();
        void LateUpdate() => CurrentState?.LateUpdate();
        void FixedUpdate() => CurrentState?.FixedUpdate();

        public void SetPlaying() => CurrentState = new PlayingState(this.playerController);

        public void LoadLevel()
        {
            //entityPool?.DestroyEntities();
            levelGenerator?.Generate(gameLength);
        }

        public void Reset()
        {
        }        

        private void CheckBindings()
        {
            Player = this.playerController ?? FindObjectOfType<PlayerController>();
            MainCamera = this.mainCamera ?? Camera.main;
        }

        private void Initialization()
        {
            preferences = new PlayerPreferences(this.masterMixer);
            preferences.Load();
            preferences.SetPreferences();

            HideAllUI();
            SetPlaying();
        }

        private void HideAllUI()
        {

        }



    }
}