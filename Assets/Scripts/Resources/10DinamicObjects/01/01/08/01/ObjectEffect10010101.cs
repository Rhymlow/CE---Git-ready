using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectEffect10010101 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PassDay(72f, 5f));
    }


    IEnumerator PassDay(float speed, float duration)
    {
        GameObject sun = GameObject.Find("DayNightCycle");
        GameObject sun1 = GameObject.Find("DayNightCycle1");
        GameSystem.EnablePlayerMovement(false);
        float time = 0f;
        while (time < duration)
        {
            sun.transform.Rotate(Vector3.right * speed * Time.deltaTime, Space.Self);
            sun1.transform.Rotate(Vector3.right * speed * Time.deltaTime, Space.Self);
            time += Time.deltaTime;
            yield return null;
        }
        GameSystem.EnablePlayerMovement(true);
        sun.transform.eulerAngles = new Vector3(9.0f, 9.81f, 0.0f);
        sun1.transform.eulerAngles = new Vector3(100.0f, 0.0f, 0.0f);
        Destroy(this.transform.gameObject);
    }
}