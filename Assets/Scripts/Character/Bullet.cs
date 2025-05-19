using UnityEngine;

namespace Vampire
{
    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed = 10f;
        public float lifetime = 2f;

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
            Destroy(gameObject);
        }
    }
}
