using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour {


    [SerializeField] private int health;

    [SerializeField] private int fallDeathHeight = -10;
    [SerializeField] private bool hasDied = false;

	// Update is called once per frame
	void FixedUpdate()
    {
        if (gameObject.transform.position.y < fallDeathHeight)
        {
            hasDied = true;
        }

        if (hasDied == true)
        {
            StartCoroutine ("Die");
        }
	}

    IEnumerator Die() // Need to further research what an IEnumerator is.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }
}
