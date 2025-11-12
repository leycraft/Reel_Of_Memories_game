using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public string dialogueText;
        public Sprite background;
        public string character;
        public AudioClip bgm;
        public bool isNarration;
    }

    public DialogueLine[] lines;
    public Image backgroundImage;
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI narrativeText;
    public Button skipButton;
    public TextMeshProUGUI tapToContinueText;
    public AudioSource bgmSource;
    public CanvasGroup dialogueGroup;
    public Image fadePanel;
    public Image narrativeBackground;
    public GameObject cutsceneParent;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine bgmFadeCoroutine;

    void Start()
    {
        narrativeBackground.gameObject.SetActive(false);
        fadePanel.raycastTarget = false;
        // 👇 ปิดกล่อง Dialogue ทั้งชุดตั้งแต่เริ่ม
        dialogueGroup.alpha = 0f;
        dialogueGroup.gameObject.SetActive(false);
        dialogueGroup.interactable = false;
        dialogueGroup.blocksRaycasts = false;

        // 👇 ปิด Narrative text ด้วย (กันแว็บ)
        Color nc = narrativeText.color;
        nc.a = 0f;
        narrativeText.color = nc;
        narrativeText.gameObject.SetActive(false);

        Color bgColor = backgroundImage.color;
        bgColor.a = 1f;
        backgroundImage.color = bgColor;

        StartCoroutine(FadeScene(true, 1.0f));

        // เริ่มโหลด cutscene
        //skipButton.onClick.AddListener(SkipCutscene);

        if (lines != null && lines.Length > 0)
            ShowLine(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // 👇 ถ้ากำลังพิมพ์อยู่ ให้ข้ามไปเลย
                skipTyping = true;
            }
            else
            {
                // 👇 ถ้าพิมพ์เสร็จแล้ว ค่อยไปบรรทัดต่อไป
                NextLine();
            }
        }
    }


    IEnumerator FadeText(TextMeshProUGUI text, float targetAlpha, float duration)
    {
        float startAlpha = text.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            Color c = text.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            text.color = c;
            yield return null;
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, bool fadeIn, float duration)
    {
        float startAlpha = group.alpha;
        float targetAlpha = fadeIn ? 1f : 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        group.alpha = targetAlpha;
        group.interactable = fadeIn;
        group.blocksRaycasts = fadeIn;

        // ปิด GameObject หลัง fade out
        if (!fadeIn)
            group.gameObject.SetActive(false);
    }

    IEnumerator FadeScene(bool fadeIn, float duration)
    {
        Color c = fadePanel.color;
        float start = fadeIn ? 1f : 0f;
        float end = fadeIn ? 0f : 1f;

        c.a = start;
        fadePanel.color = c;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            c.a = Mathf.Lerp(start, end, t);
            fadePanel.color = c;
            yield return null;
        }

        c.a = end;
        fadePanel.color = c;

    }

    IEnumerator CrossfadeBGM(AudioClip newClip, float fadeDuration = 1.5f)
    {
        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(FadeBGM(newClip, fadeDuration));
        yield return bgmFadeCoroutine;
    }

    IEnumerator FadeBGM(AudioClip newClip, float fadeDuration)
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        // ถ้ามีเพลงเดิม → ค่อยๆ เบาเสียงลง
        while (time < fadeDuration / 2f)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / (fadeDuration / 2f));
            yield return null;
        }

        // เปลี่ยนเพลง
        bgmSource.Stop();
        bgmSource.clip = newClip;

        if (newClip != null)
        {
            bgmSource.Play();
        }

        // ค่อยๆ เพิ่มเสียงกลับขึ้น
        time = 0f;
        while (time < fadeDuration / 2f)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, 1f, time / (fadeDuration / 2f));
            yield return null;
        }

        bgmSource.volume = 1f;
    }


    void ShowLine(int index)
    {
        if (index >= lines.Length)
        {
            EndCutscene();
            return;
        }

        var line = lines[index];

        backgroundImage.sprite = line.background;
        if (!string.IsNullOrEmpty(line.character))
        {
            Sprite newChar = Resources.Load<Sprite>("characters/" + line.character);
            if (newChar != null)
            {
                characterImage.sprite = newChar;
                characterImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Character sprite not found: " + line.character);
                characterImage.gameObject.SetActive(false);
            }
        }
        else
        {
            characterImage.sprite = null;
            characterImage.gameObject.SetActive(false);
        }

        if (line.bgm != bgmSource.clip)
        {
            StartCoroutine(CrossfadeBGM(line.bgm));
        }

        StopCoroutine(nameof(TypeDialogue));
        StopCoroutine(nameof(ShowNarrative));
        StopCoroutine(nameof(FadeText));
        StopCoroutine(nameof(FadeCanvasGroup));

        if (line.isNarration)
        {

            // Fade out กล่อง Dialogue ทั้งชุด
            StartCoroutine(FadeCanvasGroup(dialogueGroup, false, 0.3f));

            // เปิด Narrative
            nameText.gameObject.SetActive(false);
            narrativeBackground.gameObject.SetActive(true);
            narrativeText.gameObject.SetActive(true);
            StartCoroutine(ShowNarrative(line.dialogueText));
        }
        else
        {

            // ปิด Narrative ป้องกันแว็บ
            narrativeText.text = "";
            narrativeText.alpha = 0f;
            narrativeBackground.gameObject.SetActive(false);
            narrativeText.gameObject.SetActive(false);

            // เปิด Dialogue ทั้งกล่องพร้อม fade in
            dialogueGroup.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(dialogueGroup, true, 0.3f));

            nameText.gameObject.SetActive(true);
            nameText.text = line.speakerName;
            StartCoroutine(TypeDialogue(line.dialogueText));
        }
    }



    IEnumerator TypeDialogue(string text)
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        foreach (char c in text)
        {
            // ถ้ามีการคลิกระหว่างพิมพ์ → ข้ามทันที
            if (skipTyping)
            {
                dialogueText.text = text;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }


    IEnumerator ShowNarrative(string text)
    {
        isTyping = true;

        yield return StartCoroutine(FadeText(narrativeText, 0f, 0.4f));
        // เคลียร์ข้อความเก่า ป้องกันแวบ
        narrativeText.text = "";
        Color nc = narrativeText.color;
        nc.a = 0f;
        narrativeText.color = nc;

        // fade in ข้อความใหม่
        narrativeText.text = text;
        yield return StartCoroutine(FadeText(narrativeText, 1f, 0.6f));

        isTyping = false;
    }


    void NextLine()
    {
        currentLine++;
        ShowLine(currentLine);
    }

    public void SkipCutscene()
    {
        EndCutscene();
    }

    void EndCutscene()
    {
        Debug.Log("Cutscene finished — ready to return to main game");
        StartCoroutine(CrossfadeBGM(null, 2f));
        StartCoroutine(FadeScene(false, 1.0f));
        //fadePanel.raycastTarget = true;
        cutsceneParent.SetActive(false);
        // จากนั้นถ้าอยากให้เปลี่ยน scene หลัง fade เสร็จ
        //StartCoroutine(ExitToGame());
    }

    // ถ้าอยากให้ตัด scene หลัง fade out
    IEnumerator ExitToGame()
    {
        yield return new WaitForSeconds(1.5f);
        // SceneManager.LoadScene("MainGameScene");
    }

    public void LoadCutsceneData(CutsceneData data)
    {
        lines = new DialogueLine[data.lines.Length];

        for (int i = 0; i < data.lines.Length; i++)
        {
            lines[i] = new DialogueLine
            {
                speakerName = data.lines[i].speakerName,
                dialogueText = data.lines[i].dialogueText,
                background = Resources.Load<Sprite>("backgrounds/" + data.lines[i].background),
                character = string.IsNullOrEmpty(data.lines[i].character) ? null : data.lines[i].character,
                bgm = string.IsNullOrEmpty(data.lines[i].bgm) ? null : Resources.Load<AudioClip>("bgm/" + data.lines[i].bgm),
                isNarration = data.lines[i].isNarration
            };
        }

        currentLine = 0;
        ShowLine(0);
    }

}
