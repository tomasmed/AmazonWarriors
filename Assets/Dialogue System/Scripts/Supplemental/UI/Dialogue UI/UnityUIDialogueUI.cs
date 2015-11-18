#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This component implements IDialogueUI using Unity UI. It's based on 
	/// AbstractDialogueUI and compiles the Unity UI versions of the controls defined in 
	/// UnityUISubtitleControls, UnityUIResponseMenuControls, UnityUIAlertControls, etc.
	///
	/// To use this component, build a UI layout (or drag a pre-built one in the Prefabs folder
	/// into your scene) and assign the UI control properties. You must assign a scene instance 
	/// to the DialogueManager; you can't use prefabs with Unity UI dialogue UIs.
	/// 
	/// The required controls are:
	/// - NPC subtitle line
	/// - PC subtitle line
	/// - Response menu buttons
	/// 
	/// The other control properties are optional. This component will activate and deactivate
	/// controls as they are needed in the conversation.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Dialogue/Unity UI Dialogue UI")]
	public class UnityUIDialogueUI : AbstractDialogueUI {
		
		/// <summary>
		/// The UI root.
		/// </summary>
		[HideInInspector] public UnityUIRoot unityUIRoot;
		
		/// <summary>
		/// The dialogue controls used in conversations.
		/// </summary>
		public UnityUIDialogueControls dialogue;
		
		/// <summary>
		/// QTE (Quick Time Event) indicators.
		/// </summary>
		public UnityEngine.UI.Graphic[] qteIndicators;
		
		/// <summary>
		/// The alert message controls.
		/// </summary>
		public UnityUIAlertControls alert;

		/// <summary>
		/// Set <c>true</c> to always keep a control focused; useful for gamepads.
		/// </summary>
		[Tooltip("Always keep a control focused; useful for gamepads")]
		public bool autoFocus = false;

		[Tooltip("Look for OverrideUnityUIDialogueControls on actors")]
		public bool findActorOverrides = false;

		private UnityUIQTEControls qteControls;
		
		public override AbstractUIRoot UIRoot {
			get { return unityUIRoot; }
		}
		
		public override AbstractDialogueUIControls Dialogue {
			get { return dialogue; }
		}
		
		public override AbstractUIQTEControls QTEs {
			get { return qteControls; }
		}
		
		public override AbstractUIAlertControls Alert {
			get { return alert; }
		}
		
		private class QueuedAlert {
			public string message;
			public float duration;
			public QueuedAlert(string message, float duration) {
				this.message = message;
				this.duration = duration;
			}
		}
		
		private Queue<QueuedAlert> alertQueue = new Queue<QueuedAlert>();

		// References to the original controls in case an actor temporarily overrides them:
		private UnityUISubtitleControls originalNPCSubtitle;
		private UnityUISubtitleControls originalPCSubtitle;
		private UnityUIResponseMenuControls originalResponseMenu;

		// Caches overrides by actor so we only need to search an actor once:
		private Dictionary<Transform, OverrideUnityUIDialogueControls> overrideCache = new Dictionary<Transform, OverrideUnityUIDialogueControls>();

		#region Initialization
		
		/// <summary>
		/// Sets up the component.
		/// </summary>
		public override void Awake() {
			base.Awake();
			FindControls();
		}
		
		/// <summary>
		/// Logs warnings if any critical controls are unassigned.
		/// </summary>
		private void FindControls() {
			UITools.RequireEventSystem();
			qteControls = new UnityUIQTEControls(qteIndicators);
			if (DialogueDebug.LogErrors) {
				if (DialogueDebug.LogWarnings) {
					if (dialogue.npcSubtitle.line == null) Debug.LogWarning(string.Format("{0}: UnityUIDialogueUI NPC Subtitle Line needs to be assigned.", DialogueDebug.Prefix));
					if (dialogue.pcSubtitle.line == null) Debug.LogWarning(string.Format("{0}: UnityUIDialogueUI PC Subtitle Line needs to be assigned.", DialogueDebug.Prefix));
					if (dialogue.responseMenu.buttons.Length == 0 && dialogue.responseMenu.buttonTemplate == null) Debug.LogWarning(string.Format("{0}: UnityUIDialogueUI Response buttons need to be assigned.", DialogueDebug.Prefix));
					if (alert.line == null) Debug.LogWarning(string.Format("{0}: UnityUIDialogueUI Alert Line needs to be assigned.", DialogueDebug.Prefix));
				}
			}
			originalNPCSubtitle = dialogue.npcSubtitle;
			originalPCSubtitle = dialogue.pcSubtitle;
			originalResponseMenu = dialogue.responseMenu;
		}

		private OverrideUnityUIDialogueControls FindActorOverride(Transform actor) {
			if (!overrideCache.ContainsKey(actor)) {
				overrideCache.Add(actor, (actor != null) ? actor.GetComponentInChildren<OverrideUnityUIDialogueControls>() : null);
			}
			return overrideCache[actor];
		}

		public void OnLevelWasLoaded(int level) {
			UITools.RequireEventSystem();
		}

		public override void Open()
		{
			overrideCache.Clear();
			base.Open();
		}

		#endregion

		#region Alerts

		public override void ShowAlert(string message, float duration) {
			if (alert.queueAlerts) {
				alertQueue.Enqueue(new QueuedAlert(message, duration));
				if (!alert.IsVisible) ShowNextQueuedAlert();
			} else {
				StartShowingAlert(message, duration);
			}
		}

		public override void HideAlert() {
			base.HideAlert();
			if (alert.queueAlerts) ShowNextQueuedAlert();
		}

		public override void OnContinue() {
			CancelInvoke("HideAlert");
			base.OnContinue();
		}

		private void ShowNextQueuedAlert() {
			if (alertQueue.Count > 0) {
				var queuedAlert = alertQueue.Dequeue();
				StartShowingAlert(queuedAlert.message, queuedAlert.duration);
			}
		}
		
		private void StartShowingAlert(string message, float duration) {
			base.ShowAlert(message, duration);
			if (autoFocus) alert.AutoFocus();
			CancelInvoke("HideAlert");
			Invoke("HideAlert", duration);
		}

		#endregion

		#region Subtitles
		
		public override void ShowSubtitle(Subtitle subtitle) {
			if (findActorOverrides) {
				var overrideControls = FindActorOverride(subtitle.speakerInfo.transform);
				if (subtitle.speakerInfo.characterType == CharacterType.NPC) {
					dialogue.npcSubtitle = (overrideControls != null) ? overrideControls.subtitle : originalNPCSubtitle;
				} else {
					dialogue.pcSubtitle = (overrideControls != null) ? overrideControls.subtitle : originalPCSubtitle;
				}
			}
			HideResponses();
			base.ShowSubtitle(subtitle);
			CheckSubtitleAutoFocus(subtitle);
		}

		public void CheckSubtitleAutoFocus(Subtitle subtitle) {
			if (autoFocus) {
				if (subtitle.speakerInfo.IsPlayer) {
					dialogue.pcSubtitle.AutoFocus();
				} else {
					dialogue.npcSubtitle.AutoFocus();
				}
			}
		}

		#endregion

		#region Responses

		public override void ShowResponses (Subtitle subtitle, Response[] responses, float timeout) {
			if (findActorOverrides) {
				// Use speaker's (NPC's) world space canvas for subtitle reminder, and for menu if set:
				var overrideControls = FindActorOverride(subtitle.speakerInfo.transform);
				var subtitleReminder = (overrideControls != null) ? overrideControls.subtitleReminder : originalResponseMenu.subtitleReminder;
				if (overrideControls != null && overrideControls.responseMenu.panel != null) {
					dialogue.responseMenu = (overrideControls != null && overrideControls.responseMenu.panel != null) ? overrideControls.responseMenu : originalResponseMenu;
				} else {
					// Otherwise use PC's world space canvas for menu if set:
					overrideControls = FindActorOverride(subtitle.listenerInfo.transform);
					dialogue.responseMenu = (overrideControls != null && overrideControls.responseMenu.panel != null) ? overrideControls.responseMenu : originalResponseMenu;
				}
				// Either way, use speaker's (NPC's) subtitle reminder:
				dialogue.responseMenu.subtitleReminder = subtitleReminder;
			}
			base.ShowResponses(subtitle, responses, timeout);
			CheckResponseMenuAutoFocus();
		}

		public void CheckResponseMenuAutoFocus() {
			if (autoFocus) dialogue.responseMenu.AutoFocus();
		}

		public override void HideResponses() {
			dialogue.responseMenu.DestroyInstantiatedButtons();
			base.HideResponses();
		}

		#endregion

	}

}
#endif