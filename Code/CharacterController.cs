using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

struct Dialog
{
	public string text;
	public string ID = "";
	public Array<String> tags;

	public Dialog()
	{
		tags = [];
	}
};

struct Result
{
	public bool correctCountry;
	public bool correctPeople;
	public bool correctMoney;
	public bool correctSus;
}

public partial class CharacterController : Node
{
	public static CharacterController instance;

	String currentDialog;
	[Export(PropertyHint.File, "*.csv")] string introFile;
	[Export(PropertyHint.File, "*.csv")] string whereFile;
	[Export(PropertyHint.File, "*.csv")] string whoFile;
	[Export(PropertyHint.File, "*.csv")] string activitiesFile;
	[Export(PropertyHint.File, "*.csv")] string moneyFile;

	List<Dialog> introDialog = [];
	List<Dialog> whereDialog = [];
	List<Dialog> whoDialog = [];
	List<Dialog> activitiesDialog = [];
	List<Dialog> moneyDialog = [];

	List<string> themes = [];

	bool isCriminal;
	Dialog introLine;
	Dialog whereLine;
	Dialog whoLine;
	Dialog activitiesLine;
	Dialog moneyLine;

	[Export] NodePath speechLabel;
	[Export] NodePath whereButton;
	[Export] NodePath whoButton;
	[Export] NodePath activitiesButton;
	[Export] NodePath moneyButton;
	string currentLine = "";
	int characterIndex = 0;
	float lineTimer = 0.0f;
	float lineAnimationDelay = 0.03f;

	[Export] NodePath themeLabel;

	[Export] Array<Texture2D> characterTextures;
	[Export] NodePath sprite3D;

	Random random = new();

	int numberOfCharacters = 0;
	[Export] Array<NodePath> postcards;

	public string name = "Bob Jiang";
	[Export] NodePath label;

	List<Result> results = [];

	bool suspectedCriminal = false;

	public void EvaluateResult(List<string> countryTags, int numberPeople, int moneyIndex)
	{
		Result result = new();
		result.correctCountry = countryTags.Contains(whereLine.ID);
		result.correctPeople = numberPeople == whoLine.ID.ToInt();

		if (moneyIndex == 1)
		{
			result.correctMoney = moneyLine.ID == "low";
		}
		if (moneyIndex == 2)
		{
			result.correctMoney = moneyLine.ID == "medium";
		}
		if (moneyIndex == 3)
		{
			result.correctMoney = moneyLine.ID == "high";
		}
		result.correctSus = suspectedCriminal == isCriminal;

		//foreach (string country in countryTags) {
		//	Debug.WriteLine("Countries: " + country);
		//}
		//Debug.WriteLine("Expected Countries: " + whereLine.ID);
		Debug.WriteLine("Correct Country: " + result.correctCountry);
		//Debug.WriteLine("Expected People: " + whoLine.ID);
		//Debug.WriteLine("People: " + numberPeople);
		Debug.WriteLine("Correct People: " + result.correctPeople);
		//Debug.WriteLine("Expected Money: " + moneyLine.ID);
		//Debug.WriteLine("Money: " + moneyIndex);
		Debug.WriteLine("Correct Money: " + result.correctMoney);
        Debug.WriteLine("Correct Sus: " + result.correctSus);

        results.Add(result);
	}

	void changeSpeech(string speech)
	{
		currentLine = speech;
		GetNode<Label>(speechLabel).Text = "";
		characterIndex = 0;
	}

	public void initCharacter()
	{
		numberOfCharacters++;
		isCriminal = random.Next(2) == 0 ? false : true;
		bool shouldHaveThemedTag = isCriminal;
		for (int i = 0; i < 10; i++)
		{
			int indexNumber = random.Next(introDialog.Count);
			bool hasTheme = false;
			foreach (string tag in introDialog[indexNumber].tags)
			{
				hasTheme |= themes.Contains(tag);
			}
			introLine = introDialog[indexNumber];
			if (shouldHaveThemedTag == hasTheme)
			{
				break;
			}
		}
		for (int i = 0; i < 10; i++)
		{
			int indexNumber = random.Next(whereDialog.Count);
			bool hasTheme = false;
			foreach (string tag in whereDialog[indexNumber].tags)
			{
				hasTheme |= themes.Contains(tag);
			}
			whereLine = whereDialog[indexNumber];
			if (shouldHaveThemedTag == hasTheme)
			{
				break;
			}
		}
		for (int i = 0; i < 10; i++)
		{
			int indexNumber = random.Next(whoDialog.Count);
			bool hasTheme = false;
			foreach (string tag in whoDialog[indexNumber].tags)
			{
				hasTheme |= themes.Contains(tag);
			}
			whoLine = whoDialog[indexNumber];
			if (shouldHaveThemedTag == hasTheme)
			{
				break;
			}
		}
		for (int i = 0; i < 10; i++)
		{
			int indexNumber = random.Next(activitiesDialog.Count);
			bool hasTheme = false;
			foreach (string tag in activitiesDialog[indexNumber].tags)
			{
				hasTheme |= themes.Contains(tag);
			}
			activitiesLine = activitiesDialog[indexNumber];
			if (shouldHaveThemedTag == hasTheme)
			{
				break;
			}
		}
		for (int i = 0; i < 10; i++)
		{
			int indexNumber = random.Next(moneyDialog.Count);
			bool hasTheme = false;
			foreach (string tag in moneyDialog[indexNumber].tags)
			{
				hasTheme |= themes.Contains(tag);
			}
			moneyLine = moneyDialog[indexNumber];
			if (shouldHaveThemedTag == hasTheme)
			{
				break;
			}
		}
		GetNode<Button>(whereButton).Disabled = false;
		GetNode<Button>(whoButton).Disabled = false;
		GetNode<Button>(activitiesButton).Disabled = false;
		GetNode<Button>(moneyButton).Disabled = false;
		changeSpeech(introLine.text);
		GetNode<Sprite3D>(sprite3D).Texture = characterTextures[random.Next(characterTextures.Count)];

		for (int i = 0; i < postcards.Count; i++)
		{
			GetNode<Sprite3D>(postcards[i]).Visible = numberOfCharacters > ((i + 1) * 3);
		}
		GetNode<Label>(label).Text = name;
		suspectedCriminal = false;
    }

