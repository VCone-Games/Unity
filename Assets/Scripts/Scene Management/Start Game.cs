using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private InputField nameField;
    
    public void OnButtonPressed_StartGame()
    {
        SceneManager.LoadScene("0. Tutorial");
        gameObject.AddComponent<DataBase>();
        DataBase.Singleton.Username = nameField.text;
        Destroy(this);
    }

}
