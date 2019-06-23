using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager Inctance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameManager>();

                if (_Instance == null)
                    throw new System.NullReferenceException("Can't find instance of GameManager!");

                return _Instance;
            }
            else
                return _Instance;
        }
    }

    public float m_ItweenAnimationDelay = 0.5f;

    public enum Status { Playing, TimeUp, Win, }
    public Status m_Status = Status.Playing;

    public int m_AmountOfHiddenList = 4;

    public List<HiddenObject> m_HiddenObjects;  // Objects on the scene
    private int m_FoundedIndex;

    [SerializeField] private List<HiddenObject> m_HiddenList;   // buffer list on the bottom of screen

    [Header("UI")]
    public Image m_HiddenUIPrefab;
    public Canvas m_UIParent;
    [SerializeField] private Image[] m_HiddenUI;

    private void Start()
    {
        m_Status = Status.Playing;

        m_HiddenObjects = FindObjectsOfType<HiddenObject>().ToList();

        m_HiddenUI = new Image[m_AmountOfHiddenList];
        m_HiddenList = new List<HiddenObject>();

        RectTransform parentCanvas = m_UIParent.GetComponent<RectTransform>();
        parentCanvas.pivot = new Vector2(0, 0.5f);

        float canvasSize = 0;
        for (int i = 0; i < m_AmountOfHiddenList; i++)
        {
            m_HiddenUI[i] = Instantiate(m_HiddenUIPrefab, m_UIParent.transform);
            m_HiddenUI[i].rectTransform.sizeDelta = new Vector2(parentCanvas.rect.size.y, parentCanvas.rect.size.y);

            canvasSize += m_HiddenUI[i].rectTransform.sizeDelta.x;
            m_HiddenUI[i].rectTransform.localPosition = new Vector3(i * m_HiddenUI[i].rectTransform.sizeDelta.x, 0, 0);

            int index = Random.Range(0, m_HiddenObjects.Count);
            m_HiddenList.Add(m_HiddenObjects[index]);
            m_HiddenObjects.RemoveAt(index);

            m_HiddenUI[i].transform.Find("Image").GetComponent<Image>().sprite = m_HiddenList[i].GetComponent<SpriteRenderer>().sprite;
        }

        parentCanvas.sizeDelta = new Vector2(canvasSize, parentCanvas.sizeDelta.y);
        parentCanvas.pivot = new Vector2(0.5f, 0.5f);
        parentCanvas.localPosition = new Vector3(0, parentCanvas.position.y);
    }

    public void CheckStatus()
    {
        switch (m_Status)
        {
            case Status.Playing:
                break;  // do nothing
            case Status.TimeUp:
                // notify user he loses the game
                Debug.Log("Time's up!");
                break;
            case Status.Win:
                // notify user he wins the game
                Debug.Log("You won!");
                break;
        }
    }

    public bool ExistInCurrentList(string objectName)
    {
        for (int i = 0; i < m_HiddenList.Count; i++)
            if (m_HiddenList[i].m_ObjectName == objectName)
            {
                m_FoundedIndex = i;
                return true;
            }

        return false;
    }

    public void RemoveHiddenFromList(GameObject removedObject)
    {
        iTween.FadeTo(removedObject, 0, m_ItweenAnimationDelay * 2);
        iTween.ScaleTo(m_HiddenUI[m_FoundedIndex].transform.Find("Image").gameObject, Vector3.zero, m_ItweenAnimationDelay);

        if(m_HiddenObjects.Count > 0)
        {
            int index = Random.Range(0, m_HiddenObjects.Count);

            m_HiddenList[m_FoundedIndex] = m_HiddenObjects[index];
            m_HiddenObjects.RemoveAt(index);

            System.Action getNewItem = () =>
            {
                iTween.ScaleTo(m_HiddenUI[m_FoundedIndex].transform.Find("Image").gameObject, Vector3.one, m_ItweenAnimationDelay);
                m_HiddenUI[m_FoundedIndex].transform.Find("Image").GetComponent<Image>().sprite = m_HiddenList[m_FoundedIndex].GetComponent<SpriteRenderer>().sprite;
            };
            Invoke(getNewItem.Method.Name, m_ItweenAnimationDelay);
        }

        if(CheckWin() == true)
        {
            m_Status = Status.Win;
            CheckStatus();
        }
    }

    private bool CheckWin()
    {
        for(int i = 0; i < m_HiddenList.Count; i++)
            if (m_HiddenList[i].m_CanDetect == true)
                return false;

        return true;
    }

    public void Hint()
    {
        // show and delete one of m_HiddenList's elements
    }
}