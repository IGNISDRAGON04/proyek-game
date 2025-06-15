using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class MachineGunAbility : GunAbility
    {
        [Header("Machine Gun Stats")]
        [SerializeField] protected GameObject machineGun;
        [SerializeField] protected Transform launchTransform;
        [SerializeField] protected UpgradeableRotationSpeed rotationSpeed;
        [SerializeField] protected float gunRadius;
        protected Vector3 gunDirection = Vector2.right;

        protected override void Update()
        {
            base.Update();

            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Set z to 0 since we're working in 2D

            // Calculate the direction from the machine gun to the mouse position
            gunDirection = (mousePosition - playerCharacter.CenterTransform.position).normalized;

            // Update the gun's position and rotation
            machineGun.transform.position = playerCharacter.CenterTransform.position + gunDirection * gunRadius;

            // Calculate the angle for rotation
            float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;
            machineGun.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Check for mouse click to launch projectile
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                LaunchProjectile();
            }
        }

        protected override void LaunchProjectile()
        {
            base.LaunchProjectile();
            Projectile projectile = entityManager.SpawnProjectile(projectileIndex, launchTransform.position, damage.Value, knockback.Value, speed.Value, monsterLayer);
            projectile.OnHitDamageable.AddListener(playerCharacter.OnDealDamage.Invoke);
            projectile.Launch(gunDirection);
        }
    }
}