using UnityEditor;
using UnityEngine;
using TMPro;

namespace Jamzio.Editor.Tools
{

    public class UiUtilities : EditorWindow
    {
        [MenuItem("Tools/UI Button Utilities")]
        public static void ShowWindow()
        {
            GetWindow<UiUtilities>("UI Utilities");
        }

        private void OnGUI()
        {
            GUILayout.Label("UI Utilities", EditorStyles.boldLabel);

            if (GUILayout.Button("Set Inspector Name from Labels"))
            {
                SetInspectorNameFromTMPText();
            }

            if (GUILayout.Button("Set Labels from Inspector Name"))
            {
                SetTMPTextFromInspectorName();
            }

            if (GUILayout.Button("Set TMP_Text Names from Button Names"))
            {
                SetTMPTextNamesToButtonNames();
            }
        }

        private static void SetInspectorNameFromTMPText()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected.");
                return;
            }

            foreach (var obj in selectedObjects)
            {
                TMP_Text tmpText = obj.GetComponentInChildren<TMP_Text>();

                if (tmpText != null)
                {
                    string content = tmpText.text;
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        string pascalCaseName = ToPascalCase(content) + "Button";
                        Undo.RecordObject(obj, "Set Inspector Name");
                        obj.name = pascalCaseName;
                    }
                    else
                    {
                        Debug.LogWarning($"TMP_Text in {obj.name} is empty or null.");
                    }
                }
                else
                {
                    Debug.LogWarning($"No TMP_Text component found in {obj.name}.");
                }
            }
        }

        private static void SetTMPTextFromInspectorName()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected.");
                return;
            }

            foreach (var obj in selectedObjects)
            {
                TMP_Text tmpText = obj.GetComponentInChildren<TMP_Text>();

                if (tmpText != null)
                {
                    string inspectorName = obj.name;
                    if (inspectorName.EndsWith("Button"))
                    {
                        inspectorName = inspectorName.Substring(0, inspectorName.Length - 6);
                    }

                    string readableText = AddSpacesToPascalCase(inspectorName);
                    Undo.RecordObject(tmpText, "Set TMP_Text Content");
                    tmpText.text = readableText;
                }
                else
                {
                    Debug.LogWarning($"No TMP_Text component found in {obj.name}.");
                }
            }
        }

        private static void SetTMPTextNamesToButtonNames()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected.");
                return;
            }

            foreach (var obj in selectedObjects)
            {
                TMP_Text tmpText = obj.GetComponentInChildren<TMP_Text>();

                if (tmpText != null)
                {
                    string inspectorName = obj.name;
                    if (inspectorName.EndsWith("Button"))
                    {
                        inspectorName = inspectorName.Substring(0, inspectorName.Length - 6); // Remove "Button"
                    }

                    string labelName = inspectorName + "Label";
                    Undo.RecordObject(tmpText.gameObject, "Set TMP_Text Name");
                    tmpText.gameObject.name = labelName;
                }
                else
                {
                    Debug.LogWarning($"No TMP_Text component found in {obj.name}.");
                }
            }
        }

        private static string ToPascalCase(string input)
        {
            string[] words = input.Split(new char[] { ' ', '_', '-' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return string.Join("", words);
        }

        private static string AddSpacesToPascalCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            System.Text.StringBuilder newText = new();
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && !char.IsWhiteSpace(text[i - 1]))
                {
                    newText.Append(' ');
                }
                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}