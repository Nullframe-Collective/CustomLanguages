# CustomLanguages

**CustomLanguages** is a [MelonLoader](https://melonwiki.xyz/) mod that allows players to add custom languages.

**Required version of MelonLoader: 0.7.3.**

## Creating a Custom Language

Each custom language must have its own directory inside `UserData/CustomLanguages`.

Example structure:

```text
UserData/
└── CustomLanguages/
    └── MyLanguage/
        ├── translate.txt
        ├── language.txt
        └── icon.png
```

### `translate.txt`

<img width="250" height="450" alt="image" src="https://github.com/user-attachments/assets/8de8b9f1-6ed7-4975-853a-453695e1774c" />

Contains the translation strings used by the game. Each line corresponds to a translation ID, starting at 1.

The expected format is:

```text
1:Translated text
2:Another translated string
3:Yet another translated string
```

Keep the lines in the correct order so that translation IDs match the game's text entries.

### `language.txt`

<img width="300" height="300" alt="image" src="https://github.com/user-attachments/assets/ede931dd-1f8a-4501-9bf2-20e6e24d8808" />

An optional text file containing the display name of your language.

For example:

```text
My Custom Language
```

If this file is missing or contains only whitespace, the directory name is used instead.

### `icon.png`

<img width="143" height="135" alt="image" src="https://github.com/user-attachments/assets/7db01142-9d59-4a35-9844-d9a3d9a7b698" />

An optional PNG image used as the language's icon in the language selection menu.

