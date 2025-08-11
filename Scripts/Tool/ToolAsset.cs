using System.Collections;
using UnityEngine;

namespace Tool
{
    public abstract class ToolAsset : ScriptableObject
    {
        [SerializeField] private string toolName;
        [SerializeField] private string groupName;

        public string ToolName => toolName;
        public string GroupName => groupName;
        
        public virtual bool DrawGUI() => false;
        public abstract IEnumerator Execute();
    }
}