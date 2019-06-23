using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Add this component to your UI buttons
/// </summary>
public class LevelGeneratorUI : MonoBehaviour
{
    public string m_LevelFileName;
    public int m_LevelGeneratorBuildIndex = 1;

    public void LoadLevel()
    {
        LevelGeneratorUtility.LevelFileName = m_LevelFileName;
        SceneManager.LoadScene(m_LevelGeneratorBuildIndex);
    }
}