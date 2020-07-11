using UnityEngine;
using System.Collections;
using UnityEditor;

namespace NiftyFramework.ScreenInput
{
	public class ScreenInputController : MonoSingleton<ScreenInputController>
	{
		public delegate void InputUpdateHandler(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId =0);
		
		private float _input1DownTime = 0;
		private float _input2DownTime = 0;//length of time the mouse/finger is held down for.
		private float _inputStationaryTime = 0; //length of time the mouse/finger is stationary.
		
		private Vector3 _lastMousePosition; //used to track onInputMove/onInput stationary for the mouse

		public Camera mainCamera;


		public event InputUpdateHandler OnPrimaryInputStart;
		public event InputUpdateHandler OnSecondaryInputStart;
		public event InputUpdateHandler OnPrimaryInputMoved;
		public event InputUpdateHandler OnSecondaryInputMoved;
		public event InputUpdateHandler OnInputStationary;
		public event InputUpdateHandler OnPrimaryInputEnd;
		public event InputUpdateHandler OnSecondaryInputEnd;
		public event InputUpdateHandler OnInputCancel;

		public void Start()
		{
			mainCamera = Camera.main;
		}
		
		// Update is called once per frame
		public void Update()
		{
			int touchCount = Input.touchCount;
			Ray screenPointRay;
			if (touchCount > 0)
			{
				// Get movement of the finger since last frame
				Touch touchZero = Input.touches[0];
				Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchZero.position.x,
					touchZero.position.y, mainCamera.nearClipPlane));
				
				switch (touchZero.phase)
				{
					case TouchPhase.Began:
						_inputStationaryTime = 0;
						_input1DownTime = 0;
						
						if (OnPrimaryInputStart != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
							OnPrimaryInputStart(worldPosition, touchZero.deltaPosition, screenPointRay, 0, 0);
						}
						break;
					case TouchPhase.Moved:
						_inputStationaryTime = 0;
						_input1DownTime += Time.deltaTime;
						if (OnPrimaryInputMoved != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
							OnPrimaryInputMoved(worldPosition, touchZero.deltaPosition, screenPointRay, _input1DownTime,0);
						}
						break;
					case TouchPhase.Stationary:
						_inputStationaryTime += Time.deltaTime;
						_input1DownTime += Time.deltaTime;
						if (OnInputStationary != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
							OnInputStationary(worldPosition, touchZero.deltaPosition, screenPointRay, _input1DownTime,0);
						}
						break;
					case TouchPhase.Ended:
						_inputStationaryTime = 0;
						if (OnPrimaryInputEnd != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
							OnPrimaryInputEnd(worldPosition, touchZero.deltaPosition, screenPointRay, _input1DownTime,0);
						}
						_input1DownTime = 0;
						break;
					case TouchPhase.Canceled:
						_inputStationaryTime = 0;
						if (OnInputCancel != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(touchZero.position);
							OnInputCancel(worldPosition, touchZero.deltaPosition, screenPointRay, _input1DownTime,0);
						}
						_input1DownTime = 0;
						break;
				}
			}
			else if (Input.GetMouseButton(0))
			{
				Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
					Input.mousePosition.y, mainCamera.nearClipPlane));
				if (Input.GetMouseButtonDown(0))
				{
					_lastMousePosition = Input.mousePosition;
					if (OnPrimaryInputStart != null)
					{
						screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
						OnPrimaryInputStart(worldPosition, Vector2.zero, screenPointRay, _input1DownTime,0);
					}
				}
				else if (Input.GetMouseButton(0))
				{
					Vector2 mouseMoveDelta = _lastMousePosition - Input.mousePosition;
					_input1DownTime += Time.deltaTime;
					if (mouseMoveDelta.magnitude > 0.1f)
					{
						if (OnPrimaryInputMoved != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
							OnPrimaryInputMoved(worldPosition, mouseMoveDelta, screenPointRay, _input1DownTime,0);
						}
					}
					else
					{
						if (OnInputStationary != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
							OnInputStationary(worldPosition, mouseMoveDelta, screenPointRay, _input1DownTime,0);
						}
					}
				}
				
			}
			else if (Input.GetMouseButtonUp(0))
			{
				Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
					Input.mousePosition.y, mainCamera.nearClipPlane));
				_input1DownTime += Time.deltaTime;
				Vector2 mouseMoveDelta = _lastMousePosition - Input.mousePosition;
				if (OnPrimaryInputEnd != null)
				{
					screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					OnPrimaryInputEnd(worldPosition, mouseMoveDelta, screenPointRay, _input1DownTime,0);
				}
				_input1DownTime = 0;
			}
			
			if (Input.GetMouseButton(1))
			{
				Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
					Input.mousePosition.y, mainCamera.nearClipPlane));
				if (Input.GetMouseButtonDown(1))
				{
					_lastMousePosition = Input.mousePosition;
					if (OnSecondaryInputStart != null)
					{
						screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
						OnSecondaryInputStart(worldPosition, Vector2.zero, screenPointRay, _input2DownTime,1);
					}
				}
				else if (Input.GetMouseButton(1))
				{
					Vector2 mouseMoveDelta = _lastMousePosition - Input.mousePosition;
					_input2DownTime += Time.deltaTime;
					if (mouseMoveDelta.magnitude > 0.1f)
					{
						if (OnSecondaryInputMoved != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
							OnSecondaryInputMoved(worldPosition, mouseMoveDelta, screenPointRay, _input2DownTime,1);
						}
					}
					else
					{
						if (OnSecondaryInputMoved != null)
						{
							screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
							OnSecondaryInputMoved(worldPosition, mouseMoveDelta, screenPointRay, _input2DownTime,1);
						}
					}
				}
			}
			else if (Input.GetMouseButtonUp(1))
			{
				Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
					Input.mousePosition.y, mainCamera.nearClipPlane));
				_input2DownTime += Time.deltaTime;
				Vector2 mouseMoveDelta = _lastMousePosition - Input.mousePosition;
				if (OnSecondaryInputEnd != null)
				{
					screenPointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					OnSecondaryInputEnd(worldPosition, mouseMoveDelta, screenPointRay, _input2DownTime,1);
				}
				_input2DownTime = 0;
			}
		}
	}
}