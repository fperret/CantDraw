using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CanEditMultipleObjects] // Don't ruin everyone's day
[CustomEditor(typeof(MonoBehaviour), true)] // Target all MonoBehaviours and descendants
public class MonoBehaviourCustomEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the normal inspector
        
        // Only work in the Play mode.
        if (Application.isPlaying)
        {
            // Get the type descriptor for the MonoBehaviour we are drawing
            var type = target.GetType();
            
            // Iterate over each private or public instance method
            foreach (var method in type.GetMethods(BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance))
            {
                // make sure it is decorated by our custom attribute
                var attributes = method.GetCustomAttributes(typeof(ExposeMethodInEditorAttribute), true);
                if (attributes.Length > 0)
                {
                    if (GUILayout.Button("Run: " + method.Name))
                    {
                        // If the user clicks the button, invoke the method immediately.
                        // Invoke only works in Play Mode.
                        ((MonoBehaviour)target).Invoke(method.Name, 0f);
                    }
                }
            }
        }
    }
}
