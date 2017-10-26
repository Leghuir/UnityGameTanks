using UnityEngine;
using System.Collections;

public class Zdrowie : MonoBehaviour {
    public int zdrowie = 100;
    public GameObject gui;
    
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void powieksz_zycie(int ilosc)
    {
        zdrowie += ilosc;
    }
    public void zadajObrazenia(int ilosc)
    {
        if (gameObject.name == "Czolg")
            graczowi(ilosc);
        else
            komputerowi(ilosc);
    }
    public void komputerowi(int ilosc)
    {
        zdrowie -= ilosc;
        if (zdrowie <= 0)
        {
            PlayerPrefs.SetInt("Punkty", PlayerPrefs.GetInt("Punkty") + 10);
            Przeciwnik.przeciwnicy_zyjacy--;
            Destroy(transform.gameObject);

        }

    }
    public void graczowi(int ilosc)
    {
        PlayerPrefs.SetInt("Zycie", PlayerPrefs.GetInt("Zycie") - ilosc);
        if(PlayerPrefs.GetInt("Zycie")<=0)
        {
            Camera.main.transform.parent = null;
            Destroy(transform.gameObject);
            
        }
    }
}
