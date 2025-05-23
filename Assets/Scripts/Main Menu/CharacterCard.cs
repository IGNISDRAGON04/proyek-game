using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

namespace Vampire
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private RectTransform characterImageRect;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI mvspdText;
        [SerializeField] private TextMeshProUGUI luckText;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private LocalizedString buyLocalization, selectLocalization;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Color selectColor, buyColor;
        [SerializeField] private RectTransform startingAbilitiesParent;
        [SerializeField] private GameObject startingAbilityContainerPrefab;
        [SerializeField] private Vector2 startingAbilitiesRectSize = new Vector2(365, 85);
        private CharacterSelector characterSelector;

        // Konfigurasi blueprint karakter
        private CharacterBlueprint characterBlueprint;
        private CoinDisplay coinDisplay;
        private StartingAbilityContainer[] startingAbilityContainers;
        private bool initialized;

        private void OnEnable()
        {
            buyLocalization.StringChanged += UpdateButtonText;
        }

        private void OnDisable()
        {
            buyLocalization.StringChanged -= UpdateButtonText;
        }

        // Inisiasi Card dari karakter sebelum dipilih
        // UInya butuh display koin, dan selector karakter
        // Karakter yang dipilih adalah karakter yang ada di dalam blueprint
        public void Init(CharacterSelector characterSelector, CharacterBlueprint characterBlueprint, CoinDisplay coinDisplay)
        {
            this.characterSelector = characterSelector;
            this.characterBlueprint = characterBlueprint;
            this.coinDisplay = coinDisplay;

            characterImage.sprite = characterBlueprint.walkSpriteSequence[0];

            nameText.text = characterBlueprint.name.ToString();
            hpText.text = characterBlueprint.hp.ToString();
            armorText.text = characterBlueprint.armor.ToString();
            mvspdText.text = Mathf.RoundToInt(characterBlueprint.movespeed / 1.15f * 100f).ToString() + "%";
            luckText.text = characterBlueprint.luck.ToString();
            UpdateButtonText();
            buttonImage.color = characterBlueprint.owned ? selectColor : buyColor;

            // Instantiate the images
            startingAbilityContainers = new StartingAbilityContainer[characterBlueprint.startingAbilities.Length];
            for (int i = 0; i < characterBlueprint.startingAbilities.Length; i++)
            {
                startingAbilityContainers[i] = Instantiate(startingAbilityContainerPrefab, startingAbilitiesParent).GetComponent<StartingAbilityContainer>();
                startingAbilityContainers[i].AbilityImage.sprite = characterBlueprint.startingAbilities[i].GetComponent<Ability>().Image;
            }

            initialized = true;
        }

        public void UpdateLayout()
        {
            // Character image layout
            float yHeight = Mathf.Abs(characterImageRect.sizeDelta.y);
            float xWidth = characterBlueprint.walkSpriteSequence[0].textureRect.width / (float) characterBlueprint.walkSpriteSequence[0].textureRect.height * yHeight;
            if (xWidth > Mathf.Abs(characterImageRect.sizeDelta.x))
            {
                xWidth = Mathf.Abs(characterImageRect.sizeDelta.x);
                yHeight = characterBlueprint.walkSpriteSequence[0].textureRect.height / (float) characterBlueprint.walkSpriteSequence[0].textureRect.width * xWidth;
            }
            ((RectTransform)characterImage.transform).sizeDelta = new Vector2(xWidth, yHeight);
            
            // Character abilities layout
            float maxImageWidth = startingAbilitiesRectSize.x / startingAbilityContainers.Length;
            for (int i = 0; i < startingAbilityContainers.Length; i++)
            {
                StartingAbilityContainer startingAbilityContainer = startingAbilityContainers[i];
                float imageHeight = startingAbilitiesRectSize.y;
                float imageWidth = startingAbilityContainer.AbilityImage.sprite.textureRect.width / (float) startingAbilityContainer.AbilityImage.sprite.textureRect.height * imageHeight;
                if (imageWidth > maxImageWidth)
                {
                    imageWidth = maxImageWidth;
                    imageHeight = startingAbilityContainer.AbilityImage.sprite.textureRect.height / (float) startingAbilityContainer.AbilityImage.sprite.textureRect.width * imageWidth;
                }
                startingAbilityContainer.ImageRect.sizeDelta = new Vector2(imageWidth, imageHeight);
            }
        }

        public void Selected()
        {
            // 1. jika karakter sudah dimiliki
            // 2. jika ada kelas yang bernama characterblueprint
            if (!characterBlueprint.owned)
            {
                // 1. memulai karakter dengan koin, 
                int coinCount = PlayerPrefs.GetInt("Coins");
                // 2. jika koin lebih dari atau sama dengan biaya karakter
                if (coinCount >= characterBlueprint.cost)
                {
                    // 2.1. Karakter memulai dengan koin, dan dikurangi dengan biaya karakter
                    PlayerPrefs.SetInt("Coins", coinCount - characterBlueprint.cost);

                    // 2.2. Karakter dimulai dengan atribut telah dimiliki
                    characterBlueprint.owned = true;

                    // update teks tombolnya, dengan localized string berdasarkan kelas yang dibawah
                    UpdateButtonText();

                    // 2.3. tombolnya diubah warna mengikuti selectColor
                    buttonImage.color = selectColor;

                    // 2.4. display koin diupdate valuenya
                    coinDisplay.UpdateDisplay();
                }
            }
            else
            {
                // jika tidak membeli karakter baru, maka langsung start game.
                characterSelector.StartGame(characterBlueprint);
            }
        }

        private void UpdateButtonText(string text)
        { 
            UpdateButtonText();
        }
        
        private void UpdateButtonText()
        {
            if (!initialized) return;
            
            if (characterBlueprint.owned)
            {
                buttonText.text = selectLocalization.GetLocalizedString();
            }
            else
            {
                buttonText.text = String.Format("{0} (${1})", buyLocalization.GetLocalizedString(), characterBlueprint.cost);
            }
        }
    }
}
