using UnityEngine;

namespace Tool
{
    public interface ITool
    {
        string ToolName { get; }
        string GroupName { get; }

        bool DrawGUI();
        void Execute();
    }

    public abstract class ToolAsset : ScriptableObject, ITool
    {
        [SerializeField] private string toolName;
        [SerializeField] private string groupName;

        public string ToolName => toolName;
        public string GroupName => groupName;
        
        public virtual bool DrawGUI() => false;
        public abstract void Execute();
    }
}