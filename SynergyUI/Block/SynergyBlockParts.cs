using KHJ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyBlockParts : MonoBehaviour
{
    public RectTransform rectTrm => transform as RectTransform;
    [field: SerializeField] public Image image { get; private set; }
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private Image outLine;
    [SerializeField] private Canvas canva;
    public Vector2Int girdPos { get; set; }
    private SynergyBlock parent;

    public void Initialize(SynergyBlock parent, int grade)
    {
        this.parent = parent;
        gradeText.text = grade.ToString();
        OnEnableGradeAndOutLine(false);
    }

    public void OnPointerDown(SynergySlotUI slot)
    {
        parent.PointerDown(this, slot);
    }

    public void OnPointerUp(SynergySlotUI slot)
    {
        parent.PointerUp(this, slot);
    }

    public void OnEnableAllImg(bool isEnable)
    {
        image.enabled = isEnable;
        foreach (Transform trm in transform)
        {
            if (trm.TryGetComponent(out Image img))
                img.enabled = isEnable;
        }
    }

    public void OnEnableGradeAndOutLine(bool v)
    {
        int value = v ? 1 : 0;
        canva.overrideSorting = v;
        outLine.enabled = v;
        gradeText.enabled = v;
    }
}
