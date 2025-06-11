using UnityEngine;
using TMPro;
using UnityEngine.Localization; // Make sure this is present if using Localization

namespace Vampire
{
    public class GameOverDialog : DialogBox
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI coinsGained;
        [SerializeField] private TextMeshProUGUI enemiesRouted;
        [SerializeField] private TextMeshProUGUI damageDealt;
        [SerializeField] private TextMeshProUGUI damageTaken;
        [SerializeField] private GameObject background;

        [Header("Localization")]
        [SerializeField] private LocalizedString levelPassedLocalization, levelLostLocalization;

        [Header("Audio")]
        [SerializeField] private AudioSource gameOverAudioSource; // The AudioSource component
        [SerializeField] private AudioClip levelPassedClip;      // Sound for level passed
        [SerializeField] private AudioClip levelLostClip;        // Sound for level lost

        public void Open(bool levelPassed, StatsManager statsManager)
        {
            statusText.text = levelPassed ? levelPassedLocalization.GetLocalizedString() : levelLostLocalization.GetLocalizedString();
            coinsGained.text = "+" + statsManager.CoinsGained;
            enemiesRouted.text = statsManager.MonstersKilled.ToString();
            damageDealt.text = statsManager.DamageDealt.ToString();
            damageTaken.text = statsManager.DamageTaken.ToString();
            background.SetActive(true);

            // Play appropriate sound
            if (gameOverAudioSource != null)
            {
                if (levelPassed && levelPassedClip != null)
                {
                    gameOverAudioSource.PlayOneShot(levelPassedClip);
                }
                else if (!levelPassed && levelLostClip != null)
                {
                    gameOverAudioSource.PlayOneShot(levelLostClip);
                }
            }
            else
            {
                Debug.LogWarning("GameOver Audio Source is not assigned. No audio will be played.");
            }

            base.Open();
        }

        public override void Close()
        {
            base.Close();
            background.SetActive(false);
            // Optionally stop the audio if it's still playing when the dialog closes
            if (gameOverAudioSource != null && gameOverAudioSource.isPlaying)
            {
                gameOverAudioSource.Stop();
            }
        }
    }
}