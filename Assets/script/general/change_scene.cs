using UnityEngine;
using UnityEngine.SceneManagement;

public class change_scene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void change_to_scene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
