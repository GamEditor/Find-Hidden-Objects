using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// this class provides a window with using a unity's option menu and in that window,
/// there are some fields about HiddenObjects and some empty fields for manually jobs.
/// 
/// reference: https://docs.unity3d.com/Manual/editor-EditorWindows.html
/// reference: https://answers.unity.com/questions/859554/editorwindow-display-array-dropdown.html
/// save reference: https://docs.unity3d.com/ScriptReference/EditorUtility.SaveFilePanel.html
/// 
/// editor reference: https://docs.unity3d.com/ScriptReference/Editor.html
/// </summary>

public class LevelParser : EditorWindow
{
    private HiddenObject[] m_HiddenObjectsOnCurrentScene;
    private LevelInfo m_HiddenObjectInformations;

    private string m_FileName = "FileName";

    [MenuItem("Level Generator/Generate Background")]
    private static void GenerateBackground()
    {
        if (GameObject.Find("Background"))
            return;
        else
        {
            GameObject obj = new GameObject("Background");
            obj.AddComponent<SpriteRenderer>();

            SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
            sp.sprite = Resources.Load("Default_Background", typeof(Sprite)) as Sprite;
            sp.sortingOrder = 0;
        }
    }

    private static void GenerateHiddenObjectsParent()
    {
        GameObject parent = new GameObject("HiddenObjectsParent");
    }

    [MenuItem("Level Generator/Generate HiddenObject")]
    private static void GenerateHiddenObject()
    {
        if (!GameObject.Find("HiddenObjectsParent"))
            GenerateHiddenObjectsParent();

        GameObject obj = new GameObject();
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<HiddenObject>();

        SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
        sp.sprite = Resources.Load("Default_HiddenObject", typeof(Sprite)) as Sprite;
        sp.sortingOrder = 1;

        obj.name = sp.sprite.name;
        obj.transform.parent = GameObject.Find("HiddenObjectsParent").transform;
    }


    /// <summary>
    /// this method will retrieve all required informations for us and we just
    /// press a button in the loaded window to save informations on a file in the resources folder
    /// </summary>
    [MenuItem("Level Generator/Get the scene informations")]
    private static void SceneInformations()
    {
        EditorWindow.GetWindow(typeof(LevelParser));    // start a custom window
    }

    // The actual window code goes here
    private void OnGUI()
    {
        m_HiddenObjectsOnCurrentScene = FindObjectsOfType<HiddenObject>();

        if (!GameObject.Find("Background"))
            GenerateBackground();

        if (m_HiddenObjectsOnCurrentScene.Length == 0)
        {
            //EditorUtility.DisplayDialog("Warning", "There are no HiddenObject on this scene!", "Ok");
            GUILayout.Label("There are no HiddenObject on this scene!\nA default Background is generated.\nPlease run (Level Generator>Generate HiddenObject) from menu.", EditorStyles.helpBox);
        }
        else
        {
            m_HiddenObjectInformations = new LevelInfo()
            {
                BackgroundSprite = "",
                PictureNames = new string[m_HiddenObjectsOnCurrentScene.Length],
                Scales = new Vector2[m_HiddenObjectsOnCurrentScene.Length],
                Positions = new Vector3[m_HiddenObjectsOnCurrentScene.Length]
            };

            GUILayout.BeginVertical();

            m_FileName = EditorGUILayout.TextField("Enter the FileName", m_FileName);

            GUILayout.Label("Level Informations", EditorStyles.boldLabel);
            m_HiddenObjectInformations.BackgroundSprite = GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite.name;
            EditorGUILayout.LabelField("Backgroud Name:", m_HiddenObjectInformations.BackgroundSprite);

            GUILayout.EndVertical();

            for (int i = 0; i < m_HiddenObjectsOnCurrentScene.Length; i++)
            {
                GUILayout.BeginHorizontal();

                m_HiddenObjectInformations.PictureNames[i] = m_HiddenObjectsOnCurrentScene[i].GetComponent<SpriteRenderer>().sprite.name;
                EditorGUILayout.LabelField("Picture Name " + (i + 1) + ':', m_HiddenObjectInformations.PictureNames[i]);

                m_HiddenObjectInformations.Scales[i] = m_HiddenObjectsOnCurrentScene[i].transform.localScale;
                EditorGUILayout.LabelField("Scale " + (i + 1) + ':', m_HiddenObjectInformations.Scales[i].ToString());

                m_HiddenObjectInformations.Positions[i] = m_HiddenObjectsOnCurrentScene[i].transform.position;
                EditorGUILayout.LabelField("Position " + (i + 1) + ':', m_HiddenObjectInformations.Positions[i].ToString());

                GUILayout.EndHorizontal();
            }

            if(GUILayout.Button("Save Data"))
            {
                string path = EditorUtility.SaveFilePanel("Save Level Informations as json in Resourses folders", "", m_FileName, "txt");
                string json = JsonUtility.ToJson(m_HiddenObjectInformations);

                if (path.Length != 0)
                    File.WriteAllText(path, json);

                AssetDatabase.Refresh();
            }
        }
    }
}