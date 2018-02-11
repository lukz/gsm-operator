using System;
using UnityEngine;
using UnityEditor;
using BlendModeShader2D;


public class BlendModeMaterialEditor : MaterialEditor
{
    Material _targetMaterial;
    BlendMode _selectedBlendMode;

    public override void OnEnable()
    {
        base.OnEnable();

        _targetMaterial = (Material)target;

        _selectedBlendMode = BlendMode.Normal;
        foreach (string key in _targetMaterial.shaderKeywords)
        {
            if (key.StartsWith("_BM"))
            {
                _selectedBlendMode = (BlendMode)Enum.Parse(typeof(BlendMode), key.Replace("_BM_", string.Empty), true);
                break;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!isVisible) return;

        GUILayout.Space(10);

        EditorGUI.BeginChangeCheck();
        _selectedBlendMode = (BlendMode)EditorGUILayout.EnumPopup("Blend Mode", _selectedBlendMode);
        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < _targetMaterial.shaderKeywords.Length; i++)
            {
                if (_targetMaterial.shaderKeywords[i].StartsWith("_BM_"))
                {
                    _targetMaterial.DisableKeyword(_targetMaterial.shaderKeywords[i]);
                }
            }
            _targetMaterial.EnableKeyword("_BM_" + _selectedBlendMode.ToString().ToUpper());
            EditorUtility.SetDirty(_targetMaterial);
        }
    }
}


