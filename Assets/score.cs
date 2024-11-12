using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private bool isScored;
    private GameObject corbeille;

    // Start is called before the first frame update
    void Start()
    {
        isScored = false;

        corbeille = GameObject.Find("Corbeille");
        if (corbeille == null)
        {
            Debug.LogError("Corbeille non trouvée dans la scène !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isScored /*condition*/)
        {
            isScored = false;
            corbeille.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
}
