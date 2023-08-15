using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BD_ProgressBar : MonoBehaviour
{
    public int PoolSize = 100;
    public int Size = 5;
    public bool ShowIncrement = true;
    [HideInInspector]
    public TextMeshProUGUI BarText;
    public string BarTitle = "New Bar";
    [HideInInspector]
    public Image Icon;
    [HideInInspector] public Image IconBg, TextBg;
    public Sprite IconSprite;
    public Color enableColor, disableColor;
    [Range(0, 10)]
    public int BarLevel = 1;
    public int DefaultLevel = 1;

    private Queue<GameObject> segments = null, pool = null;

    private void OnEnable()
    {
        BarText = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        Icon = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();
        IconBg = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        TextBg = transform.GetChild(1).GetChild(0).GetComponent<Image>();



        CheckPool();
    }


    public void PaintUI()
    {
        UpdateBar();
        int i = 0;
        foreach (GameObject item in segments)
        {
            try
            {
                if (BarLevel <= i)
                    item.GetComponent<Image>().color = disableColor;
                else
                    item.GetComponent<Image>().color = enableColor;
                i++;
            }
            catch (System.Exception)
            {
                EnsureIntegrity();
            }

        }
        if (ShowIncrement)
            BarText.SetText(BarTitle + ": " + (BarLevel + DefaultLevel).ToString());
        else
            BarText.SetText(BarTitle);
        SetBarName();
        Icon.sprite = IconSprite;
        IconBg.color = disableColor;
        TextBg.color = enableColor;
    }

    private GameObject root;
    public void CreateSegmentPool(int size)
    {
        pool = new Queue<GameObject>();
        root = new GameObject("bar_root");
        root.transform.SetParent(transform);
        for (int i = 0; i < size; i++)
        {
            GameObject tempSegment = new GameObject("Segment");
            tempSegment.transform.SetParent(root.transform);
            tempSegment.AddComponent<Image>().color = disableColor;
            Shadow shadow = tempSegment.AddComponent<Shadow>();
            tempSegment.GetComponent<Shadow>().effectDistance.Set(-3, -5);
            tempSegment.GetComponent<Image>().raycastTarget = false;
            //let me disable the segment before putting it to pool.
            tempSegment.gameObject.SetActive(false);
            pool.Enqueue(tempSegment);
        }
        segments = new Queue<GameObject>();
    }


    private bool IsIncreasing()
    {
        try
        {
            return segments.Count < Size;
        }
        catch (System.Exception)
        {
            EnsureIntegrity();
            return segments.Count < Size;
        }
    }

    private void AddToBar()
    {
        GameObject tempSegment;
        int itemsToAdd = Size - segments.Count;

        for (int i = 0; i < itemsToAdd; i++)
        {
            tempSegment = pool.Dequeue();
            tempSegment.SetActive(true);
            tempSegment.transform.SetParent(transform.Find("Bar"));
            segments.Enqueue(tempSegment);
        }
    }

    private void RemoveFromBar()
    {
        GameObject tempSegment;
        int itemsToRemove = segments.Count - Size;

        for (int i = 0; i < itemsToRemove; i++)
        {
            tempSegment = segments.Dequeue();
            tempSegment.SetActive(false);
            tempSegment.transform.SetParent(root.transform);
            pool.Enqueue(tempSegment);
        }
    }
    public void CheckPool()
    {

        if (!HasPool())
        {
            Debug.Log("Creating Pool");
            ClearBar();
            pool = null;
            CreateSegmentPool(PoolSize);
        }
        else
        {
            if (pool == null)
            {
                root = transform.Find("bar_root").gameObject;
                pool = new Queue<GameObject>();
                for (int i = 0; i < root.transform.childCount; i++)
                {
                    pool.Enqueue(root.transform.GetChild(i).gameObject);
                }
            }
        }


    }

    public bool HasPool()
    {
        print(transform.childCount);
        if (transform.Find("bar_root") != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ClearBar()
    {
        if (transform.Find("Bar").childCount > 0)
        {
            int childCount = transform.Find("Bar").childCount;
            for (int i = 0; i < childCount; i++)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(transform.Find("Bar").GetChild(0).gameObject);
#else
                GameObject.Destroy(transform.Find("Bar").GetChild(0).gameObject);
#endif
            }
        }
    }

    public void EnsureIntegrity()
    {
        int childCount = transform.Find("Bar").transform.childCount;

        if (segments == null)
        {
            segments = new Queue<GameObject>();
            for (int i = 0; i < childCount; i++)
            {
                GameObject segmentObj = transform.Find("Bar").GetChild(i).gameObject;
                segments.Enqueue(segmentObj);
            }
        }
        else
        {
            if (Size - childCount > 0)
            {
                //we ha a deficit. add segments.
                for (int i = 0; i < Size - childCount; i++)
                {
                    GameObject segmentObj = pool.Dequeue();
                    segmentObj.transform.SetParent(transform.Find("Bar"));
                    segmentObj.transform.localPosition = Vector3.zero;
                    segmentObj.SetActive(true);
                    segments.Enqueue(segmentObj);
                }
            }
            else if (childCount - Size > 0)
            {
                //there are too many segments in the bar. Reduce.
                for (int i = 1; i <= childCount - Size; i++)
                {
                    GameObject segmentObj = segments.Dequeue();
                    segmentObj.transform.SetParent(root.transform);
                    segmentObj.transform.localPosition = Vector3.zero;
                    segmentObj.SetActive(false);
                    pool.Enqueue(segmentObj);

                }
            }
        }
    }

    private void UpdateBar()
    {
        if (IsIncreasing())
            AddToBar();
        else
            RemoveFromBar();
        EnsureIntegrity();
    }

    private void SetBarName()
    {
        gameObject.name = BarTitle;
    }

    public void Increment()
    {
        EnsureIntegrity();
        if (BarLevel + 1 <= DefaultLevel + Size)
            BarLevel++;
        PaintUI();
        print("Increased.");
    }

    public void Decrement()
    {
        EnsureIntegrity();
        if (BarLevel - 1 >= DefaultLevel)
            BarLevel--;
        PaintUI();
        print("Decreased.");
    }
}
