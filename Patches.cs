using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace CustomLanguages
{
    [HarmonyPatch(typeof(MainMenu), "Awake")]
    public static class MainMenuPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(MainMenu __instance)
        {
            foreach (var language in Core.Languages)
            {
                string displayName = Core.LanguageNames[language.Key];
                bool exists = false;

                for (int i = 0; i < __instance.OLHCOGFBIGC.Count; i++)
                {
                    var current = __instance.OLHCOGFBIGC[i];
                    if (current != null && current.name == displayName)
                    {
                        exists = true;

                        break;
                    }
                }

                if (exists) continue;

                __instance.OLHCOGFBIGC.Add(new MainMenu.language
                {
                    name = displayName,
                    icon = Core.GetIcon(language.Value)
                });
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(KDLPMCJBDEB), "NNNPLCHBBGH", [typeof(int), typeof(string)])]
    public static class TranslatorPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(int KJLIHNBGHEK, string GBPADMJAADG, ref string __result)
        {
            int languageId = PlayerPrefs.GetInt("Language");
            if (languageId > 0 && languageId < 10)
            {
                TextAsset asset = Resources.Load<TextAsset>($"{languageId}/{GBPADMJAADG}");
                if (asset == null)
                {
                    __result = string.Empty;

                    return false;
                }

                if (TryTranslate(asset.text,KJLIHNBGHEK, out __result)) return false;

                __result = string.Empty;

                return false;
            }

            if (languageId >= 10)
            {
                if (!Core.Languages.TryGetValue(languageId, out string directory))
                {
                    __result = string.Empty;

                    return false;
                }

                string path = Path.GetFullPath(Path.Combine(directory, "translate.txt"));
                string root = Path.GetFullPath(directory) + Path.DirectorySeparatorChar;

                if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                {
                    __result = string.Empty;

                    return false;
                }

                if (!File.Exists(path))
                {
                    __result = string.Empty;

                    return false;
                }

                try
                {
                    string text = File.ReadAllText(path);
                    if (TryTranslate(text, KJLIHNBGHEK, out __result)) return false;
                }
                catch (Exception ex)
                {
                    Core.Log($"Translation error: {ex.Message}");
                }

                __result = string.Empty;

                return false;
            }

            __result = string.Empty;

            return false;
        }

        private static bool TryTranslate(string text, int id, out string result)
        {
            result = string.Empty;

            if (string.IsNullOrEmpty(text) || id < 1) return false;

            string[] lines = text.Split('\n');

            if (id > lines.Length) return false;

            string line = lines[id - 1].TrimEnd('\r');
            string[] parts = line.Split(':');

            if (parts.Length > 3)
            {
                string normalized = 
                     parts[0] + ";" + 
                     parts[1] + ":" +
                     parts[2] + ";" + parts[3];

                parts = normalized.Split(':');
            }
            else if (parts.Length > 2)
            {
                string normalized =
                    parts[0] + ":" + 
                    parts[1] + ";" + 
                    parts[2];

                parts = normalized.Split(':');
            }

            if (parts.Length < 2) return false;

            result = parts[1].Replace("#", string.Empty);

            return true;
        }
    }
}
