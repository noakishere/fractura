using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingGraphView : GraphView
{
    public CraftingGraphView()
    {
        AddGridBackground();
        AddStyles();
    }

    private void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();

        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }

    private void AddStyles()
    {
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("Assets/_Scripts/Crafting/GraphView/Refs/CraftingGraphStylesheet.uss");
        styleSheets.Add(styleSheet);
    }
}
