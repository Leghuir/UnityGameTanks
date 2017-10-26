using UnityEngine;
using System.Collections;

public class Kostka : MonoBehaviour {

    public void autodestrukcja()
    {
        StartCoroutine(destr());
    }
private IEnumerator destr()
    {
        while (transform.position.y > -2)
        {
            transform.position -= new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this);
    }
}
