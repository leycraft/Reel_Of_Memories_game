using UnityEngine;

public class CutsceneStater : MonoBehaviour
{
    public CutsceneLoader CutsceneLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadCutScene(string JSONName)
    {
        CutsceneLoader.gameObject.SetActive(true);
        CutsceneLoader.LoadAndPlayCutscene(JSONName);
    }
}
