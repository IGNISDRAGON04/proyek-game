using UnityEngine;

namespace Vampire
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public float lifetime = 2f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime); 
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Handle collision with enemies or other objects
            // Example: Destroy bullet on hit
            Destroy(gameObject);
        }
    }
}
