using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "toolInfo", menuName = "toolInfo")]
public class ToolInfo : ScriptableObject
{
    public string myName;

    [TextArea]
    public string myDescription;

    public string functionName;
}
