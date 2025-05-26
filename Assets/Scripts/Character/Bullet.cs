using UnityEngine;

namespace Vampire
{
    public class Bullet : MonoBehaviour
    {
        //[SerializedField] private float BulletDamage = 2f;
        public float bulletSpeed = 10f;
        public float lifetime = 2f;
        //private float damage;
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime); 
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //DamageBullet DamageBullet = collision.gameObject.GetComponent<DamageBullet>();
            //if (DamageBullet != null)
            //{
            //    DamageBullet.Damage(damage)
            //}
            Destroy(gameObject);
        }
    }
}
