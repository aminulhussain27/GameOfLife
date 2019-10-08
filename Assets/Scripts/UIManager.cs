using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Header ("GameObject")]
	public GameObject PlayButton;
	public GameObject StopButton;
	public GameObject StepButton;
	public GameObject homePanel;

	[Header("Button")]
	public Button enterButton;
	public Button backButton;

	public GameObject MoreSizeButton;
	public GameObject LessSizeButton;

	[Header ("Slider")]
	public Slider timeSlider;

    [Header("Text")]
    public Text updateIntervalText;

	private void Start()
    {
		enterButton.onClick.RemoveAllListeners ();
		enterButton.onClick.AddListener (() => 
			{
					EnterButtonAction();
			});

		backButton.onClick.RemoveAllListeners ();
		backButton.onClick.AddListener (() => 
				{
					BackButtonAction();
				});

		updateIntervalText.text = "Time Step: " + Mathf.Round(GameManager.Instance().updateInterval * 1000.0f) + "ms";
    }


	//Enter button in Home screen
	void EnterButtonAction()
	{
		//Leadt to start the game
		ClearGrid();
		homePanel.SetActive (false);
	}

	//Back to Menu panel
	void BackButtonAction()
	{
		homePanel.SetActive (true);
		ClearGrid();

	}

	//Starts the timing here
    public void StartButtonAction ()
	{
		GameManager.Instance().Run ();

		PlayButton.SetActive (false);
		StopButton.SetActive (true);
		StepButton.SetActive (false);
	}

	//Stops the generation here
	public void StopButtonAction ()
	{
		GameManager.Instance().Stop ();

		PlayButton.SetActive (true);
		StopButton.SetActive (false);
		StepButton.SetActive (true);
	}


	//It redirects to next generation
	public void NextStepButtonAction()
	{
		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.CLICK);

		GameManager.Instance().UpdateCells();
	}

    // Clear map now automatically stops the game.
	public void ClearGrid ()
	{
        StopButtonAction();
		GameManager.Instance().ResetCells ();
	}


	//Changing the interval time for go to next generation
	public void ChangeUpdateInterval (Slider slider)
	{
		GameManager.Instance().updateInterval = slider.value;

		updateIntervalText.text = "Time Step: " + Mathf.Round(GameManager.Instance().updateInterval * 1000.0f) + "ms";
    }
}
