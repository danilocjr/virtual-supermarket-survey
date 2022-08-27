using UnityEngine;
using UnityEngine.UI;

public class ViewProductBehaviour : MonoBehaviour
{
    [Header("ViewerPanel Components")]
    [SerializeField] GameObject viewerPanel;
   
    void Start()
    {
        ShowProductViewerPanel(false);
    }

    public void ShowProductViewerPanel(bool enable)
    {
        viewerPanel.SetActive(enable);
    }
   
}
