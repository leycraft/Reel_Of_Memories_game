using UnityEngine;

public class CutsceneLoader : MonoBehaviour
{
    [Header("Cutscene File Name (without .json)")]
    public string cutsceneFileName = "first_cutscene"; // ตั้งชื่อไฟล์ JSON ที่จะโหลด

    [Header("Reference to Cutscene Manager")]
    public CutsceneManager cutsceneManager; // เชื่อมกับตัวจัดการ cutscene หลัก

    void Start()
    {
        //LoadAndPlayCutscene();
    }

    public void LoadAndPlayCutscene(string JSONName)
    {
        cutsceneFileName = JSONName;
        // โหลดไฟล์จาก Resources/Cutscenes/
        TextAsset jsonFile = Resources.Load<TextAsset>("Cutscenes/" + cutsceneFileName);

        if (jsonFile == null)
        {
            Debug.LogError("❌ ไม่พบไฟล์ Cutscene: " + cutsceneFileName);
            return;
        }

        // แปลงข้อความ JSON → ข้อมูลในเกม
        CutsceneData data = JsonUtility.FromJson<CutsceneData>(jsonFile.text);

        if (data == null || data.lines == null || data.lines.Length == 0)
        {
            Debug.LogError("⚠️ ข้อมูลใน JSON ว่างหรือไม่ถูกต้อง: " + cutsceneFileName);
            return;
        }

        // ส่งข้อมูลเข้า CutsceneManager
        cutsceneManager.LoadCutsceneData(data);
    }
}
