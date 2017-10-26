using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    // Use this for initialization
    public GameObject interfejs;
    public GameObject gui;
    private GameObject[] gracz ;
    public int limitpunktow;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;

    public object SceneMenager { get; private set; }

    void Start()
    {
        Cursor.visible = false;
        limitpunktow = 20;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("Player") == null & Przeciwnik.przeciwnicy_zyjacy > 0)
        {
            Cursor.visible= true;
            Debug.Log(Przeciwnik.przeciwnicy_zyjacy);
            gui.SetActive(true);
        }
        else
        {
            Debug.Log("nie zginol"+ GameObject.FindGameObjectWithTag("Player"));
            gui.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            gracz = GameObject.FindGameObjectsWithTag("Player");
            pokazMenu();
            deaktywujPrzyciski();
            uaktywnijPrzyciski();
        }

        

    }
    public void laduj_poczatkowa()
    {
        SceneManager.LoadScene("Poczatkowa");
    }
    private void pokazMenu()
    {
        if(interfejs!=null)
        {
            interfejs.SetActive(!interfejs.activeSelf);
            if (interfejs.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
                Time.timeScale = 1;
        }
    }
    public void obrazenia()
    {
        gracz[0].GetComponent<Gracz>().powieszObrazenia();
        deaktywujPrzyciski();
    }
    public void szybkostrzelnosc()
    {
        gracz[0].GetComponent<Gracz>().powieszSzybkostrzelnosc();
        deaktywujPrzyciski();

    }
    public void szybkosc()
    {
        gracz[0].GetComponent<Gracz>().powieszSzybkosc();
        deaktywujPrzyciski();
    }
    public void zycie()
    {
        gracz[0].GetComponent<Gracz>().powieszZycie();
        deaktywujPrzyciski();
    }
    public void uaktywnijPrzyciski()
    {
        if(PlayerPrefs.GetInt("Punkty") >= limitpunktow)
        {
            button1.GetComponent<Button>().interactable = true;
            button2.GetComponent<Button>().interactable = true;
            button3.GetComponent<Button>().interactable = true;
            button4.GetComponent<Button>().interactable = true;
        }
    }
    public void deaktywujPrzyciski()
    {
        if (PlayerPrefs.GetInt("Punkty") < limitpunktow)
        {
            button1.GetComponent<Button>().interactable = false;
            button2.GetComponent<Button>().interactable = false;
            button3.GetComponent<Button>().interactable = false;
            button4.GetComponent<Button>().interactable = false;
        }
    }
}
