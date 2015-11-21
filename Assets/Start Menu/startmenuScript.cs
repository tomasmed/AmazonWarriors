using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class startmenuScript : MonoBehaviour {

	public Canvas QuitMenu; 
	public Button startText;
	public Button exitText;
	// Use this for initialization
	void Start () 
	{
		QuitMenu = QuitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		QuitMenu.enabled = false;
		
	}

	public void exitQuestion()
	{
		QuitMenu.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
	}

	public void noAnswer ()
	{
		QuitMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
	}

	public void StartLevel()
	{
		Application.LoadLevel (1);

	}

	public void ExitGame()
	{
		Application.Quit ();
	}

}


	

