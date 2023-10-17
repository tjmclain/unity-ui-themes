using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	public static class StyleHelperEditorUtility
	{
		public static void ApplyStylesInScene()
		{
			var stageHandle = StageUtility.GetCurrentStageHandle();
			var styleHelpers = stageHandle.FindComponentsOfType<StyleHelper>();
			foreach (var styleHelper in styleHelpers)
			{
				styleHelper.ApplyStyle();
			}

			RepaintGameView();
		}

		// I want to repaint the GameView as well as the scene view
		// There isn't any public interface for GameView, so I have to do some wonky shit to access it
		// However, nothing I tried seemed to work. Maybe come back to this later? Probably a waste of time though
		public static void RepaintGameView()
		{
			// Attempt #1: find the GameView specifically (NOTE: does not work)
			// https://discussions.unity.com/t/how-to-manually-update-gameview/70035
			//var assembly = typeof(UnityEditor.EditorWindow).Assembly;
			//var type = assembly.GetType("UnityEditor.GameView");
			//var gameView = EditorWindow.GetWindow(type);
			//if (gameView != null)
			//{
			//	gameView.Repaint();
			//}

			// Attempt #2: repaint all views (NOTE: does not force game view to repaint)
			//UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		}
	}
}