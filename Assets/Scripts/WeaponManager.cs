using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] List<Weapon> weaponslots;
        [SerializeField] Weapon melee;
        [SerializeField] SpriteRenderer crosshair;

        public int currentWeaponSlot = 0;

        void Start()
        {
            ShowCurrentWeapon();
        }

        public void WeaponScrollEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var direction = context.ReadValue<float>();
                if (direction <= 0)
                {
                    PrevWeapon();

                }
                else
                {
                    NextWeapon();
                }
            }
        }

        public void NextWeaponEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                NextWeapon();
            }
        }

        public void PrevWeaponEvent(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PrevWeapon();
            }
        }

        public void Weapon1Event(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SelectWeapon(0);
            }
        }

        public void Weapon2Event(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SelectWeapon(1);
            }
        }

        public void Weapon3Event(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                SelectWeapon(2);
            }
        }

        public void ShowCurrentWeapon()
        {
            HideAll();
            weaponslots[currentWeaponSlot].gameObject.SetActive(true);
        }

        private void SelectWeapon(int weapon)
        {
                weaponslots[currentWeaponSlot].gameObject.SetActive(false);
                currentWeaponSlot = weapon;
                weaponslots[currentWeaponSlot].gameObject.SetActive(true);
            
        }

        private void NextWeapon()
        {
            weaponslots[currentWeaponSlot].gameObject.SetActive(false);
            currentWeaponSlot++;

            if (currentWeaponSlot >= weaponslots.Count)
            {
                currentWeaponSlot = 0;
            }
            weaponslots[currentWeaponSlot].gameObject.SetActive(true);
        }

        private void PrevWeapon()
        {
            weaponslots[currentWeaponSlot].gameObject.SetActive(false);
            currentWeaponSlot--;
            if (currentWeaponSlot < 0)
            {
                currentWeaponSlot = weaponslots.Count;
            }
            weaponslots[currentWeaponSlot].gameObject.SetActive(true);
        }

        private void HideAll()
        {
            for (int i = 0; i < weaponslots.Count; i++)
            {
                weaponslots[i].gameObject.SetActive(false);
            }
        }
    }
}