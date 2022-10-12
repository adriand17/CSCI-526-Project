using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;

    static private int popUpIndex = 0;

    public float waitTime = 1f;

    void Update()
    {
        if (popUpIndex >= popUps.Length)
        {
            return;
        }

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        int pre = popUpIndex - 1;

        // particle selection
        if (popUpIndex == 0)
        {
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
                waitTime = 1f;
            }
        }

        // place tile 
        else if (popUpIndex == 1)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
                waitTime = 4f;
            }
        }

        // spawn next wave
        else if (popUpIndex == 2)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
                waitTime = 3f;
            }
        }

        // use laser
        else if (popUpIndex == 3)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                popUpIndex++;
                waitTime = 4f;
            }
        }

        // start game
        else if (popUpIndex == 4)
        {
            popUps[pre].SetActive(false);
            popUps[popUpIndex].SetActive(true);
        }

    }
}
