using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPauseMenu() 
    {
        pauseMenu.SetActive(true);
    }

    public void QuitPauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
