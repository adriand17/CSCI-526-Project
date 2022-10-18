using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private BlockType Type;

    public void InitializeButton(BlockType type)
    {
        Type = type;
        if (type == BlockType.None)
        {
            gameObject.SetActive(false);
            return;
        }
        Type = type;
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonClicked);
        gameObject.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(type.ToString());
    }

    public void ButtonClicked()
    {
        GameManager _gameManager = GameManager.Instance;
        _gameManager._gridManager.SetBlockBuildType(Type);
    }
    
}
