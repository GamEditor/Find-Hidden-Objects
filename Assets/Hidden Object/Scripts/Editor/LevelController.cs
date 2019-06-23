using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObject))]
[CanEditMultipleObjects]

public class LevelController : Editor
{
    private HiddenObject[] m_HiddenObjectsOnCurrentScene;
    private GameObject[] m_GameObjects;
    private bool m_HasOneBackground = false;

    public override void OnInspectorGUI()
    {
        m_HiddenObjectsOnCurrentScene = FindObjectsOfType<HiddenObject>();
        m_GameObjects = FindObjectsOfType<GameObject>();

        for(int i = 0; i < m_HiddenObjectsOnCurrentScene.Length; i++)
            if (m_HiddenObjectsOnCurrentScene[i].transform.parent != GameObject.Find("HiddenObjectsParent").transform)
                m_HiddenObjectsOnCurrentScene[i].transform.parent = GameObject.Find("HiddenObjectsParent").transform;

        for (int i = 0; i < m_GameObjects.Length; i++)
        {
            if (m_HasOneBackground == false && m_GameObjects[i].name == "Background")
                m_HasOneBackground = true;
            else if(m_HasOneBackground == true && m_GameObjects[i].name == "Background")
                DestroyImmediate(m_GameObjects[i]);
        }
        m_HasOneBackground = false;
    }
}