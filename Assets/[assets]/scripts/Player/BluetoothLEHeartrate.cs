using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class BluetoothLEHeartrate : MonoBehaviour
{
    static BluetoothLEHeartrate instance;
    float time = 4;
    bool shouldScan = false;
    bool waitingToInit = false;
    bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) Destroy(gameObject);

#if UNITY_STANDALONE_WIN
            Destroy(gameObject);
#endif
#if UNITY_EDITOR_WIN
        Destroy(gameObject);
#endif
        Initialize();
    }

    void Initialize()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            if (initialized) return;
            initialized = true;
            Debug.Log("Bluetooth initialized successfully!!!!!!!!");
            ScanForDevices();
            shouldScan = true;
        }, (error) =>
        {
            if (initialized) return;
            Debug.LogError("Bluetooth initialization failed: " + error);
            if (!waitingToInit)
                StartCoroutine(WaitToInitialize());
        });
    }

    void ScanForDevices()
    {

        Debug.Log("Looking for devices...");
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(new string[] { "180D" }, (address, name) =>
        {
            Debug.Log("Devices found!");
            Debug.Log($"address: {address} | name: {name}");
            ConnectToDevice(address);

        });
    }
    private List<string> devicesConnected = new List<string>();

    private void ConnectToDevice(string address)
    {
        if (devicesConnected.Contains(address)) return;
        BluetoothLEHardwareInterface.ConnectToPeripheral(address, PeripheralConnected, ServiceAction, CharacteristicAction, DisconnectAction);
    }

    private void DisconnectAction(string address)
    {
        devicesConnected.Remove(address);
        Debug.Log($"{address} disconnected!");
    }

    private void CharacteristicAction(string deviceAddress, string serviceUUID, string characteristicUUID)
    {
        Debug.Log($"Characteristic Action: Device Address: {deviceAddress} | Service UUID: {serviceUUID} | Characteristic UUID: {characteristicUUID}");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(deviceAddress, serviceUUID, characteristicUUID, DeviceNotificationAction, CharacteristicAction);
    }

    private void CharacteristicAction(string arg1, string arg2, byte[] arg3)
    {
        Debug.Log($"Characteristic Action: {arg1} | {arg2}");
        if (arg2 == "00002a37-0000-1000-8000-00805f9b34fb")
            ProcessHeartRateData(arg3);
    }

    private void ProcessHeartRateData(byte[] data)
    {
        if (data.Length > 0)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Debug.Log($"Data at {i}: {data[i]}");
            }
            // int heartRate = data[1];
            // Debug.Log($"Heart Rate: {heartRate}");
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

    private void PeripheralConnected(string address)
    {

        devicesConnected.Add(address);
        Debug.Log($"{address} connected!");
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
        if (devicesConnected.Count == 0)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                ScanForDevices();
                time = 4;
            }
        }
        else time = 4;
    }
}
