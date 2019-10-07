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

	public GameObject MoreSizeButton;
	public GameObject LessSizeButton;

	[Header ("Slider")]
	public Slider timeSlider;

    [Header("Text")]
    public Text mapSizeText;
    public Text updateIntervalText;

    void Start()
    {
		updateIntervalText.text = "Time Step: " + Mathf.Round(GameManager.Instance().updateInterval * 1000.0f) + "ms";
    }

    public void StartSim ()
	{
		GameManager.Instance().Run ();

		PlayButton.SetActive (false);
		StopButton.SetActive (true);
		StepButton.SetActive (false);
	}

	public void StopSim ()
	{
		GameManager.Instance().Stop ();

		PlayButton.SetActive (true);
		StopButton.SetActive (false);
		StepButton.SetActive (true);
	}

	public void NextStep()
	{
		GameManager.Instance().UpdateCells();
	}

    // Clear map now automatically stops the simulation.
	public void ClearMap ()
	{
        StopSim();
		GameManager.Instance().ResetCells ();
	}


	public void ChangeUpdateInterval (Slider slider)
	{
		GameManager.Instance().updateInterval = slider.value;

		updateIntervalText.text = "Time Step: " + Mathf.Round(GameManager.Instance().updateInterval * 1000.0f) + "ms";
    }
}