	void readDialogFile(string fileName, List<Dialog> dialogList, bool hasID)
	{
		FileAccess file = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
		string fileText = file.GetAsText();
		string[] lines = fileText.Split("\n");
		foreach (string line in lines)
		{
			Dialog dialog = new Dialog();
			List<string> tags = [.. line.Split(",")];
			dialog.text = tags[0];
			tags.RemoveAt(0);
			if (hasID && tags.Count > 0)
			{
				//Debug.WriteLine(dialog.text);
				dialog.ID = tags[0];
				//Debug.WriteLine(dialog.ID);
				tags.RemoveAt(0);
			}
			foreach (string tag in tags)
			{
				if (!string.IsNullOrWhiteSpace(tag))
				{
					dialog.tags.Add(tag.Trim());
				}
			}
			dialogList.Add(dialog);
		}
	}

	public override void _Ready()
	{
		instance = this;

		readDialogFile(introFile, introDialog, false);
		readDialogFile(whereFile, whereDialog, true);
		readDialogFile(whoFile, whoDialog, true);
		readDialogFile(activitiesFile, activitiesDialog, false);
		readDialogFile(moneyFile, moneyDialog, true);

		/*foreach (Dialog dialog in dialogs)
		{
			Debug.WriteLine("Text: " + dialog.text);
			foreach (string tag in dialog.tags)
			{
				Debug.WriteLine("Tag:" + tag);
			}
		}*/

		selectThemes();
		initCharacter();
		//Debug.WriteLine("Intro: " + introLine);
		//Debug.WriteLine("Where: " + whereLine);
		//Debug.WriteLine("who: " + whoLine);
		//Debug.WriteLine("Activities: " + activitiesLine);
		//Debug.WriteLine("Money: " + moneyLine);
		GetNode<Button>(whereButton).Pressed += WhereButtonPressed;
		GetNode<Button>(whoButton).Pressed += WhoButtonPressed;
		GetNode<Button>(activitiesButton).Pressed += ActivitiesButtonPressed;
		GetNode<Button>(moneyButton).Pressed += MoneyButtonPressed;
	}

	void WhereButtonPressed()
	{
		changeSpeech(whereLine.text);
		GetNode<Button>(whereButton).Disabled = true;
	}

	void WhoButtonPressed()
	{
		changeSpeech(whoLine.text);
		GetNode<Button>(whoButton).Disabled = true;
	}

	void ActivitiesButtonPressed()
	{
		changeSpeech(activitiesLine.text);
		GetNode<Button>(activitiesButton).Disabled = true;
	}

	void MoneyButtonPressed()
	{
		changeSpeech(moneyLine.text);
		GetNode<Button>(moneyButton).Disabled = true;
	}

	void selectThemes()
	{
		HashSet<string> uniqueTags = [];
		List<Dialog> allDialog = [];
		allDialog.AddRange(introDialog);
		allDialog.AddRange(whereDialog);
		allDialog.AddRange(whoDialog);
		allDialog.AddRange(activitiesDialog);
		allDialog.AddRange(moneyDialog);
		foreach (Dialog dialog in allDialog)
		{
			foreach (string tag in dialog.tags)
			{
				uniqueTags.Add(tag);
			}
		}

		HashSet<string> randomTags = [];
		while (randomTags.Count != 3)
		{
			randomTags.Add(uniqueTags.ElementAt(random.Next(uniqueTags.Count)));
		}
		themes.AddRange(randomTags);
		/*foreach (string tag in themes)
		{
			Debug.WriteLine("Tag:" + tag);
		}*/
		GetNode<Label>(themeLabel).Text = "";
		foreach (string tag in themes)
		{
			GetNode<Label>(themeLabel).Text += tag + "\n";
		}
	}

	public override void _Process(double delta)
	{
		lineTimer += (float)delta;
		while (lineTimer > lineAnimationDelay)
		{
			lineTimer -= lineAnimationDelay;
			characterIndex++;
			characterIndex = Math.Clamp(characterIndex, 0, currentLine.Length);
			GetNode<Label>(speechLabel).Text = currentLine.Substr(0, characterIndex);
		}
	}
}
