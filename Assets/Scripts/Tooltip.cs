using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;

    [SerializeField] public GridManager gridManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Text tooltipText;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private RectTransform tooltipTextRectTransform;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        instance = this;
        tooltipText = gameObject.transform.Find("text").GetComponent<Text>();
        tooltipTextRectTransform = gameObject.transform.Find("text").GetComponent<RectTransform>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        HideTooltip();
    }

    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        tooltipTextRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPos;
  
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(),
            Input.mousePosition, null, out localPos);

        localPos.x += -tooltipText.preferredWidth+5f;
        localPos.y += 20f;
        transform.localPosition = localPos;
    }

    public static void ShowTooltip_static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_static()
    {
        instance.HideTooltip();
    }
}