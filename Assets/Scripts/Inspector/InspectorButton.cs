using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// <para>A class used to place a button on the inspector editor that will trigger a method in the base class.</para>
/// <para>Pass in the method name and optionally an array of parameters in the constructor.</para>
/// <para>Usage: "public InspectorButton actionButton = new InspectorButton("ActionMethod", new object[] { "Argument1", 2 });"</para>
/// </summary>
[System.Serializable]
public class InspectorButton
{
    public string MethodName;
    public object[] Parameters;
    public string DisplayName;

    public InspectorButton(string methodName, object[] parameters = null, string displayName = "")
    {
        MethodName = methodName;
        Parameters = parameters;
        DisplayName = displayName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButton))]
public class InspectorButtonDrawer : PropertyDrawer
{
    private MethodInfo _methodInfo = null;

    private Dictionary<string, object> parameterList = new Dictionary<string, object>();

    private bool showParameters;

    private string methodName;

    private float controlHeight;

    private const float margin = 5;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        controlHeight = EditorGUI.GetPropertyHeight(SerializedPropertyType.Float, GUIContent.none);

        var totalHeight = controlHeight;

        methodName = prop.FindPropertyRelative("MethodName").stringValue;

        var methodInfo = prop.serializedObject.targetObject.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        if (methodInfo != null)
        {
            var parameterInfos = methodInfo.GetParameters();

            if (parameterInfos.Length > 0)
            {
                totalHeight *= 2;

                for (int i = 1; i <= parameterInfos.Length; i++)
                {
                    totalHeight += controlHeight;
                }
            }
        }
        else
        {
            Debug.LogWarning("InspectorButton: Method '" + methodName + "' Not Found in Target.");
        }
       

        totalHeight += margin * 2;

        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        var propName = prop.name;

        var target = prop.serializedObject.targetObject;
        InspectorButton btn = (InspectorButton)fieldInfo.GetValue(target);

        var MethodName = prop.FindPropertyRelative("MethodName").stringValue;
        var DisplayName = prop.FindPropertyRelative("DisplayName").stringValue;
        var ParamsProp = prop.FindPropertyRelative("Parameters");

        List<object> defaultParams = new List<object>();
        if (btn != null && btn.Parameters != null)
        {
            for (int i = 0; i < btn.Parameters.Length; i++)
            {
                defaultParams.Add(btn.Parameters[i]);
            }
        }

        GUI.Box(position, "", GUI.skin.box);

        var eventOwnerType = prop.serializedObject.targetObject.GetType();

        var methodInfo = eventOwnerType.GetMethod(MethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);


        if (methodInfo != null)
        {
            var parameterInfos = methodInfo.GetParameters();

            var buttonRect = new Rect(position.x + margin, position.y + margin, position.width - (margin * 2), controlHeight);
            if (GUI.Button(buttonRect, DisplayName.Length > 0 ? DisplayName : MethodName))
            {
                object[] parameters = new object[parameterList.Count];
                parameterList.Values.CopyTo(parameters, 0);
                if (_methodInfo == null)
                    _methodInfo = eventOwnerType.GetMethod(MethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (_methodInfo != null)
                    _methodInfo.Invoke(prop.serializedObject.targetObject, parameters);
                else
                    Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", MethodName, eventOwnerType));

            }
            if (parameterInfos.Length > 0)
            {
                var hMultipler = showParameters ? parameterInfos.Length : 1;

                var foldoutRect = new Rect(position.x + margin + 10, position.y + margin + (controlHeight), position.width - (margin * 2), controlHeight);

                var sb = new StringBuilder();

                sb.Append("Parameters [ ");
                foreach (KeyValuePair<string, object> pair in parameterList)
                {
                    sb.Append(pair.Key + ":" + pair.Value.ToString() + " ");
                }
                sb.Append("]");

                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    var rect = new Rect(position.x + margin, position.y + margin + (controlHeight * 2) + (controlHeight * i), position.width - (margin * 2), controlHeight);
                    var name = parameterInfos[i].Name;

                    if (parameterInfos[i].ParameterType == typeof(string))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : string.Empty;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.TextField(rect, name, (string)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(int))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : 0;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.IntField(rect, name, (int)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(float))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : 0f;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.FloatField(rect, name, (float)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(bool))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : false;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.Toggle(rect, name, (bool)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(Vector2))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : Vector2.zero;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.Vector2Field(rect, name, (Vector2)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(Vector3))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : Vector3.zero;
                            parameterList.Add(name, Vector3.zero);
                        }
                        parameterList[name] = EditorGUI.Vector3Field(rect, name, (Vector3)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(Vector4))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : Vector4.zero;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.Vector4Field(rect, name, (Vector4)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType.IsEnum)
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? (int)defaultParams[i] : 0;
                            var defaultVal = Enum.GetValues(parameterInfos[i].ParameterType).GetValue(def);
                            parameterList.Add(name, defaultVal);
                        }
                        var enumType = parameterInfos[i].ParameterType;
                        var val = Convert.ChangeType(parameterList[name], enumType);
                        parameterList[name] = EditorGUI.EnumPopup(rect, name, (Enum)parameterList[name]);
                    }
                    else if (parameterInfos[i].ParameterType == typeof(UnityEngine.Object))
                    {
                        if (!parameterList.ContainsKey(name))
                        {
                            var def = (i < defaultParams.Count && defaultParams[i] != null) ? defaultParams[i] : null;
                            parameterList.Add(name, def);
                        }
                        parameterList[name] = EditorGUI.ObjectField(rect, name, (UnityEngine.Object)parameterList[name], parameterInfos[i].ParameterType, true);
                    }
                }
            }
        
        }
        else
        {
            EditorGUI.LabelField(position, "Method '" + methodName + "' Not Found");
        }

    }
}
#endif