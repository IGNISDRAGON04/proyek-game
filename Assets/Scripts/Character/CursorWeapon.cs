using UnityEngine;

namespace Vampire
{
    public class CursorWeapon : MonoBehaviour
    {
        public Transform firePoint;
        public GameObject bulletPrefab;

        void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Important for 2D

            Vector3 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Flip weapon vertically if aiming left
            if (mousePosition.x < transform.position.x)
                transform.localScale = new Vector3(1, -1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            // Fire bullet
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            }
        }
    }
}
