using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController player;
        public bool PlayerActionAllowed => true;

        public PlayingState(PlayerController player)
        {
            this.player = player;
        }


        public void Begin()
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.Instance.Reset();
            GameManager.Instance.LoadLevel();
        }

        public void End()
        {
            
        }

  
        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {
            
        }

   
        public void Start()
        {
            
        }

        public void Select()
        {

        }

        public void Update()
        {
            this.player.UpdateBehaviour();
        }


    }
}