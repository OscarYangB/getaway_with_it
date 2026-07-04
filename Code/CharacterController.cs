using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

struct Dialog
{
	public string text;
	public Array<String> tags;

	public Dialog()
	{
		tags = [];
	}
};

public partial class CharacterController : Node
{
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

	void readDialogFile(string fileName, List<Dialog> dialogList)
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
		readDialogFile(introFile, introDialog);
		readDialogFile(whereFile, whereDialog);
		readDialogFile(whoFile, whoDialog);
		readDialogFile(activitiesFile, activitiesDialog);
		readDialogFile(moneyFile, moneyDialog);

		/*foreach (Dialog dialog in dialogs)
		{
			Debug.WriteLine("Text: " + dialog.text);
			foreach (string tag in dialog.tags)
			{
				Debug.WriteLine("Tag:" + tag);
			}
		}*/

		selectTags();
	}

	void selectTags()
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

		Random random = new();
		HashSet<string> randomTags = [];
		while (randomTags.Count != 3)
		{
			randomTags.Add(uniqueTags.ElementAt(random.Next(uniqueTags.Count)));
		}
		themes.AddRange(randomTags);
		foreach (string tag in themes)
		{
			Debug.WriteLine("Tag:" + tag);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
