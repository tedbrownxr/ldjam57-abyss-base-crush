using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class EndingState : AppState 
	{
		public const string Name = "Ending";
		public override string StateName => Name;

		public TMP_Text _scoreText;

		private float _finalScore;
		private float _timer;
		private float _duration = 4;

		public void ClickReload ()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Main", UnityEngine.SceneManagement.LoadSceneMode.Single);
		}

		public void ClickReview ()
		{
			Application.OpenURL("https://ldjam.com/events/ludum-dare/57/abyss-base-crush");
		}

		protected override void OnAfterEnter()
		{
			_scoreText.text = string.Empty;
			var em = FindObjectOfType<EnemyManager>();
			_finalScore = em.TotalEnemiesKilled * 1000;
			_timer = 0;
		}

		protected override void OnProcess()
		{
			_timer += Time.deltaTime;
			float t = Mathf.Clamp01(_timer / _duration);
			t = TweenLib.EaseOutCubic(t);
			int displayScore = (int) Mathf.Lerp(0, _finalScore, t);
			// comma delimited
			_scoreText.text = displayScore.ToString("N0");
		}

		protected override void OnBeforeExit()
		{
			
		}		
	}
}
