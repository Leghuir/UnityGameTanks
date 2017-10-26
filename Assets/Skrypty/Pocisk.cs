using UnityEngine;
using System.Collections;

public class Pocisk : MonoBehaviour {
    private float czasomierz;
    public float samozniszczenie=3f;
    public int obrazenia = 0;
    public string ktowyslal;
	// Use this for initialization
	void Start () {
        czasomierz = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        czasomierz += Time.deltaTime;
        if (czasomierz >= samozniszczenie)
        {
            autodestrukcja();
        }
    }
    private void autodestrukcja()
    {
            DestroyObject(transform.gameObject);
        
    }
    public void OnTriggerEnter(Collider col)
    {
        
        if(( col.name == "Czolg" & ktowyslal=="Przeciwnik") | (col.tag=="Przeciwnik" & ktowyslal=="Gracz"))
        {
            col.transform.GetComponent<Zdrowie>().zadajObrazenia(obrazenia);
        }
        if (!(( col.name == "Czolg" & ktowyslal == "Gracz") | (col.tag == "Przeciwnik" & ktowyslal == "Przeciwnik")))
        {
            autodestrukcja();
        }
        if (col.tag == "kostka_czerwona")
        {
            col.gameObject.GetComponent<Kostka>().autodestrukcja();
        }

    }
}
