using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    private InputDevice targetDevice;
    // This will appear as a prop in Unity and could be populated by drag-drop models into list
    private GameObject spawnedController;
    public List<GameObject> controllerPrefabs;
    // Start is called before the first frame update
    void Start()
    {

        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightController = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightController, devices);

        foreach (var device in devices)
        {
            Debug.Log(device.name + device.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            // Device name read from InputDevice must match the Model names added in Unity
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Did not find the controller model named: " + targetDevice.name);
                Debug.Log("Loading default controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
            Debug.Log("Pressing primary button: " + primaryButtonValue);

        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f) Debug.Log("Pressing trigger button: " + triggerValue);

        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisValue != Vector2.zero) Debug.Log("Primary joystick: " + primary2DAxisValue);
    }
}
