using System;
using System.Collections.Generic;
using System.Management;
using System.IO.Ports;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace CSharpErgoBoard
{
    public partial class id_main : Form
    {
        // Base variables
        private String m_selectedKey = "NONE";
        private Logging m_logger = new Logging();
        private SystemMonitor m_monitor = new SystemMonitor();

        // Configuration Variables. 
        //TODO have this written to a text document and read it post shutdown. 

        // Operational variables 
        private SerialPort m_leftKeyboard = new SerialPort();
        private Queue<String> m_leftKeyBoardQueue = new Queue<string>();    // Ouput Queue
        private SerialPort m_leftLeds = new SerialPort();
        private Queue<String> m_leftLedsQueue = new Queue<string>();    // Output Queue
        private SerialPort m_rightKeyboard = new SerialPort();
        private Queue<String> m_rightKeyBoardQueue = new Queue<string>();   // Output Queue
        private SerialPort m_rightLeds = new SerialPort();
        private Queue<String> m_rightLedsQueue = new Queue<string>();   // Output Queue

        /// <summary>
        /// Default constructor.
        /// </summary>
        public id_main()
        {
            m_logger.Log("Starting finding the avaible ports");
            GetPorts();

            m_logger.Log("Starting the program. ");
            InitializeComponent();

            m_logger.Log("by default start in light mode");
            SelectLightMode();
        }

        /// <summary>
        /// Finds a list of serial ports and their friendly descriptions and returns it. 
        /// </summary>
        /// <returns> A list of strings housing the COM ports and their friendly descriptions. </returns>
        private List<String> GetPorts()
        {
            List<String> names = new List<string>();
            ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_SerialPort");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                names.Add(queryObj["Name"].ToString());
                m_logger.Log(queryObj["Name"].ToString());
            }

            return names;
        }

        /// <summary>
        /// Changes the color layout of the application to a dark mode style. 
        /// </summary>
        private void SelectDarkMode()
        {
            m_logger.Log("Selected Dark Mode");

            Color kindofBlack = new Color();
            Color backgroundBlack = new Color();
            kindofBlack = Color.FromArgb(255, 50, 50, 50);
            backgroundBlack = Color.FromArgb(255, 20, 20, 20);

            this.id_panelMain.BackColor = backgroundBlack;
            this.id_panelMain.ForeColor = System.Drawing.Color.White;
            this.menuStrip1.BackColor = System.Drawing.Color.DimGray;
            this.menuStrip1.ForeColor = System.Drawing.Color.White;
            this.BackColor = backgroundBlack;
            
            this.id_buttonLeftKeyConnectComPort.BackColor = kindofBlack;
            this.id_buttonLeftLayerUpdate.BackColor = kindofBlack;
            this.id_buttonRightKeyConnectComPort.BackColor = kindofBlack;
            this.id_buttonRightLayerUpdate.BackColor = kindofBlack;
            this.id_buttonLeftUpdateKey.BackColor = kindofBlack;
            this.id_buttonRightUpdateKey.BackColor = kindofBlack;
            // Left side
            // Column 1
            this.id_buttonLeftR1C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonLeftR2C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonLeftR3C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonLeftR4C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonLeftR5C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonLeftR6C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            // Column 2
            this.id_buttonLeftR1C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR2C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR3C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 3
            this.id_buttonLeftR1C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR2C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR3C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 4
            this.id_buttonLeftR1C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR2C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR3C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 5
            this.id_buttonLeftR1C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR2C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR3C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 6
            this.id_buttonLeftR1C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR2C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR3C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C6.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyDark;
            // Column 7
            this.id_buttonLeftR1C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR2C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR3C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C7.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyDark;
            // Column 8
            this.id_buttonLeftR3C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR4C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR5C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonLeftR6C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;


            // Right side
            // Column 1
            this.id_buttonRightR1C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonRightR2C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonRightR3C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonRightR4C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonRightR5C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            this.id_buttonRightR6C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyDark;
            // Column 2
            this.id_buttonRightR1C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR2C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR3C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 3
            this.id_buttonRightR1C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR2C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR3C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 4
            this.id_buttonRightR1C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR2C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR3C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 5
            this.id_buttonRightR1C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR2C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR3C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            // Column 6
            this.id_buttonRightR1C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR2C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR3C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C6.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyDark;
            // Column 7
            this.id_buttonRightR1C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR2C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR3C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C7.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyDark;
            // Column 8
            this.id_buttonRightR3C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR4C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR5C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
            this.id_buttonRightR6C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyDark;
        }

        /// <summary>
        /// Changes the color layout of the application to a light mode style
        /// </summary>
        private void SelectLightMode()
        {
            m_logger.Log("Selected Light Mode");

            // Main Window 
            this.id_panelMain.BackColor = System.Drawing.Color.White;
            this.id_panelMain.ForeColor = System.Drawing.Color.Black;
            this.menuStrip1.BackColor = System.Drawing.Color.Gainsboro;
            this.menuStrip1.ForeColor = System.Drawing.Color.Black;
            this.ForeColor = System.Drawing.Color.Black;
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            
            this.id_buttonLeftKeyConnectComPort.BackColor = System.Drawing.Color.WhiteSmoke;
            this.id_buttonLeftLayerUpdate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.id_buttonRightKeyConnectComPort.BackColor = System.Drawing.Color.WhiteSmoke;
            this.id_buttonRightLayerUpdate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.id_buttonLeftUpdateKey.BackColor = System.Drawing.Color.WhiteSmoke;
            this.id_buttonRightUpdateKey.BackColor = System.Drawing.Color.WhiteSmoke;

            // Left hand side
            // Column 1
            this.id_buttonLeftR1C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonLeftR2C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonLeftR3C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonLeftR4C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonLeftR5C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonLeftR6C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            // Column 2
            this.id_buttonLeftR1C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR2C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR3C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 3
            this.id_buttonLeftR1C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR2C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR3C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 4
            this.id_buttonLeftR1C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR2C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR3C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 5
            this.id_buttonLeftR1C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR2C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR3C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 6
            this.id_buttonLeftR1C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR2C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR3C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C6.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyLight;
            // Column 7
            this.id_buttonLeftR1C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR2C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR3C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C7.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyLight;
            // Column 8
            this.id_buttonLeftR3C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR4C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR5C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonLeftR6C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;


            // Right side
            // Column 1
            this.id_buttonRightR1C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonRightR2C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonRightR3C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonRightR4C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonRightR5C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            this.id_buttonRightR6C1.Image = global::CSharpErgoBoard.Properties.Resources.WideKeyLight;
            // Column 2
            this.id_buttonRightR1C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR2C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR3C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C2.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 3
            this.id_buttonRightR1C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR2C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR3C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C3.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 4
            this.id_buttonRightR1C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR2C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR3C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C4.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 5
            this.id_buttonRightR1C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR2C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR3C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C5.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            // Column 6
            this.id_buttonRightR1C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR2C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR3C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C6.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C6.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyLight;
            // Column 7
            this.id_buttonRightR1C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR2C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR3C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C7.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C7.Image = global::CSharpErgoBoard.Properties.Resources.TallKeyLight;
            // Column 8
            this.id_buttonRightR3C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR4C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR5C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
            this.id_buttonRightR6C8.Image = global::CSharpErgoBoard.Properties.Resources.SingleKeyLight;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormClosing += Form1_FormClosing;
        }

        /// <summary>
        /// Quits the program using the menu button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void id_menuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Acts as a deafult destructor. Closes any open threads and does closing actions. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //leftKeyboard.End();
            m_monitor.End();
            m_logger.End();
        }

        /// <summary>
        /// When the user of the application chooses to enter dark mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void id_menuDarkMode_Click(object sender, EventArgs e)
        {
            SelectDarkMode();
        }

        /// <summary>
        /// When the user of the application chooses to enter light mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void id_menuLightMode_Click(object sender, EventArgs e)
        {
            SelectLightMode();
        }

        private void id_buttonLeftR1C1_Click(object sender, EventArgs e)
        {
            m_selectedKey = "LeftR1C1";
        }

        private void id_buttonLeftR1C2_Click(object sender, EventArgs e)
        {
            m_selectedKey = "LeftR1C1";
        }

    }
}
