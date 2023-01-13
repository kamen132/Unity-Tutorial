using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;
using View.EnumView;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private UISettings defaultUISettings;
    public UIFrame uiFrame;
    void Start()
    {
        //uiFrame.rui("YourScreenId", guideMask);
        uiFrame = defaultUISettings.CreateUIInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            uiFrame.ShowPanel(UIWindowId.GuideMask);
        }
    }
}
