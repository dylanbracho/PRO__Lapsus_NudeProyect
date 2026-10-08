using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Collections;

public class botonjugarcode : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Level2";
    [SerializeField] private bool sceneSwitch = false;

    public void PresionarJugar()
    {

        SceneManager.LoadScene(0); /*puta mierda no funciona toca loadearla pero es un codigo re largo dylan no me mates porfa */
                    
               
    }

    
}
