using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace CSharpErgoBoard
{
    /// <summary>
    /// A class with its own dedicated thread. Used for Processing serial IO between the keyboard and the computer.
    /// </summary>
    class IOConnect
    {
        // Constants
        const int m_maxBufferSize = 255;

        SerialPort m_serialPort;
        Boolean m_initalized = false;
        int m_baudRate = 9600;
        int m_dataBits = 8;
        Handshake m_handshake = Handshake.None; // This can cause issues on Arduino devices if enabled
        Parity m_pairty = Parity.None;
        StopBits m_stopBits = StopBits.One;
        int m_timeout = 500;
        Queue m_output;
        Queue m_input;
        Mutex m_outputLock = new Mutex();
        Mutex m_inputLock = new Mutex();

        /// <summary>
        /// Starts a serial port instance. 
        /// </summary>
        /// <param name="comPort"></param>
        /// <returns>True once the port was setup.</returns>
        public Boolean Setup(String comPort)
        {
            //m_serialPort.StopBits;
            m_serialPort.PortName = comPort;
            m_serialPort.BaudRate = m_baudRate;
            m_serialPort.Parity = m_pairty;
            m_serialPort.DataBits = m_dataBits;
            m_serialPort.StopBits = m_stopBits;
            m_serialPort.Handshake = m_handshake;

            m_serialPort.ReadTimeout = m_timeout;
            m_serialPort.WriteTimeout = m_timeout;

            m_serialPort.Open();
            m_initalized = m_serialPort.IsOpen;
            Thread m_thread = new Thread(DataStream);


            return m_initalized;
        }

        public void DataStream()
        {
            while (m_initalized)
            {
                try
                {
                    String readMessage = m_serialPort.ReadLine();
                    if (readMessage.Count() > 0)
                    {
                        m_inputLock.WaitOne();
                        m_input.Enqueue(readMessage);
                        m_inputLock.ReleaseMutex();
                    }
                    
                    m_outputLock.WaitOne();
                    String writeMessage = m_output.Dequeue().ToString();
                    m_outputLock.ReleaseMutex();
                    if (writeMessage.Count() > 0)
                    {
                        m_serialPort.WriteLine(writeMessage);
                    }
                }
                catch (TimeoutException)
                {
                    ;
                }
            }
        }

        /// <summary>
        /// Adds a System::String to the output Queue. 
        /// </summary>
        /// <param name="writtenLine"></param>
        /// <returns>True if we add a string to queue, false if the string is too short or too long.</returns>
        public bool WriteString(String writtenLine)
        {
            // Line is too big
            if (writtenLine.Length > m_maxBufferSize)
            {
                return false;
            }
            // The line is empty
            if (writtenLine.Length == 0)
            {
                return false;
            }

            m_outputLock.WaitOne();
            m_output.Enqueue(writtenLine);
            m_outputLock.ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Finds the COM ports and the friendly description of the serial port
        /// </summary>
        /// <returns> A list of the serial ports and their friendly description in a single string format. </returns>
        public List<String> GetPorts()
        {
            List<String> names = new List<string>();
            ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_SerialPort");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                names.Add(queryObj["Name"].ToString());
            }

            return names;
        }


    }
}
