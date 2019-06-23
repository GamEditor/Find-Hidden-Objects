using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	void Awake ()
    {
        TextAsset jsonText = Resources.Load(LevelGeneratorUtility.LevelFileName) as TextAsset;
        LevelInfo info = JsonUtility.FromJson<LevelInfo>(jsonText.text);

        GameObject background = new GameObject("Background");

        background.AddComponent<SpriteRenderer>();
        SpriteRenderer spb = background.GetComponent<SpriteRenderer>();
        spb.sprite = Resources.Load(info.BackgroundSprite, typeof(Sprite)) as Sprite;
        spb.sortingOrder = 0;

        background.transform.position = Vector3.zero;
        background.transform.localScale = Vector3.one;

        GameObject parent = new GameObject("HiddenObjectsParent");

        for (int i = 0; i < info.PictureNames.Length; i++)
        {
            GameObject hiddenObject = new GameObject(info.PictureNames[i]);

            hiddenObject.transform.parent = parent.transform;

            hiddenObject.AddComponent<SpriteRenderer>();
            SpriteRenderer sp = hiddenObject.GetComponent<SpriteRenderer>();
            sp.sprite = Resources.Load(info.PictureNames[i], typeof(Sprite)) as Sprite;
            sp.sortingOrder = 1;

            hiddenObject.transform.localScale = info.Scales[i];
            hiddenObject.transform.position = info.Positions[i];

            hiddenObject.AddComponent<PolygonCollider2D>();
            hiddenObject.AddComponent<HiddenObject>();
        }
    }
}