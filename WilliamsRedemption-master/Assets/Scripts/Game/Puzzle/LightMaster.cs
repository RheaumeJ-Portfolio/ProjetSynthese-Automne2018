﻿using System.Collections;
using System.Collections.Generic;
using Game.Puzzle;
using Game.Puzzle.Light;
using UnityEngine;
using UnityEngine.Analytics;

public class LightMaster : MonoBehaviour
{

	[Tooltip("Array of lights. Add the lights in the order they should light up.")]
	[SerializeField] private GameObject [] Lights;
	[Tooltip("Amount of time in seconds where each light stays open.")]
	[SerializeField] private float timePerLight;
	[Tooltip("Checking this box will close every open light and open every closed light.")]
	[SerializeField] private bool AlternateLights;

	private ITriggerable currentlight;
	private float timeAtStart;
	private int currentLightIndex;

	private void Awake()
	{
		timeAtStart = 0;
		currentLightIndex = 0;
	}

	private void Start()
	{
		
		if (!AlternateLights)
		{
			currentlight = Lights[currentLightIndex].GetComponent<ITriggerable>();
			currentlight.Open();
		}
		
		timeAtStart = Time.time;
	}
	
	private void Update () 
	{
		if (!AlternateLights)
		{
			Cycle();
		}
		else
		{
			Alternate();
		}
		
	}

	private void Cycle()
	{
		if(TimeSinceLit() >= timePerLight)
		{
			currentlight.Close();
			currentLightIndex++;
			if (currentLightIndex >= Lights.Length)
			{
				currentLightIndex = 0;
				currentlight = Lights[currentLightIndex].GetComponent<ITriggerable>();
				currentlight.Open();
				timeAtStart = Time.time;
			}
			else
			{
				currentlight = Lights[currentLightIndex].GetComponent<ITriggerable>();
				currentlight.Open();
				timeAtStart = Time.time;
			}
		}
	}

	private void Alternate()
	{
		if (TimeSinceLit() >= timePerLight)
		{
			foreach (var light in Lights)
			{
				currentlight = light.GetComponent<ITriggerable>();

				if (currentlight.IsOpened())
				{
					currentlight.Close();
				}
				else
				{
					currentlight.Open();
				}
			}

			timeAtStart = Time.time;
		}
	}

	private float TimeSinceLit()
	{
		float timeSinceStart = Time.time - timeAtStart;
		return timeSinceStart;
	}
}
