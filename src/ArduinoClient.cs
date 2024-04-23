using System;
using System.IO;
using System.IO.Ports;

namespace Arduino.Sharp
{
    /// <summary>
    /// The base client of communication
    /// </summary>
    public class ArduinoClient
    {
        /// <summary>
        /// The configured portname we want to communicate to
        /// </summary>
        public string PortName => IsConnected ? m_serialPort.PortName : string.Empty;
        /// <summary>
        /// The configured baudrate we're expecting
        /// </summary>
        public int BaudRate => IsConnected ? m_serialPort.BaudRate : -1;
        /// <summary>
        /// If the communication to the port is open or closed.
        /// </summary>
        public bool IsConnected => m_serialPort.IsOpen;

        /// <summary>
        /// Called whenever we receive data on the configured port.
        /// </summary>
        public event EventHandler<string> DataReceived;

        private readonly SerialPort m_serialPort;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <exception cref="IOException"></exception>
        public ArduinoClient(string portName, int baudRate)
        {
            try
            {
                m_serialPort = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate
                };
                m_serialPort.Open();

                m_serialPort.DataReceived += SerialPortOnDataReceived; 
            }
            catch (FileNotFoundException ex)
            {
                throw new IOException("Could not establish connection with port " + portName, ex.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception"></exception>
        public void SendData(string message)
        {
            if (IsConnected)
                m_serialPort.Write(message);
            else
                throw new Exception("Not connected"); // TODO Create custom exception for connectivity issues
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <exception cref="Exception"></exception>
        public void SendData(byte[] buffer, int offset, int count)
        {
            if(IsConnected)
                m_serialPort.Write(buffer, offset, count);
            else
                throw new Exception("Not connected"); // TODO Create custom exception for connectivity issues
        }
        
        
        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = m_serialPort.ReadExisting();
            
            DataReceived?.Invoke(sender, data);
        }
    }
}