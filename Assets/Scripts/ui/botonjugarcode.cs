using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Collections;

public class botonjugarcode : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
   

    public void PresionarJugar()
    {

        SceneManager.LoadScene(sceneToLoad); /*puta mierda no funciona toca loadearla pero es un codigo re largo dylan no me mates porfa */
        Time.timeScale = 1f;


    }

    
}
