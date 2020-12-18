#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private CharacterController? controller = null;
        [SerializeField] private Camera? lookCamera = null;
        [SerializeField] private Transform? groundCheck = null;

        [Header("Settings")]
        [SerializeField] private float mouseSensitivity = 100f;

        [SerializeField] private float moveSpeed = 12.0f;
        [SerializeField] private float dashSpeed = 20.0f;
        [SerializeField] private float dashTimeout = 1f;
        [SerializeField] private float dashPlayTime = 0.25f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 2f;

        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        private float xRotation = 0f;

        private float verticalVelocity = 0;
        private bool isDashing = false;
        private float dashLastTimer = 0;
        private Vector3 dashDirection = Vector3.zero;
        private bool isGrounded = true;
        private bool doubleJumpAllowed = true;

        // Input values
        private Vector2 lookDirection = Vector2.zero;
        private Vector2 moveDirection = Vector2.zero;
        private bool jumpFlag = false;
        private bool dashFlag = false;
        private bool fireFlag = false;
        private bool meleeFlag = false;


        public void MoveEvent(InputAction.CallbackContext context)
        {
            this.moveDirection = context.ReadValue<Vector2>();
        }

        public void LookEvent(InputAction.CallbackContext context)
        {
            this.lookDirection = context.ReadValue<Vector2>();
        }

        public void JumpEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                this.jumpFlag = true;
            }
        }

        public void FireEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                this.fireFlag = true;
            }
            else if (context.canceled)
            {
                this.fireFlag = false;
            }
        }

        public void MeleeEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                this.meleeFlag = true;
            }
        }

        public void DashEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                this.dashFlag = true;
            }
        }

        // Update is called once per frame
        public void UpdateBehaviour()
        {
            if (GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                //Melee();
                //Fire();
                Look();
                Movement();
            }
        }


        // Update is called once per frame
        private void Movement()
        {
            Vector3 direction = GetMovementDirection();
            //bool jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);

            isGrounded = groundCheck != null && Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                doubleJumpAllowed = true;
            }

            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -1f;
            }

        

            if (isDashing && Time.time > (dashLastTimer + dashPlayTime))
            {
                isDashing = false;
                dashDirection = Vector3.zero;
            }

            if (dashFlag && Time.time >= (dashLastTimer + dashTimeout))
            {
                dashLastTimer = Time.time;
                isDashing = true;
                //AudioManager.Instance.PlayDash();
                dashDirection = direction == Vector3.zero ? transform.forward : direction;
                //GameManager.Instance.ConsumeEnergy(GameManager.Instance.GameSettings != null ? GameManager.Instance.GameSettings.dashEnergy : 0);
            }

            if (jumpFlag && isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (jumpFlag && !isGrounded && doubleJumpAllowed)
            {
                verticalVelocity += Mathf.Sqrt(jumpHeight * gravity * -2);
                doubleJumpAllowed = false;
            }                

            verticalVelocity += gravity * Time.deltaTime;

            Vector3 move = ((direction * moveSpeed) + (Vector3.up * verticalVelocity) + (isDashing ? dashDirection * dashSpeed : Vector3.zero)) * Time.deltaTime;

            controller?.Move(move);
            jumpFlag = false;
            dashFlag = false;
        }

        private Vector3 GetMovementDirection()
        {
            return Vector3.ClampMagnitude(((transform.right * this.moveDirection.x) + (transform.forward * this.moveDirection.y)), 1);
        }

        private void Look()
        {
            Vector2 look = this.lookDirection;


            look *= mouseSensitivity * Time.deltaTime;

           xRotation -= look.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            this.transform.Rotate(Vector3.up * look.x);
            lookCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);


        }
     
    }
}