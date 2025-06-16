using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField][TextArea(3, 10)] private string[] lines;
    [SerializeField] private float textSpeed = 0.04f;
    
    [SerializeField] private Button backButton; // Reference to the back button
    [SerializeField] private CanvasGroup buttonCanvasGroup; // Reference to the CanvasGroup for fading

    private int index;

    void Start()
    {   
        backButton.onClick.AddListener(LoadMainMenu);
        backButton.gameObject.SetActive(false); // Hide the button initially
        buttonCanvasGroup.alpha = 0; // Ensure the button is fully transparent
    }

    void Open()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // If it's the last line, show the button
        // if (index >= lines.Length - 1)
        // {
        //     StartCoroutine(FadeInButton());
        // }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = string.Empty;
        }
    }

    IEnumerator FadeInButton()
    {
        backButton.gameObject.SetActive(true); // Show the button
        float duration = 1f; // Duration for the fade
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            buttonCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // Replace with your main menu scene name
    }
}