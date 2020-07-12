using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetLevelTextBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshPro>().text = $"Level {SceneManager.GetActiveScene().buildIndex}";
    }
}
