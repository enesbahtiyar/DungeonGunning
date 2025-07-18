using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenü : MonoBehaviour

   
{
     public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
