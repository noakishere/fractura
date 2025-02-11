using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingEditorWindow : EditorWindow
{
    [MenuItem("Tools/Crafting/Crafting Graph")]
    public static void Open()
    {
        GetWindow<CraftingEditorWindow>("Crafting Graph");
    }

    private void OnEnable()
    {
        AddGraphWindow();
    }

    private void AddGraphWindow()
    {
        CraftingGraphView graphView = new CraftingGraphView();
        
        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }
}
