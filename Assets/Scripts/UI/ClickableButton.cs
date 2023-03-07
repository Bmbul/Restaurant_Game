using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableButton : Button
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => AudioManager.Instance.OnButtonClick());
    }
}
