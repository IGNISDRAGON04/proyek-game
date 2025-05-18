using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class EscapeToQuit : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
            #if UNITY_EDITOR
                // If we're in the Unity Editor, stop playing the scene
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                // If we're in a built game, quit the application
                Application.Quit();
            #endif
            }
        }
    }
}
