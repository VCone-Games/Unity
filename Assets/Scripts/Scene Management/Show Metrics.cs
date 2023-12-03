using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowMetrics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI metricsText;
    // Start is called before the first frame update
    void Start()
    {
        DataBase data = DataBase.Singleton;
        float timer = data.TimerGame;
        string user = data.Username;
        int nDeaths = data.DeathCount;
        int parryTimes = data.ParriedTimes;
        int nEnemies = data.DeathEnemies;
        int coleccionables = data.Coleccionables;

		int minutos = Mathf.FloorToInt(timer / 60);
		int segundos = Mathf.FloorToInt(timer % 60);

		string text =
            $"Usuario: {user}\n" +
            $"Tiempo: {minutos.ToString("00")}:{segundos.ToString("00")}\n" +
            $"Numero de muertes: {nDeaths}\n" +
            $"Numero de contraataques: {parryTimes}\n" +
            $"Numero de enemigos derrotados: {nEnemies}\n" +
            $"Coleccionables recogidos: {coleccionables}";
        metricsText.text = text;
    }
}
