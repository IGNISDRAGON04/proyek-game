using UnityEngine;

namespace Vampire
{
    public class ShowDialogueTriggerEnter : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogueBox;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.StartDialogue();
            gameObject.SetActive(false);
        }
    }
    }
}
