using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelector_N : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OpenLevel1()
    {
        SceneManager.LoadScene("1 Glass Level");
    }
    public void OpenLevel2()
    {
        SceneManager.LoadScene("2 Mirror Level");
    }
    public void OpenLevel3()
    {
        SceneManager.LoadScene("3 Dirt Level");
    }
    public void OpenLevel4()
    {
        SceneManager.LoadScene("4 Magma_TNT");
    }
    public void OpenLevel5()
    {
        SceneManager.LoadScene("5 Vapor");
    }
    public void OpenLevel6()
    {
        SceneManager.LoadScene("Mixed Level");
    }
//        public void OpenLevel7()
//    {
//        SceneManager.LoadScene("");
//    }
//        public void OpenLevel8()
//    {
//        SceneManager.LoadScene("");
//    }
//        public void OpenLevel9()
//    {
//        SceneManager.LoadScene("");
//    }public void OpenLevel10()
//    {
//        SceneManager.LoadScene("");
//    }public void OpenLevel11()
//    {
//        SceneManager.LoadScene("");
//    }public void OpenLeve12()
//    {
//        SceneManager.LoadScene("");
//    }
}
