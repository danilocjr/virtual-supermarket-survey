using System;
using UnityEngine;

public enum NavCommand
{
    center,
    forward,
    backward,
    left,
    right,
    lookRight,
    lookLeft,
    stand,
    normal,
    crouch,
    none
}

public class NavController : MonoBehaviour
{
    private MDXButton[] navButtons;

    public delegate void NavAction(NavCommand navCommand);
    public static event NavAction OnNavCommandClicked;


    void Start()
    {
        navButtons = GetComponentsInChildren<MDXButton>();

        foreach (MDXButton nb in navButtons)
        {
            NavCommand _navCommand = (NavCommand)Enum.Parse(typeof(NavCommand), nb.name, true);
            nb.OnPressed(() => ClickCommand(_navCommand));
            nb.OnReleased(() => ClickCommand(NavCommand.none));

            // Initial State
            if (nb.name.ToLower() == "normal")
                nb.Select();
        }

    }

    private void ClickCommand(NavCommand navCommand)
    {
        OnNavCommandClicked?.Invoke(navCommand);
    }

   
}
