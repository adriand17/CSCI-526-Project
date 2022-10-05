using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] public BlockType Type;

    public void InitializeButton()
    {
        if (Type == BlockType.None)
        {
            gameObject.SetActive(false);
        }
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonClicked);
        gameObject.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(Type.ToString());
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Type.ToString();
    }

    public void ButtonClicked()
    {
        GameManager _gameManager = GameManager.Instance;
        _gameManager._gridManager.SetBlockBuildType(Type);
    }
    
}
