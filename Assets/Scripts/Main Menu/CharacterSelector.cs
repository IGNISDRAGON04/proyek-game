using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

namespace Vampire
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] protected CharacterBlueprint[] characterBlueprints;
        [SerializeField] protected GameObject characterCardPrefab;
        [SerializeField] protected CoinDisplay coinDisplay;

        [Header("Audio Settings")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip selectSoundEffect;

        private CharacterCard[] characterCards;
        
        public void Init()
        {
            characterCards = new CharacterCard[characterBlueprints.Length];
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                characterCards[i] = Instantiate(characterCardPrefab, this.transform).GetComponent<CharacterCard>();
                characterCards[i].Init(this, characterBlueprints[i], coinDisplay);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                characterCards[i].UpdateLayout();
            }
        }
        
        // --- MODIFIED: Changed from 'void' to 'IEnumerator' and made it public ---
        public IEnumerator StartGameCoroutine(CharacterBlueprint characterBlueprint)
        {
            // Play the character selection sound effect here
            if (audioSource != null && selectSoundEffect != null)
            {
                audioSource.PlayOneShot(selectSoundEffect);
                
                // Wait for the duration of the sound effect
                yield return new WaitForSeconds(selectSoundEffect.length); 
            }
            // If no sound is assigned, or audioSource is null, this will just proceed immediately.

            CrossSceneData.CharacterBlueprint = characterBlueprint;
            SceneManager.LoadScene(1);
        }

        // Keep this public method to easily call it from UI Buttons
        public void StartGame(CharacterBlueprint characterBlueprint)
        {
            // Start the coroutine when the button is clicked
            StartCoroutine(StartGameCoroutine(characterBlueprint));
        }
    }
}