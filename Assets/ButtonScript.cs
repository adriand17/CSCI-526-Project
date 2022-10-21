using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (Type == BlockType.Dirt){
            Tooltip.ShowTooltip_static("Dirt");
        }
        else if (Type == BlockType.Mirror){
            Tooltip.ShowTooltip_static("Mirror");
        }
        else if (Type == BlockType.Glass){
            Tooltip.ShowTooltip_static("Glass");
        }
        else if (Type == BlockType.Magma){
            Tooltip.ShowTooltip_static("Magma");
        }
        else if (Type == BlockType.BlueIce){
            Tooltip.ShowTooltip_static("BlueIce");
        }
        else if (Type == BlockType.TNT){
            Tooltip.ShowTooltip_static("TNT");
        }
        else if (Type == BlockType.Bedrock){
            Tooltip.ShowTooltip_static("Bedrock");
        }
        else if (Type == BlockType.Evaporator){
            Tooltip.ShowTooltip_static("Evaporator");
        }
        else if (Type == BlockType.Condensation){
            Tooltip.ShowTooltip_static("Condensation");
        }
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
       Tooltip.HideTooltip_static();
    }
}      
