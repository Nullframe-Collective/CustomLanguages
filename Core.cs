using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;

[assembly: MelonInfo(typeof(CustomLanguages.Core), "CustomLanguages", "1.0.0", "Nullframe Collective", null)]
[assembly: MelonGame("ZeoWorks", "Slendytubbies 3")]
[assembly: MelonColor(8, 39, 80, 245)]

namespace CustomLanguages
{
    public class Core : MelonMod
    {
        public static readonly string Root = Path.Combine
        (
            MelonEnvironment.UserDataDirectory,
            "CustomLanguages"
        );

        public static readonly Dictionary<int, string> Languages = [];
        public static readonly Dictionary<int, string> LanguageNames = [];

        //private static readonly Dictionary<string, Sprite> Icons = new(StringComparer.OrdinalIgnoreCase);

        private static bool _initialized;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
            Initialize();
        }

        public static void Log(object message)
        {
            MelonLogger.Msg(message);
        }

        public static void Initialize()
        {
            if (_initialized) return;

            _initialized = true;

            Directory.CreateDirectory(Root);

            Languages.Clear();
            LanguageNames.Clear();

            int nextId = 10;

            foreach (string directory in Directory.GetDirectories(Root)
                         .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
            {
                string translatePath = Path.Combine(directory, "translate.txt");
                if (!File.Exists(translatePath)) continue;

                string folderName = Path.GetFileName(directory);
                string namePath = Path.Combine(directory, "language.txt");
                string displayName = File.Exists(namePath) ?
                    File.ReadAllText(namePath).Trim() :
                    folderName;

                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = folderName;
                }

                Languages[nextId] = directory;
                LanguageNames[nextId] = displayName;

                MelonLogger.Msg($"Registered language: {displayName}");

                nextId++;
            }
        }

        public static Sprite GetIcon(string directory)
        {
            string path = Path.Combine(directory, "icon.png");
            //if (Icons.TryGetValue(path, out Sprite cached)) return cached;

            Sprite sprite = LoadSprite(path);
            //Icons[path] = sprite;

            return sprite;
        }

        private static Sprite LoadSprite(string path)
        {
            if (!File.Exists(path)) return null;

            Texture2D texture = null;

            try
            {
                byte[] bytes = File.ReadAllBytes(path);

                texture = new Texture2D(2, 2);
                if (!ImageConversion.LoadImage(texture, bytes))
                {
                    UnityEngine.Object.Destroy(texture);

                    return null;
                }

                return Sprite.Create
                (
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );
            }
            catch (Exception ex)
            {
                if (texture != null)
                {
                    UnityEngine.Object.Destroy(texture);
                }

                MelonLogger.Msg($"Could not load icon '{path}': {ex.Message}");

                return null;
            }
        }
    }
}