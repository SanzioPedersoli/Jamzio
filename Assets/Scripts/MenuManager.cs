using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Jamzio.Runtime.UI
{

    public class MenuManager : MonoBehaviour
    {
        [Serializable]
        private struct SceneLoadButton
        {
            public string SceneName;
            public Button Button;
            public float TransitionTime;
        }

        [SerializeField] private List<SceneLoadButton> _sceneLoadButtons;
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            foreach (var sceneLoadButton in _sceneLoadButtons)
                sceneLoadButton.Button.onClick.AddListener(async () =>
                {
                    SetButtonsActive(false);
                    await Timer(sceneLoadButton.TransitionTime, () => LoadScene(sceneLoadButton.SceneName));
                    SetButtonsActive(true);
                });

            if (_closeButton) _closeButton.onClick.AddListener(() =>
            {
                SetButtonsActive(false);
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            });
        }

        public void SetButtonsActive(bool areActive)
        {
            foreach (SceneLoadButton sceneLoadButton in _sceneLoadButtons) sceneLoadButton.Button.interactable = areActive;
        }

        private async UniTask Timer(float transitionTime, Action action)
        {
            await UniTask.WaitForSeconds(transitionTime);
            action.Invoke();
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}