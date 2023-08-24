using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum PanelType
{
    None,
    Menu,
    Merge,
    Hunt,
    GameOver,
    LevelComplete
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private VirtualPanel[] _panels;
    [SerializeField] private float _fadeTime = 1f;

    private void OnEnable()
    {
        EventBus.OnMenuState += OpenMenu;
        EventBus.OnMergeState += OpenMerge;
        EventBus.OnHuntState += OpenHunt;
    }

    private void OnDisable()
    {
        EventBus.OnMenuState -= OpenMenu;
        EventBus.OnMergeState -= OpenMerge;
        EventBus.OnHuntState -= OpenHunt;
    }

    public void OpenPanel(PanelType type)
    {
        foreach (var panel in _panels)
            panel.gameObject.SetActive(panel.Type == type);
    }

    public async Task FadePanel(PanelType type, float endValue = 1)
    {
        OpenPanel(type);
        VirtualPanel targetPanel = null;

        foreach (var panel in _panels)
            if (panel.Type == type) targetPanel = panel;


        if (targetPanel == null) return;
        Image targetImage = targetPanel.GetComponent<Image>();
        if (targetImage == null) return;
        targetImage.DOFade(1 - endValue, 0).Play().Kill(true);
        await targetImage.DOFade(endValue, _fadeTime).Play().AsyncWaitForCompletion();
    }

    private void OpenMenu()
    {
        OpenPanel(PanelType.Menu);
    }

    private void OpenMerge()
    {
        OpenPanel(PanelType.Merge);
    }
    private void OpenHunt()
    {
        OpenPanel(PanelType.Hunt);
    }
}
