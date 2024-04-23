using System;

namespace Arduino.Sharp
{
    /// <summary>
    /// Builder class for setting up the main client.
    /// </summary>
    public class ArduinoClientBuilder
    {
        private int m_baudRate;
        /// <summary>
        /// Sets the baudrate for communication to the board.
        /// </summary>
        /// <param name="baudRate">The baudrate</param>
        /// <example>9600</example>
        /// <returns>The client builder instance</returns>
        public ArduinoClientBuilder SetBaudRate(int baudRate)
        {
            m_baudRate = baudRate;
            return this;
        }

        private string m_portName = string.Empty;
        /// <summary>
        /// Sets the name of the serial port on the computer that is connected to the board. 
        /// </summary>
        /// <param name="portName">The name of the COM-port</param>
        /// <example>COM4</example>
        /// <returns>The client builder instance</returns>
        public ArduinoClientBuilder SetPortName(string portName)
        {
            m_portName = portName;
            return this;
        }

        /// <summary>
        /// Builds the ArduinoClient with specified settings
        /// </summary>
        /// <returns>Built <see cref="ArduinoClient"/> with the settings specified</returns>
        /// <exception cref="ArgumentException">If required settings have not been set</exception>
        public ArduinoClient Build()
        {
            if (m_baudRate == 0 || m_portName == string.Empty)
                throw new ArgumentException("Not enough settings passed");
            
            return new ArduinoClient(m_portName, m_baudRate);
        }
    }
}