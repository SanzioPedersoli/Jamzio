using UnityEditor;
using UnityEngine;

public class AlignmentTool : EditorWindow
{
    [MenuItem("Tools/Alignment Tool")]
    public static void ShowWindow()
    {
        GetWindow<AlignmentTool>("Alignment Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Alignment Options", EditorStyles.boldLabel);

        if (GUILayout.Button("Align Vertically"))
        {
            AlignObjects(Axis.Vertical);
        }

        if (GUILayout.Button("Align Horizontally"))
        {
            AlignObjects(Axis.Horizontal);
        }

        if (GUILayout.Button("Distribute Regularly"))
        {
            DistributeObjects();
        }
    }

    private enum Axis
    {
        Vertical,
        Horizontal
    }

    private static void AlignObjects(Axis axis)
    {
        var selectedObjects = Selection.transforms;

        if (selectedObjects.Length < 2)
        {
            Debug.LogWarning("Select at least two objects to align.");
            return;
        }

        float center = 0f;
        foreach (var obj in selectedObjects)
        {
            center += axis == Axis.Vertical ? obj.position.y : obj.position.x;
        }
        center /= selectedObjects.Length;

        foreach (var obj in selectedObjects)
        {
            Undo.RecordObject(obj, "Align Objects");
            obj.position = axis == Axis.Vertical
                ? new Vector3(obj.position.x, center, obj.position.z)
                : new Vector3(center, obj.position.y, obj.position.z);
        }
    }

    private static void DistributeObjects()
    {
        var selectedObjects = Selection.transforms;

        if (selectedObjects.Length < 3)
        {
            Debug.LogWarning("Select at least three objects to distribute.");
            return;
        }

        var startPosition = selectedObjects[0].transform.position;
        var targetPosition = selectedObjects[^1].transform.position;
        var floatlenght = selectedObjects.Length - 1f;

        for (int i = 1; i < selectedObjects.Length - 1; i++)
        {
            Undo.RecordObject(selectedObjects[i], "Distribute Objects");
            selectedObjects[i].position = Vector3.Lerp(startPosition, targetPosition, i / floatlenght);
        }
    }
}
