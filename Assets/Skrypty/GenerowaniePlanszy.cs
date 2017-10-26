using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GenerowaniePlanszy : MonoBehaviour
{
    private int rozmiarPlanszy;
    private string nazwaPlanszy;
    private bool[,] tablicaKostek;
    private Object[] kostki;
    public GameObject przeciwnik;
    public static bool wygenerowana;
    public GameObject gracz;
    void Start()
    {
        Przeciwnik.czyBylSpawnPrzeciwnika = false;
        rozmiarPlanszy = PlayerPrefs.GetInt("RozmiarPlanszy");
        Debug.Log(rozmiarPlanszy);
        PlayerPrefs.SetInt("Poziom", PlayerPrefs.GetInt("Poziom") + 1);
        GenerowaniePlanszy.wygenerowana = false;

        Camera.main.transform.localPosition += new Vector3(0, rozmiarPlanszy, 0);

        nazwaPlanszy = gameObject.name;
        tablicaKostek = new bool[rozmiarPlanszy - 1, rozmiarPlanszy - 1];
        kostki = Resources.LoadAll("Kostki");
        czyscTablice(tablicaKostek);
        generujTablice(tablicaKostek, tablicaKostek.GetLength(0));
        StartCoroutine(generujPlansze(nazwaPlanszy));
    }

    void Update()
    {
        if (Przeciwnik.przeciwnicy_zyjacy <= 0 && Przeciwnik.czyBylSpawnPrzeciwnika)
        {
            if (PlayerPrefs.GetInt("RozmiarPlanszy") < 50)
            {
                PlayerPrefs.SetInt("RozmiarPlanszy", PlayerPrefs.GetInt("RozmiarPlanszy") + 10);
                SceneManager.LoadScene("Plansza");
            }
        }
    }

    private void czyscTablice(bool[,] tablica)
    {
        for (int i = 0; i < tablica.GetLength(0); i++)
        {
            for(int j = 0; j < tablica.GetLength(1); j++)
            {
                tablica[i, j] = false;
            }
        }
    }

    private void generujTablice(bool[,] tablica, int maxKostek)
    {
        int iloscKostek = 0;

        while (iloscKostek < maxKostek)
        {
            int wiersz = Random.Range(0, tablicaKostek.GetLength(0));
            int kolumna = Random.Range(0, tablicaKostek.GetLength(1));
            if (!tablica[wiersz, kolumna] && !sprawdzSasiadow(tablica, wiersz, kolumna))
            {
                tablica[wiersz, kolumna] = true;
                iloscKostek++;
            }
        }

        // pokazTablice(tablica);
    }

    private IEnumerator generujPlansze(string rodzic)
    {
        GameObject podloga = GameObject.Find("Podloga");
        GameObject scianaGorna = GameObject.Find("ScianaGorna");
        GameObject scianaDolna = GameObject.Find("ScianaDolna");
        GameObject scianaLewa = GameObject.Find("ScianaLewa");
        GameObject scianaPrawa = GameObject.Find("ScianaPrawa");
        float powiekszenie = rozmiarPlanszy / 50f;

        do
        {
            // podłoga
            podloga.transform.localScale += new Vector3(powiekszenie, 0, powiekszenie);

            // ściana górna
            scianaGorna.transform.localScale += new Vector3(powiekszenie, 0, 0);
            scianaGorna.transform.localPosition += new Vector3(0, 0, powiekszenie / 2f);

            // ściana dolna
            scianaDolna.transform.localScale += new Vector3(powiekszenie, 0, 0);
            scianaDolna.transform.localPosition += new Vector3(0, 0, -powiekszenie / 2f);

            // ściana lewa
            scianaLewa.transform.localScale += new Vector3(0, 0, powiekszenie);
            scianaLewa.transform.localPosition += new Vector3(powiekszenie / 2f, 0, 0);

            // ściana prawa
            scianaPrawa.transform.localScale += new Vector3(0, 0, powiekszenie);
            scianaPrawa.transform.localPosition += new Vector3(-powiekszenie / 2f, 0, 0);

            yield return new WaitForSeconds(0.01f);
        }
        while (podloga.transform.lossyScale.x < rozmiarPlanszy);

        StartCoroutine(generujKostki(tablicaKostek, nazwaPlanszy));

        
    }
    private IEnumerator stworzenie_gracza()
    {
        GameObject czolg = (GameObject)Instantiate(gracz, wolneMiejsce(0), new Quaternion());
        czolg.name = "Czolg";
        Camera.main.transform.parent = czolg.transform;
        for (int i = 0; i < 250; i++)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, czolg.transform.position + new Vector3(0, 3.5f, -3f), 0.7f * Time.deltaTime);
            Camera.main.transform.LookAt(czolg.transform);
            yield return new WaitForSeconds(0.05f);
        }
        Camera.main.transform.position = czolg.transform.position + new Vector3(0f, 2.5f, -3f);
        Camera.main.transform.LookAt(czolg.transform);
        GenerowaniePlanszy.wygenerowana = true;
        stworz_przeciwnika();
    }
    private void stworz_przeciwnika()
    {
        int pom = rozmiarPlanszy / 10;
        for(int i=0;i< pom;i++)
        Instantiate(przeciwnik,wolneMiejsce(0),Quaternion.identity);
        Przeciwnik.czyBylSpawnPrzeciwnika = true;
    }
    private IEnumerator generujKostki(bool[,] tablica, string rodzic)
    {
        GameObject pojemnik = new GameObject("Kostki");
        pojemnik.transform.parent = GameObject.Find(rodzic).transform;
        int losowa = 0;

        for(int i = 0; i < tablica.GetLength(0); i++)
        {
            for(int j = 0; j < tablica.GetLength(1); j++)
            {
                if (tablica[i, j])
                {
                    losowa = Random.Range(0, kostki.Length);
                    switch(losowa)
                    {
                        case 1: // przesuwająca się
                            Instantiate(kostki[losowa], new Vector3(0.5f + j - (tablica.GetLength(1) / 2f), 10.25f, 0.5f + i - (tablica.GetLength(0) / 2f)), Quaternion.identity, pojemnik.transform);
                            break;

                        default:
                            Instantiate(kostki[losowa], new Vector3(0.5f + j - (tablica.GetLength(1) / 2f), -10.25f, 0.5f + i - (tablica.GetLength(0) / 2f)), new Quaternion(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180), 0), pojemnik.transform);
                            break;
                    }
                    
                }
            }
        }

        float szybkosc = (rozmiarPlanszy / 50f) * 1.5f;
        Component[] kostkiWGrze = pojemnik.GetComponentsInChildren<Component>();
        foreach(Component k in kostkiWGrze)
        {
            while(k.transform.position.y < 0.25f)
            {
                k.transform.localPosition += new Vector3(0, szybkosc, 0);
                yield return new WaitForSeconds(0.005f);
            }
        }
        StartCoroutine(stworzenie_gracza());
    }

    private bool sprawdzSasiadow(bool[,] tablica, int wiersz, int kolumna)
    {
        bool tymczasowa = false;

        for(int i = wiersz - 1; i <= wiersz + 1; i++)
        {
            if (i > -1 && i < tablica.GetLength(0))
            {
                for (int j = kolumna - 1; j <= kolumna + 1; j++)
                {
                    if (j > -1 && j < tablica.GetLength(1))
                    {
                        if(tablica[i, j])
                        {
                            tymczasowa = true;
                            break;
                        }
                    }
                }
            }
        }

        return tymczasowa;
    }

    private void pokazTablice(bool[,] tablica)
    {
        string tymczasowy = "";
        for(int i = 0; i < tablica.GetLength(0); i++)
        {
            tymczasowy = "";
            for(int j = 0; j < tablica.GetLength(1); j++)
            {
                tymczasowy += System.Convert.ToInt32(tablica[i, j]) + "   ";
            }
            Debug.Log(tymczasowy);
        }
    }

    public Vector3 wolneMiejsce(float wysokosc)
    {
        int wiersz = 0;
        int kolumna = 0;

        do
        {
            wiersz = Random.Range(0, tablicaKostek.GetLength(0));
            kolumna = Random.Range(0, tablicaKostek.GetLength(1));
        }
        while (sprawdzSasiadow(tablicaKostek, wiersz, kolumna));

        // Debug.Log("Czołg: " + wiersz + ", " + kolumna);
        tablicaKostek[wiersz, kolumna] = true;

        return new Vector3(0.5f + kolumna - (tablicaKostek.GetLength(1) / 2f), wysokosc, 0.5f + wiersz - (tablicaKostek.GetLength(0) / 2f));
    }

    public void funkcja()
    {
        SceneManager.LoadScene("Poczatkowa");
    }
}
