using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class BluetoothLEHeartrate : MonoBehaviour
{
    float time = 4;
    bool shouldScan = false;
    bool waitingToInit = false;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE_WIN
            Destroy(this);
#endif
#if UNITY_EDITOR_WIN
        Destroy(this);
#endif
        Initialize();
    }

    void Initialize()
    {
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            Debug.Log("Bluetooth initialized successfully");
            ScanForDevices();
            shouldScan = true;
        }, (error) =>
        {
            Debug.LogError("Bluetooth initialization failed: " + error);
            if (!waitingToInit)
                StartCoroutine(WaitToInitialize());
        });
    }

    void ScanForDevices()
    {
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(new string[] { "180D" }, (address, name) =>
        {
            Debug.Log("Devices found!");
            Debug.Log($"address: {address} | name: {name}");
            ConnectToDevice(address);

        });
    }

    private void ConnectToDevice(string address)
    {
        BluetoothLEHardwareInterface.ConnectToPeripheral(address, PeripheralConnected, ServiceAction, CharacteristicAction, DisconnectAction);
    }

    private void DisconnectAction(string obj)
    {
        Debug.Log($"{obj} disconnected!");
    }

    private void CharacteristicAction(string deviceAddress, string serviceUUID, string characteristicUUID)
    {
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(deviceAddress, serviceUUID, characteristicUUID, DeviceNotificationAction, CharacteristicAction);
    }

    private void CharacteristicAction(string arg1, string arg2, byte[] arg3)
    {
        Debug.Log($"Characteristic Action: {arg1} | {arg2}");
        ProcessHeartRateData(arg3);
    }

    private void ProcessHeartRateData(byte[] data)
    {
        if (data.Length > 0)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Debug.Log($"Data at i: {data[i]}");
            }
            int heartRate = data[1];
            Debug.Log($"Heart Rate: {heartRate}");
        }
    }

    private void DeviceNotificationAction(string arg1, string arg2)
    {
        Debug.Log($"Device Notification Action: {arg1} | {arg2}");
    }

    private void ServiceAction(string arg1, string arg2)
    {
        Debug.Log($"Service Action: {arg1} | {arg2}");
    }

    private void PeripheralConnected(string obj)
    {
        Debug.Log($"{obj} connected!");
        shouldScan = false;
    }

    IEnumerator WaitToInitialize()
    {
        waitingToInit = true;
        yield return new WaitForSeconds(5);
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldScan)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                ScanForDevices();
                time = 4;
            }
        }
    }
}
