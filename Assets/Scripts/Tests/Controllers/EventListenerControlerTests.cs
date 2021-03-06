﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

namespace Tests
{
	public class EventListenerControlerTests : TestTools.BaseTestClass
	{
		[UnityTest]
		public IEnumerator OnRaiseNotNull()
		{
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			eventListenerCtrl.eventHandle = ScriptableObject
				.CreateInstance<EventHandle>();

			yield return new WaitForEndOfFrame();

			Assert.NotNull(eventListenerCtrl.onRaise);
		}

		[UnityTest]
		public IEnumerator OnRaise()
		{
			var called = 0;
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.onRaise.AddListener(() => ++called);
			eventHandle.Raise();

			Assert.AreEqual(1, called);
		}

		[UnityTest]
		public IEnumerator NoOnRaiseOnInactiveGameObject()
		{
			var called = 0;
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.onRaise.AddListener(() => ++called);

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.gameObject.SetActive(false);

			yield return new WaitForEndOfFrame();

			eventHandle.Raise();

			Assert.AreEqual(0, called);
		}

		[UnityTest]
		public IEnumerator NoOnRaiseWhenInactive()
		{
			var called = 0;
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.onRaise.AddListener(() => ++called);

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.enabled = false;

			yield return new WaitForEndOfFrame();

			eventHandle.Raise();

			Assert.AreEqual(0, called);
		}

		[UnityTest]
		public IEnumerator RemoveOnDestroy()
		{
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;

			yield return new WaitForEndOfFrame();

			Object.Destroy(eventListenerCtrl.gameObject);

			yield return new WaitForEndOfFrame();

			Assert.DoesNotThrow(() => eventHandle.Raise());
		}

		[UnityTest]
		public IEnumerator OnRaiseAfterReenable()
		{
			var called = 0;
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.onRaise.AddListener(() => ++called);

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.enabled = false;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.enabled = true;

			yield return new WaitForEndOfFrame();

			eventHandle.Raise();

			Assert.AreEqual(1, called);
		}

		[UnityTest]
		public IEnumerator OnRaiseAfterUnityEventReinitialization()
		{
			var called = 0;
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.onRaise.AddListener(() => ++called);
			eventListenerCtrl.onRaise = new UnityEvent();
			eventHandle.Raise();

			Assert.AreEqual(0, called);
		}

		[UnityTest]
		public IEnumerator OnRaiseAddOnlyOnce()
		{
			var called = 0;
			var eventListenerCtrl = new GameObject("listener")
				.AddComponent<EventListenerControler>();
			var eventHandle = ScriptableObject.CreateInstance<EventHandle>();
			eventListenerCtrl.eventHandle = eventHandle;
			eventListenerCtrl.enabled = false;

			yield return new WaitForEndOfFrame();

			eventListenerCtrl.enabled = true;

			yield return new WaitForEndOfFrame();  // Start() + OnEnable()

			eventListenerCtrl.onRaise.AddListener(() => ++called);
			eventHandle.Raise();

			Assert.AreEqual(1, called);
		}
	}
}
