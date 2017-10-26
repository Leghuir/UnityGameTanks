using UnityEngine;
using System.Collections;

public class Przeciwnik : MonoBehaviour
{
    public float predkoscObrotu = 6.0f;
    public bool gladkiObrot = true;
    public float predkoscRuchu = 5.0f;
    public float zasiegWzroku = 30f;
    public float odstepOdGracza = 2f;
    public GameObject pocisk;
    public float szybkostrzelnosc=5f;
    private Transform mojObiekt;
    private Transform gracz;
    private Transform lufa;
    private bool patrzNaGracza = false;
    private Vector3 pozycjaGraczaXYZ;
    private float czasomierz;
    public int obrazenia= 10;
    public static int przeciwnicy_zyjacy;
    public static bool czyBylSpawnPrzeciwnika = false;

    // Use this for initialization
    void Start()
    {
        czyBylSpawnPrzeciwnika = true;
        ulepsz_przeciwnika();
        przeciwnicy_zyjacy++;
        czasomierz = 0;
        mojObiekt = transform;
        
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        czasomierz += Time.deltaTime;
        if (GameObject.FindWithTag("Player") != null)
        {
            gracz = GameObject.FindWithTag("Player").transform;
            strzelaj();
            idzWStroneGracza();
            patrzNaMnie();
        }
    }
    private void autodestrukcja()
    {
        if (transform.position.y < -20)
        {
            Przeciwnik.przeciwnicy_zyjacy--;
            Destroy(this);
        }

    }
    private void strzelaj()
    {
        if (pocisk != null & czasomierz > szybkostrzelnosc)
        {
            czasomierz = 0;
            bool pra=patrzyNaGracza();
        }
    }
    private bool patrzyNaGracza()
    {
        lufa = transform.FindChild("lufa");
        float dist = Vector3.Distance(mojObiekt.position, gracz.position);
        RaycastHit sprawdz = new RaycastHit();
        Physics.Raycast(lufa.transform.position, lufa.transform.forward, out sprawdz, dist+10,9);
        if (sprawdz.collider != null)
        {
            if (sprawdz.collider.name == "Czolg" & dist<7f)
            {
                GameObject rakieta=(GameObject)Instantiate(pocisk, lufa.transform.position-new Vector3(0,0.1f), Quaternion.LookRotation(gracz.transform.position));
                rakieta.GetComponent<Rigidbody>().velocity = lufa.transform.forward * 10;
                rakieta.GetComponent<Pocisk>().obrazenia = obrazenia;
                rakieta.GetComponent<Pocisk>().ktowyslal = "Przeciwnik";
            }
        }
        return false;
    }

    public void idzWStroneGracza()
    {
        pozycjaGraczaXYZ = new Vector3(gracz.position.x, mojObiekt.position.y, gracz.position.z);
        float dist = Vector3.Distance(mojObiekt.position, gracz.position);

        patrzNaGracza = false;

        if (dist <= zasiegWzroku && dist > odstepOdGracza)
        {
            patrzNaGracza = true;

            mojObiekt.position = Vector3.MoveTowards(mojObiekt.position, pozycjaGraczaXYZ, predkoscRuchu * Time.deltaTime);

        }
        else if (dist <= odstepOdGracza)
        {
            patrzNaGracza = true;
        }
    }
    void patrzNaMnie()
    {
        if (gladkiObrot && patrzNaGracza == true)
        {
            Quaternion rotation = Quaternion.LookRotation(pozycjaGraczaXYZ - mojObiekt.position);
            mojObiekt.rotation = Quaternion.Slerp(mojObiekt.rotation, rotation, Time.deltaTime * predkoscObrotu);
        }
        else if (!gladkiObrot && patrzNaGracza == true)
        {
            transform.LookAt(pozycjaGraczaXYZ);
        }

    }
    private void ulepsz_przeciwnika()
    {
        int lvl = PlayerPrefs.GetInt("Poziom");
        lvl -= 4;
        for(int i=0;i< lvl;i++)
        {
            obrazenia += 10;
            transform.GetComponent<Zdrowie>().powieksz_zycie(20);
            
        }
    }
}
