using UnityEngine;

namespace Tool.Tools
{
    [CreateAssetMenu(fileName = "Particle Generate Tool", menuName = "Tools/Particle Generate")]
    public class ParticleGenerateTool : ToolAsset
    {
        [SerializeField] private int count = 1000;
        [SerializeField] private string countStr = "1000";
        [SerializeField] private int minCount = 1;
        [SerializeField] private int maxCount = 1_000_000;       

        private void OnValidate()
        {
            count = Mathf.Clamp(count, minCount, maxCount);
            countStr = count.ToString();
        }

        public override bool DrawGUI()
        {
            bool changed = false;

            GUILayout.Label("Particle Count");
            string newCountStr = GUILayout.TextField(countStr, 6, GUILayout.MinWidth(70));

            if (newCountStr != countStr)
            {
                countStr = newCountStr;
                if (int.TryParse(newCountStr, out var newCount))
                {
                    newCount = Mathf.Clamp(newCount, minCount, maxCount);
                    if (newCount != count)
                    {
                        count = newCount;
                        changed = true;
                    }
                }
            }

            return changed;
        }

        public override void Execute()
        {
            Debug.Log($"[ParticleGenerateTool] Particle Generated: {count}");
        }
    }
}