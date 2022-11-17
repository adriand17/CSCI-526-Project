using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDict : MonoBehaviour
{
    public GameObject DictPanel;
    public void LoadScene()
    {
//        SceneManager.LoadScene("BlockSystem");
            DictPanel.SetActive(true);

    }
//     int y = SceneManager.GetActiveScene().buildIndex;
//            yield return new WaitForSeconds(Timer);
//            print("15 sec wait");  
//            SceneManager.UnloadSceneAsync(y);
     public void GoBack()
    {
            DictPanel.SetActive(false);
//         int y = SceneManager.GetActiveScene().buildIndex;
//         SceneManager.UnloadSceneAsync(y);
    }
}