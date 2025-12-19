# 🎭 AI-Powered NPC Dialogue Tool

![Unity](https://img.shields.io/badge/Unity-6000.x-000000?style=flat&logo=unity)
![C#](https://img.shields.io/badge/Language-C%23-239120?style=flat&logo=c-sharp)
![License](https://img.shields.io/badge/License-MIT-blue)

A professional **Unity Editor Tool** designed to streamline the creation of dynamic NPC dialogues. This tool simulates AI-driven text generation using asynchronous calls (`async/await`) to prevent editor freezing and utilizes **JSON Serialization** to save generated data for in-game implementation.

## 📸 Preview

!<img width="755" height="771" alt="image" src="https://github.com/user-attachments/assets/2d3e5d53-4adf-4f7d-a4be-51366af3df73" />

)
*(This tool allows developers to generate and save dialogues without leaving the Unity Editor)*

## 🌟 Key Features

* **Custom Editor Window:** Built entirely using Unity's `EditorWindow` API for a native look and feel.
* **Asynchronous Execution:** Uses C# `Task` and `async/await` patterns to handle generation delays without locking the main thread.
* **Diverse Personas:** Includes a mock database with **6 distinct characters** (e.g., *Wretched Beggar, Goblin Scout, Mysterious Wizard*) and over **120 unique lines**.
* **JSON Save System:** Successfully generated dialogues can be serialized and exported to `.json` files with a single click.
* **UX/UI Design:** Features conditional buttons, loading states, and user feedback loops.

## 🛠 Technical Implementation

### 1. Editor Scripting & GUI
The tool extends `EditorWindow` to create a custom interface within Unity.

```csharp
[MenuItem("My AI Tools/Dialogue Generator")]
public static void ShowWindow()
{
    GetWindow<DialogueWindow>("AI Dialogue Tool");
}
```
### 2. Async/Await Pattern (No Coroutines)
Instead of legacy Coroutines, modern Task based asynchronous programming is used to simulate API latency.
```
async void GenerateMockDialogue()
{
    isGenerating = true;
    await Task.Delay(1000); // Non-blocking delay
    // ... logic ...
    isGenerating = false;
    Repaint();
}
```
### 3. Data Serialization
The tool converts runtime objects into persistent data using JsonUtility.
```
{
    "characterName": "Wretched Beggar",
    "dialogueText": "[Wretched Beggar]: Alms for the poor...",
    "dateCreated": "12/19/2025"
}
```
🚀 How to Install
Clone this repository.

Open the project in Unity 6 (6000.x) or later.
Navigate to the top menu: My AI Tools -> Dialogue Generator.

👨‍💻 Author
Developed as a demonstration of Tool Programming, Editor Scripting, and System Architecture in Unity.
