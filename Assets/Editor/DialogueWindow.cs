using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO; // Required for file operations (Saving JSON)

public class DialogueWindow : EditorWindow
{
    // --- VARIABLES ---
    int selectedCharacterIndex = 0;
    
    // Character Options (Dropdown List)
    string[] characterOptions = new string[] { 
        "Village Elder", 
        "City Guard", 
        "Mysterious Wizard", 
        "Wretched Beggar", 
        "Tavern Keeper",   
        "Goblin Scout"     
    };

    string playerInput = "Hello, do you have any information?";
    string generatedResponse = ""; // The text shown in the UI
    bool isGenerating = false;

    // Database to hold all dialogue lines
    Dictionary<string, string[]> dialogueDatabase;

    // --- MENU ITEM ---
    [MenuItem("My AI Tools/Dialogue Generator")]
    public static void ShowWindow()
    {
        GetWindow<DialogueWindow>("AI Dialogue Tool");
    }

    // Initialize the database when the window is enabled
    void OnEnable()
    {
        InitializeDatabase();
    }

    // --- GUI (USER INTERFACE) ---
    void OnGUI()
    {
        GUILayout.Label("AI NPC Dialogue Generator (Final)", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 1. Character Selection
        GUILayout.Label("Select Character:", EditorStyles.label);
        selectedCharacterIndex = EditorGUILayout.Popup(selectedCharacterIndex, characterOptions);

        GUILayout.Space(5);

        // 2. Player Input Area
        GUILayout.Label("Player Input:", EditorStyles.label);
        playerInput = EditorGUILayout.TextArea(playerInput, GUILayout.Height(40));

        GUILayout.Space(10);

        // 3. Generate Button
        if (!isGenerating)
        {
            if (GUILayout.Button("Generate Dialogue"))
            {
                GenerateMockDialogue();
            }
        }
        else
        {
            // Loading Indicator
            GUILayout.Label("AI is crafting a masterpiece... Please wait.", EditorStyles.helpBox);
        }

        GUILayout.Space(15);

        // 4. Result Area
        GUILayout.Label("Generated Response:", EditorStyles.boldLabel);
        
        GUIStyle responseStyle = new GUIStyle(EditorStyles.textArea);
        responseStyle.wordWrap = true;
        responseStyle.fontSize = 12;
        
        // Display the generated text (Editable by user)
        generatedResponse = EditorGUILayout.TextArea(generatedResponse, responseStyle, GUILayout.Height(80));

        GUILayout.Space(10);

        // --- 5. SAVE BUTTON (Visible only if text exists) ---
        if (!string.IsNullOrEmpty(generatedResponse))
        {
            GUI.backgroundColor = Color.green; // Make button green to highlight it
            if (GUILayout.Button("Save to JSON File"))
            {
                SaveDialogueToJSON();
            }
            GUI.backgroundColor = Color.white; // Reset color
        }
    }

    // --- LOGIC (GENERATION) ---
    async void GenerateMockDialogue()
    {
        isGenerating = true;
        generatedResponse = ""; 
        
        // Simulate network/processing delay (1 second)
        await Task.Delay(1000);

        string selectedRole = characterOptions[selectedCharacterIndex];

        if (dialogueDatabase.ContainsKey(selectedRole))
        {
            string[] lines = dialogueDatabase[selectedRole];
            string randomLine = lines[Random.Range(0, lines.Length)];
            
            // Format the output: [Name]: Line
            generatedResponse = $"[{selectedRole}]: {randomLine}";
        }
        else
        {
            generatedResponse = "Error: Database connection failed.";
        }

        isGenerating = false;
        Repaint(); // Refresh the editor window
    }

    // --- SAVE SYSTEM (JSON) ---
    void SaveDialogueToJSON()
    {
        // 1. Create Data Object
        DialogueData data = new DialogueData();
        data.characterName = characterOptions[selectedCharacterIndex];
        data.dialogueText = generatedResponse; // Save whatever is currently in the text box
        data.dateCreated = System.DateTime.Now.ToString(); // Timestamp

        // 2. Serialize to JSON string
        // 'true' parameter makes the JSON pretty-printed (readable)
        string json = JsonUtility.ToJson(data, true);

        // 3. Define File Name (Unique with random number)
        // Example: Dialogue_CityGuard_4921.json
        string fileName = $"Dialogue_{data.characterName.Replace(" ", "")}_{Random.Range(1000, 9999)}.json";
        
        // 4. Define Path (Assets folder)
        string path = Application.dataPath + "/" + fileName;

        // 5. Write to File
        File.WriteAllText(path, json);

        // 6. Refresh Unity Database (so the file appears instantly)
        AssetDatabase.Refresh();

        // 7. Success Feedback
        Debug.Log($"<color=green>SUCCESS:</color> Dialogue saved to: {path}");
        EditorUtility.DisplayDialog("Success", $"Dialogue saved as {fileName}!", "OK");
    }

    // --- MOCK DATABASE (FULL CONTENT) ---
    void InitializeDatabase()
    {
        dialogueDatabase = new Dictionary<string, string[]>();

        // 1. VILLAGE ELDER
        dialogueDatabase.Add("Village Elder", new string[] {
            "Welcome, traveler. The harvest has been poor this year.",
            "I don't want any trouble in my village square.",
            "I cannot sell you that plot of land without the council's seal.",
            "News from the capital is grim, keep your sword sharp.",
            "Have you seen my son? He wandered off towards the river again.",
            "Meet me at the tavern tonight, we shall discuss the matter there.",
            "We don't take kindly to strangers, but you seem honest.",
            "Collecting taxes is getting harder, the King is ruthless.",
            "The irrigation channels are clogged again, can you help?",
            "I am the law here, at least until the royal army arrives.",
            "They say ghosts haunt the old mill, stay away from there.",
            "If you catch the fox stealing my chickens, I will reward you.",
            "Winter is coming, have you stocked up on firewood?",
            "My daughter's wedding is soon, the whole village is invited.",
            "Adventurers like you usually bring nothing but trouble.",
            "If I weren't the Elder, I'd be napping in the fields right now.",
            "Don't talk politics with me, just do your job.",
            "We need to repair the bridge at the village entrance.",
            "Aunt Martha makes the best apple pie in these lands.",
            "Leave me be, I have a terrible headache today."
        });

        // 2. CITY GUARD
        dialogueDatabase.Add("City Guard", new string[] {
            "Halt! Show me your identification papers.",
            "Entrance to the city is forbidden after sunset.",
            "Do you carry any contraband magical items?",
            "Hey you! Sheathe that weapon immediately.",
            "Do you have a permit to loiter here?",
            "Perhaps a few gold coins could refresh my memory...",
            "The Lord's command is absolute: Thieves lose a hand.",
            "I've got my eye on you, stranger. No sudden moves.",
            "Don't tell anyone I fell asleep on duty last night.",
            "There is plenty of room in the dungeon for scum like you.",
            "Only nobility may pass through this gate.",
            "Patrolling makes me thirsty. Do you have any water?",
            "Begging is prohibited in the city district, move along!",
            "You look surprisingly like the person on this wanted poster...",
            "I am the law! Are you questioning me?",
            "Pray for the King's health, or we don't get paid.",
            "See that merchant? He's acting very suspiciously.",
            "My helmet is too heavy, I should have been a farmer.",
            "Speak quickly, I don't have all day.",
            "Bribery is a serious crime... but how much are you offering?"
        });

        // 3. MYSTERIOUS WIZARD
        dialogueDatabase.Add("Mysterious Wizard", new string[] {
            "The stars predict a dark path for you today.",
            "Your aura is weak... Have you never witnessed true magic?",
            "I must wait for the full moon to brew this potion.",
            "Silence! You are breaking my concentration.",
            "That primitive sword of yours is nothing compared to my arcane arts.",
            "The ancient scrolls do not mention your destiny.",
            "The mana flow in this region is terribly erratic.",
            "Bring me a dragon scale, and I shall teach you immortality.",
            "The emptiness in your eyes... it shows a hunger for knowledge.",
            "Do not touch! That orb could consume your soul.",
            "Time flows differently for us sorcerers.",
            "Talking to you is like explaining the universe to an ant.",
            "The last person who entered my tower uninvited is now a toad.",
            "Magic is an art reserved only for the intellectually gifted.",
            "The shadows are whispering... Can you hear them?",
            "You cannot imagine the price I paid for this staff.",
            "My teleportation spell backfired again, I feel dizzy.",
            "Necromancy? It is a forbidden art... but I know a few things.",
            "You are not a worthy opponent for me.",
            "I solved the secrets of the cosmos, but I forgot where I put my keys."
        });

        // 4. WRETCHED BEGGAR
        dialogueDatabase.Add("Wretched Beggar", new string[] {
            "Alms... alms for the poor...",
            "I haven't eaten in three days, my lord.",
            "Please, just a crust of bread is all I ask.",
            "I used to be a soldier like you, until I took an arrow to the knee.",
            "The rats in the alley are my only friends.",
            "Don't look at me with pity! Look at me with charity!",
            "It's so cold... so very cold tonight.",
            "Can you spare a copper? Just one copper?",
            "They burned my house down... took everything.",
            "Luck has never been on my side.",
            "Bless you, kind soul! Bless you!",
            "I saw the King once... he didn't look at me either.",
            "This old blanket is the only thing I own.",
            "Beware the shadows in the sewers...",
            "A coin? For me? Are you an angel?",
            "I found a shiny rock yesterday, but a goblin stole it.",
            "Cough... cough... I don't think I'll make it through winter.",
            "People walk by as if I am invisible.",
            "Do you have any old boots? Mine are falling apart.",
            "May the gods smile upon you, unlike they did on me."
        });

        // 5. TAVERN KEEPER
        dialogueDatabase.Add("Tavern Keeper", new string[] {
            "Welcome to The Rusty Tankard! Leave your weapons at the door.",
            "Best ale in the kingdom, brewed it myself this morning.",
            "If you're looking for a room, it's 5 gold a night.",
            "Don't start a fight, I just cleaned the floor.",
            "You look like you've traveled a long way. Sit, drink!",
            "Whatever gossip you want to hear, it will cost you a drink.",
            "We serve stew and bread. Take it or leave it.",
            "That hooded man in the corner? He's been there all day.",
            "Pay upfront. I don't do tabs for adventurers.",
            "We are out of wine. The shipment was attacked by bandits.",
            "Need a job? I need someone to clear the rats from the cellar.",
            "Music starts at sundown, try not to insult the bard.",
            "Hey! No magic casting inside the tavern!",
            "I've heard rumors of a dragon in the northern peaks.",
            "Last group of adventurers who sat there never came back.",
            "Cleaning up vomit is not in my job description.",
            "One round for the house! Just kidding, pay up.",
            "You want information? Buy me a drink first.",
            "Close the door! You're letting the cold in.",
            "Last call! Finish your drinks and get out."
        });

        // 6. GOBLIN SCOUT
        dialogueDatabase.Add("Goblin Scout", new string[] {
            "Shinies? You have shinies for Rakka?",
            "Me not see you! You not see me!",
            "Big human ugly. Rakka hide.",
            "Smash! Smash! Hehe...",
            "King Goblin want head of big man.",
            "Me hungry. You look tasty.",
            "Gold! Gold! Give!",
            "Run away! Run away!",
            "Rakka find shortcut through mountain.",
            "Stupid wolf eat my sandwich.",
            "Me best scout. Others dumb.",
            "Trap is set. Don't tell big human.",
            "Dark is good. Light hurts eyes.",
            "Me poke you with stick!",
            "Why human smell like wet dog?",
            "Shhh! Big monster sleeping nearby.",
            "Rakka trade? Dead rat for shiny sword?",
            "Fire burn! Fire bad!",
            "Me no like water. Water make Rakka clean.",
            "Glory to the Goblin King!"
        });
    }
}

// --- DATA CLASS (Serialization) ---
[System.Serializable]
public class DialogueData
{
    public string characterName;
    public string dialogueText;
    public string dateCreated;
}