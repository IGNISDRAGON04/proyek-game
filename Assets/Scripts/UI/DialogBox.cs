using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class DialogBox : MonoBehaviour
    {
        [Header("Dialog Box Settings")] // Added a header for better organization
        [SerializeField] private bool appearInstantly = false;
        [SerializeField] private float animationSpeed;
        [SerializeField] private DialogBox previousDialog, nextDialog;

        // --- NEW: Audio Fields ---
        [Header("Audio Settings")]
        [SerializeField] private AudioSource audioSource; // Assign an AudioSource component in the Inspector
        [SerializeField] private AudioClip openSoundEffect; // Assign the sound clip for opening
        [SerializeField] private AudioClip closeSoundEffect; // Optional: Sound for closing
        [SerializeField] private AudioClip navigateSoundEffect; // Optional: Sound for Return/Continue

        public virtual void Open()
        {
            gameObject.SetActive(true);

            // --- Play the open sound effect here ---
            if (audioSource != null && openSoundEffect != null)
            {
                audioSource.PlayOneShot(openSoundEffect);
            }

            if (appearInstantly)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(OpenAnimation());
            }
        }

        public virtual void Close()
        {
            // Optional: Play a close sound effect before closing
            if (audioSource != null && closeSoundEffect != null)
            {
                audioSource.PlayOneShot(closeSoundEffect);
            }

            // Consider waiting for the sound to finish if it's crucial,
            // but for simple UI sounds, playing and immediately setting inactive is common.
            // If you want to wait, you'd need a Coroutine for closing as well.

            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            // StopAllCoroutines(); // Keep commented out unless you add a CloseAnimation() coroutine
            // StartCoroutine(CloseAnimation()); // Keep commented out unless you add a CloseAnimation() coroutine
        }

        public void Return()
        {
            // --- Play a navigation sound effect here ---
            if (audioSource != null && navigateSoundEffect != null)
            {
                audioSource.PlayOneShot(navigateSoundEffect);
            }

            // It's generally better to let the previous/next dialog handle its own 'Open' sound.
            // This 'navigateSoundEffect' would be for the *action* of clicking return/continue.
            previousDialog?.Open();
            Close();
        }

        public void Continue()
        {
            // --- Play a navigation sound effect here ---
            if (audioSource != null && navigateSoundEffect != null)
            {
                audioSource.PlayOneShot(navigateSoundEffect);
            }

            nextDialog?.Open();
            Close();
        }

        private IEnumerator OpenAnimation()
        {
            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, EasingUtils.EaseOutBack(t));
                t += Time.unscaledDeltaTime * animationSpeed;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }

        // Uncomment and implement this if you want a closing animation with sound
        /*
        private IEnumerator CloseAnimation()
        {
            // This would play the sound and then animate the close
            if (audioSource != null && closeSoundEffect != null)
            {
                audioSource.PlayOneShot(closeSoundEffect);
            }

            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, EasingUtils.EaseOutQuart(t));
                t += Time.deltaTime * animationSpeed;
                yield return null;
            }
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
        */
    }
}