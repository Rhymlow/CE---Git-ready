using UnityEngine;

public class BRBRBRY : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CastSpell();
    }

    void CastSpell()
    {
        GameSystem.LoadGame();
        Destroy(this.transform.gameObject);
    }
}
