using System;
using System.IO.Ports;
using Arduino.Sharp.Exceptions;

namespace Arduino.Sharp.Entities
{
    public class Connection
    {
        private readonly SerialPort m_serialPort;
        
        /// <summary>
        /// If the communication to the port is open or closed.
        /// </summary>
        public bool IsConnected => m_serialPort.IsOpen;
        /// <summary>
        /// The configured portname we want to communicate to
        /// </summary>
        public string PortName => IsConnected ? m_serialPort.PortName : string.Empty;
        /// <summary>
        /// The configured baudrate we're expecting
        /// </summary>
        public int BaudRate => IsConnected ? m_serialPort.BaudRate : -1;
        
        internal Connection(SerialPort serialPort)
        {
            m_serialPort = serialPort;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception"></exception>
        public void SendData(string message)
        {
            if (m_serialPort.IsOpen)
                m_serialPort.Write(message);
            else
                throw
                    new ConnectivityException("Not connected to a port");
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
            if(m_serialPort.IsOpen)
                m_serialPort.Write(buffer, offset, count);
            else
                throw new ConnectivityException("Not connected");
        }
    }
}