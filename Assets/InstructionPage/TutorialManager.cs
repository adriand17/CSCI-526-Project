using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;

    static private int popUpIndex = 1;

    public float waitTime = 3f;

    void Update()
    {
        if (popUpIndex > popUps.Length)
        {
            return;
        }

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        int pre = popUpIndex - 1;

        // use laser
        if (popUpIndex == 1)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
             if (Input.GetKeyDown(KeyCode.Space))
            {
                popUpIndex++;
                waitTime = 3f;
            }
        }

        // particle selection
        else if (popUpIndex == 2)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
                waitTime = 1f;
            }
        }

        // place tile
        else if (popUpIndex == 3)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
                waitTime = 3f;
            }
        }

        // remove block
        else if (popUpIndex == 4)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
                waitTime = 2f;
            }
        }

        // where to place block
        else if (popUpIndex == 5)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            popUpIndex++;
            waitTime = 3f;
            
        }

        // reset
        else if (popUpIndex == 6)
        {
       
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            popUpIndex++;
            waitTime = 4f;
            
        }

        // start game
        else if (popUpIndex == 7)
        {
            popUps[pre].SetActive(false);
        }

    }
}
