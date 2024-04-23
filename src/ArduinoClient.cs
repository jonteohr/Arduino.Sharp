using System;
using System.IO;
using System.IO.Ports;
using Arduino.Sharp.Entities;
using Arduino.Sharp.Exceptions;

namespace Arduino.Sharp
{
    /// <summary>
    /// The base client of communication
    /// </summary>
    public class ArduinoClient : IDisposable
    {
        public Connection Connection { get; private set; }

        /// <summary>
        /// Called whenever we receive data on the configured port.
        /// </summary>
        public event EventHandler<string> DataReceived;

        private readonly SerialPort m_serialPort;
        
        /// <summary>
        /// Main constructor of the client.
        /// </summary>
        /// <param name="portName">Name of the port used to the arduino. Ex: COM4</param>
        /// <param name="baudRate">The baudrate used to communicate with the board.</param>
        /// <param name="leonardo">Enable leonardo support?</param>
        /// <exception cref="ConnectivityException">Not able to find the port.</exception>
        public ArduinoClient(string portName, int baudRate, bool leonardo)
        {
            try
            {
                m_serialPort = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate
                };

                if (leonardo) // Enable DTR and RTS if we want support for Leonardo boards
                {
                    m_serialPort.DtrEnable = true;
                    m_serialPort.RtsEnable = true;
                }

                m_serialPort.DataReceived += SerialPortOnDataReceived;
            }
            catch (FileNotFoundException ex)
            {
                throw new ConnectivityException("Could not establish connection with port " + portName, ex.InnerException);
            }
        }

        /// <summary>
        /// Connect the client to the set port.
        /// </summary>
        public Connection Connect()
        {
            m_serialPort.Open();
            
            return Connection = new Connection(m_serialPort);
        }
        
        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = m_serialPort.ReadExisting();
            
            DataReceived?.Invoke(sender, data);
        }

        public void Dispose()
        {
            m_serialPort?.Dispose();
            Connection = null;
        }
    }
}