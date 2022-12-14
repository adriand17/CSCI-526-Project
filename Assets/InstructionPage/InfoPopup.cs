using UnityEngine;

public class InfoPopup : MonoBehaviour
{
    public GameObject InfoPanel;

    public float waitTime = 3f;
    bool skip = false;

    void Update()
    {
        if (!skip && waitTime <= 0)
        {
            InfoPanel.SetActive(false);
            skip = true;
        } else {
            waitTime -= Time.deltaTime;
        }
        
    }

    public void clickInfo() {
        if (!Input.GetKeyDown(KeyCode.Space))
            Debug.Log(InfoPanel.activeSelf);
            InfoPanel.SetActive(!InfoPanel.activeSelf);
    }

}
