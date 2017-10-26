using UnityEngine;
using System.Collections;

public class Gracz : MonoBehaviour
{
    public GameObject lufa;
    public GameObject pocisk;
    public float predkoscPoruszania = 9.0f;
    public float bieg2 = 7.0f;
    public float czuloscMyszki = 3.0f;
    public float myszGoraDol = 0.0f;
    public float myszLewoPrawo = 0.0f;
    public float zakresMyszyGoraDol = 90.0f;
    public static int punkty;
    public int obrazenia = 10;
    public float czasDoWystrzalu=5;
    private float czasOdWystrzalu = 5;
    void Start()
    {
        if(PlayerPrefs.GetInt("Obrazenia")==0)
        {
            PlayerPrefs.SetInt("Obrazenia", obrazenia);
        }
        if (PlayerPrefs.GetInt("Punkty") == 0)
        {
            PlayerPrefs.SetInt("Punkty", 0);
        }
        if (PlayerPrefs.GetInt("Zycie") == 0)
        {
            PlayerPrefs.SetInt("Zycie", 100);
        }
        if (PlayerPrefs.GetFloat("Predkosc_poruszania") == 0)
        {
            PlayerPrefs.SetFloat("Predkosc_poruszania", predkoscPoruszania);
        }
        if (PlayerPrefs.GetFloat("Czas_do_wystrzalu") == 0)
        {
            PlayerPrefs.SetFloat("Czas_do_wystrzalu", czasDoWystrzalu);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GenerowaniePlanszy.wygenerowana & Time.timeScale!=0)
        {
            czasOdWystrzalu += Time.deltaTime;
            klawiatura();
            myszka();
        }
    }
    private void klawiatura()
    {

        float ruchPrzodTyl = Input.GetAxis("Vertical") * predkoscPoruszania;
        if (Input.GetKeyDown("left shift"))
        {
            predkoscPoruszania += bieg2;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            predkoscPoruszania -= bieg2;
        }
        Vector3 ruch = new Vector3(0, 0, ruchPrzodTyl);
        if ((Input.GetKeyDown("space") |Input.GetMouseButtonDown(0)) & czasOdWystrzalu>=czasDoWystrzalu)
        {
            czasOdWystrzalu = 0;
            strzal();
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            transform.GetComponent<Rigidbody>().MovePosition((transform.position + (transform.forward * Time.deltaTime)*predkoscPoruszania));
        }
        else
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                transform.GetComponent<Rigidbody>().MovePosition((transform.position - (transform.forward * Time.deltaTime) * predkoscPoruszania));
            }

        }      
    }
    private void myszka()
    {
        //nowe
        if (Input.GetAxis("Horizontal") != 0)
        {
            this.transform.Rotate(0, Input.GetAxis("Horizontal") * 2, 0);
        }

        float obrot = Input.GetAxis("Mouse X")*czuloscMyszki;
        GameObject wieza = GameObject.Find("wieza");
        wieza.transform.Rotate(0, obrot, 0);
        
        //this.transform.rotation = Quaternion.Slerp(transform.rotation, wieza.transform.rotation, Time.deltaTime);
        //wieza.transform.rotation = Quaternion.Slerp(wieza.transform.rotation, transform.rotation, Time.deltaTime);
    }
    public void strzal()
    {
        GameObject gb = (GameObject)Instantiate(pocisk, lufa.transform.position, Camera.main.transform.rotation);
        gb.GetComponent<Rigidbody>().velocity = lufa.transform.forward * 20;
        gb.GetComponent<Pocisk>().obrazenia = PlayerPrefs.GetInt("Obrazenia"); ;
        gb.GetComponent<Pocisk>().ktowyslal = "Gracz";
    }
    public void powieszSzybkosc()
    {
        PlayerPrefs.SetFloat("Predkosc_poruszania", PlayerPrefs.GetFloat("Predkosc_poruszania")+1);
        PlayerPrefs.SetInt("Punkty",PlayerPrefs.GetInt("Punkty")-20);
    }
    public void powieszZycie()
    {
        PlayerPrefs.SetInt("Zycie", PlayerPrefs.GetInt("Zycie")+50);
        PlayerPrefs.SetInt("Punkty",PlayerPrefs.GetInt("Punkty")-20);
    }
    public void powieszObrazenia()
    {
        PlayerPrefs.SetInt("Obrazenia", PlayerPrefs.GetInt("Obrazenia")+ 10);
        PlayerPrefs.SetInt("Punkty",PlayerPrefs.GetInt("Punkty")-20);
    }
    public void powieszSzybkostrzelnosc()
    {
        PlayerPrefs.SetFloat("Czas_do_wystrzalu", PlayerPrefs.GetFloat("Czas_do_wystrzalu")-1);
        PlayerPrefs.SetInt("Punkty",PlayerPrefs.GetInt("Punkty")-20);
    }
}
