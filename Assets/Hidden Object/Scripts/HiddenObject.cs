using System.Linq;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    [HideInInspector] public string m_ObjectName;

    [HideInInspector] public bool m_CanDetect = true;

    private void Awake()
    {
        m_ObjectName = GenerateRandomName();
    }

    private static System.Random random = new System.Random();
    private static string GenerateRandomName()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    void OnMouseDown()
    {
        if (m_CanDetect)
            if (GameManager.Inctance.ExistInCurrentList(m_ObjectName))
            {
                m_CanDetect = false;
                GameManager.Inctance.RemoveHiddenFromList(gameObject);
            }
    }
}