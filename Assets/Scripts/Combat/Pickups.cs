using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickups : MonoBehaviour,IRaycastable
{
   [SerializeField] WeaponConfig weapon;
   [SerializeField] float RespawnTime=5f;



        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(RespawnPickup());
        }

        IEnumerator RespawnPickup()
    {
        ShowPickup(false);
        yield return new WaitForSeconds(RespawnTime);
        ShowPickup(true);
    }

        private void ShowPickup(bool isShow)
        {
            GetComponent<CapsuleCollider>().enabled=isShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(isShow);
            }
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Pickup(playerController.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}


