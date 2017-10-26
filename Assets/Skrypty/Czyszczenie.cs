using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Czyszczenie : MonoBehaviour {

	// Use this for initialization
	void Start () {
        funkcja();

    }
	
    public void funkcja()
    {
        PlayerPrefs.DeleteAll();
        Przeciwnik.przeciwnicy_zyjacy = 0;
        PlayerPrefs.SetInt("Poziom", 0);
        PlayerPrefs.SetInt("RozmiarPlanszy", 10);
        SceneManager.LoadScene("Plansza");
    }

}
