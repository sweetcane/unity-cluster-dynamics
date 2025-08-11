using System.Collections;
using Core.Particle;
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
        [SerializeField] private int seed = 0;
        [SerializeField] private string seedStr = "";

        private void OnValidate()
        {
            count = Mathf.Clamp(count, minCount, maxCount);
            countStr = count.ToString();
        }

        public override bool DrawGUI()
        {
            bool changed = false;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Particle Count");
            string newCountStr = GUILayout.TextField(countStr, 6, GUILayout.MinWidth(36));

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

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Random Seed");
            string newSeedStr = GUILayout.TextField(seedStr, 6, GUILayout.MinWidth(36));

            if (newSeedStr != seedStr)
            {
                seedStr = newSeedStr;
                if (int.TryParse(newSeedStr, out var newSeed))
                {
                    if (newSeed != seed)
                    {
                        seed = newSeed;
                        changed = true;
                    }
                }
            }

            GUILayout.EndHorizontal();

            return changed;
        }

        public override IEnumerator Execute()
        {
            ParticleManager.Instance.Clear();
            System.Random random = new System.Random(seed);

            for (int i = 0; i < count; i++)
            {
                float x = (float)(random.NextDouble() * 2.0 - 1.0);
                float y = (float)(random.NextDouble() * 2.0 - 1.0);
                float z = (float)(random.NextDouble() * 2.0 - 1.0);

                var pos = new Vector3(x, y, z);
                ParticleManager.Instance.Generate(pos);
            }
            yield return null;
        }
    }
}