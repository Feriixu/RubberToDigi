using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Diagnostics;

namespace RubberToDigi
{
    public partial class RubberToDigi : Form
    {
        private List<string> convertedCommands;
        private List<string> reducedStrings;
        private string convertedScript;
        private bool useDefaultDelay;
        private bool useReducedStrings;
        private int reducedStringsMaxLength;
        private int defaultDelay;
        private bool arduinoPathFound;
        private string arduinoPath;
        

        public RubberToDigi()
        {
            InitializeComponent();
            arduinoPathFound = false;
            useDefaultDelay = false;
            useReducedStrings = false;
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogSelectRubber.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(openFileDialogSelectRubber.FileName);
                textBoxSelectedFile.Text = openFileDialogSelectRubber.FileName;

                string text = System.IO.File.ReadAllText(openFileDialogSelectRubber.FileName);
                string[] lines = System.IO.File.ReadAllLines(openFileDialogSelectRubber.FileName);
                buttonConvert.Enabled = true;
            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            convertedCommands = new List<string>();
            reducedStrings = new List<string>();
            string text = System.IO.File.ReadAllText(openFileDialogSelectRubber.FileName);
            string[] lines = System.IO.File.ReadAllLines(openFileDialogSelectRubber.FileName);

            for (int lineNr = 0; lineNr < lines.Length; lineNr++)
            {
                string line = lines[lineNr];

                //string convertedLine = textBoxHeaderName.Text + ".";
                string convertedLine = "";
                string[] splitLine = line.Split(Convert.ToChar(" "));

                try
                {
                    //Convert the Line
                    switch (splitLine[0])
                    {
                        //Comment
                        case "REM":
                            break;

                        //Set default delay
                        case "DEFAULTDELAY":
                            useDefaultDelay = true;
                            defaultDelay = Convert.ToInt32(splitLine[1]);
                            break;
                        case "DEFAULT_DELAY":
                            useDefaultDelay = true;
                            defaultDelay = Convert.ToInt32(splitLine[1]);
                            break;

                        //Delay
                        case "DELAY":
                            convertedLine += string.Format("waitFor({0});", splitLine[1]);
                            break;

                        //Print something
                        case "STRING":
                            useReducedStrings = true;

                            string convertedString = line.Substring(7).Replace("\\", "\\\\").Replace("\"", "\\\"");

                            reducedStrings.Add("const char L" + lineNr.ToString() + "[] PROGMEM = \"" + convertedString + "\";");

                            if (convertedString.Length > reducedStringsMaxLength)
                            {
                                reducedStringsMaxLength = convertedString.Length;
                            }

                            if (lines[lineNr + 1].Split(Convert.ToChar(" "))[0] == "ENTER")
                            {
                                convertedLine += string.Format("sendLine({0});", "GetPsz (L" + lineNr + ")");
                                lineNr++;
                            }
                            else
                            {
                                convertedLine += string.Format("sendKeys({0});", "GetPsz (L" + lineNr + ")");
                            }
                            break;

                        //GUI Key
                        case "WINDOWS":
                            if (splitLine.Length > 1)
                                convertedLine += string.Format("sendModKey({0} , MOD_GUI_LEFT);", "KEY_" + splitLine[1].ToUpper());
                            else
                                convertedLine += "sendKey(MOD_GUI_LEFT);";
                            break;
                        case "GUI":
                            if (splitLine.Length > 1)
                                convertedLine += string.Format("sendModKey({0} , MOD_GUI_LEFT);", "KEY_" + splitLine[1].ToUpper());
                            else
                                convertedLine += "sendKey(MOD_GUI_LEFT);";
                            break;

                        //Menu key
                        case "APP":
                            convertedLine += "sendModKey(KEY_F10, MOD_SHIFT_LEFT);";
                            break;
                        case "MENU":
                            convertedLine += "sendModKey(KEY_F10, MOD_SHIFT_LEFT);";
                            break;

                        //Shift key
                        case "SHIFT":
                            convertedLine += "sendModKey(";
                            switch (splitLine[1])
                            {
                                case "DELETE":
                                    convertedLine += "KEY_DELETE";
                                    break;
                                case "HOME":
                                    convertedLine += "KEY_HOME";
                                    break;
                                case "INSERT":
                                    convertedLine += "KEY_INSERT";
                                    break;
                                case "PAGEUP":
                                    convertedLine += "KEY_PAGE_UP";
                                    break;
                                case "PAGEDOWN":
                                    convertedLine += "KEY_PAGE_DOWN";
                                    break;
                                case "WINDOWS":
                                    convertedLine += "MOD_GUI_LEFT";
                                    break;
                                case "GUI":
                                    convertedLine += "MOD_GUI_LEFT";
                                    break;
                                case "UPARROW":
                                    convertedLine += "KEY_UP_ARROW";
                                    break;
                                case "DOWNARROW":
                                    convertedLine += "KEY_DOWN_ARROW";
                                    break;
                                case "LEFTARROW":
                                    convertedLine += "KEY_LEFT_ARROW";
                                    break;
                                case "RIGHTARROW":
                                    convertedLine += "KEY_RIGHT_ARROW";
                                    break;
                                case "TAB":
                                    convertedLine += "KEY_TAB";
                                    break;
                                default:
                                    break;
                            }
                            convertedLine += ", MOD_SHIFT_LEFT);";
                            break;

                        //Alt key
                        case "ALT":
                            convertedLine += "sendModKey(KEY_" + splitLine[1].ToUpper() + ", MOD_ALT_LEFT);";
                            break;

                        //Control key
                        case "CONTROL":
                            convertedLine += "sendModKey(";
                            //Handle keys
                            switch (splitLine[splitLine.Length - 1])
                            {
                                case "BREAK":
                                    convertedLine += "KEY_PAUSE, ";
                                    break;
                                case "ESC":
                                    convertedLine += "KEY_ESCAPE, ";
                                    break;
                                case "SHIFT":
                                    convertedLine += "MOD_SHIFT_LEFT, ";
                                    break;
                                case "ALT":
                                    convertedLine += "MOD_ALT_LEFT, ";
                                    break;
                                default:
                                    convertedLine += "KEY_" + splitLine[splitLine.Length - 1].ToUpper() + ", ";
                                    break;
                            }

                            //Handle Modifiers
                            if (splitLine.Length > 2) //Only if there are Modifiers
                            {
                                switch (splitLine[1])
                                {
                                    case "SHIFT":
                                        convertedLine += "MOD_SHIFT_LEFT + ";
                                        break;
                                    case "ALT":
                                        convertedLine += "MOD_ALT_LEFT + ";
                                        break;
                                    default:
                                        convertedLine += "KEY_" + splitLine[splitLine.Length].ToUpper() + " + ";
                                        break;
                                }
                            }
                            convertedLine += "MOD_CONTROL_LEFT);";
                            break;
                        case "CTRL":
                            convertedLine += "sendModKey(";
                            //Handle keys
                            switch (splitLine[splitLine.Length - 1])
                            {
                                case "BREAK":
                                    convertedLine += "KEY_PAUSE, ";
                                    break;
                                case "ESC":
                                    convertedLine += "KEY_ESCAPE, ";
                                    break;
                                case "SHIFT":
                                    convertedLine += "MOD_SHIFT_LEFT, ";
                                    break;
                                case "ALT":
                                    convertedLine += "MOD_ALT_LEFT, ";
                                    break;
                                default:
                                    convertedLine += "KEY_" + splitLine[splitLine.Length - 1].ToUpper() + ", ";
                                    break;
                            }

                            //Handle Modifiers
                            if (splitLine.Length > 2) //Only if there are Modifiers
                            {
                                switch (splitLine[1])
                                {
                                    case "SHIFT":
                                        convertedLine += "MOD_SHIFT_LEFT + ";
                                        break;
                                    case "ALT":
                                        convertedLine += "MOD_ALT_LEFT + ";
                                        break;
                                    default:
                                        convertedLine += "KEY_" + splitLine[splitLine.Length].ToUpper() + " + ";
                                        break;
                                }
                            }
                            convertedLine += "MOD_CONTROL_LEFT);";
                            break;
                        case "CONTROL-SHIFT":
                            convertedLine += "sendModKey(";
                            //Handle keys
                            switch (splitLine[1])
                            {
                                case "BREAK":
                                    convertedLine += "KEY_PAUSE, ";
                                    break;
                                case "ESC":
                                    convertedLine += "KEY_ESCAPE, ";
                                    break;
                                case "SHIFT":
                                    convertedLine += "MOD_SHIFT_LEFT, ";
                                    break;
                                case "ALT":
                                    convertedLine += "MOD_ALT_LEFT, ";
                                    break;
                                default:
                                    convertedLine += "KEY_" + splitLine[1].ToUpper() + ", ";
                                    break;
                            }
                            convertedLine += "MOD_CONTROL_LEFT + MOD_SHIFT_LEFT);";
                            break;
                        case "CTRL-SHIFT":
                            convertedLine += "sendModKey(";
                            //Handle keys
                            switch (splitLine[1])
                            {
                                case "BREAK":
                                    convertedLine += "KEY_PAUSE, ";
                                    break;
                                case "ESC":
                                    convertedLine += "KEY_ESCAPE, ";
                                    break;
                                case "SHIFT":
                                    convertedLine += "MOD_SHIFT_LEFT, ";
                                    break;
                                case "ALT":
                                    convertedLine += "MOD_ALT_LEFT, ";
                                    break;
                                default:
                                    convertedLine += "KEY_" + splitLine[1].ToUpper() + ", ";
                                    break;
                            }
                            convertedLine += "MOD_CONTROL_LEFT + MOD_SHIFT_LEFT);";
                            break;



                        //Arrow Keys
                        case "LEFT":
                            convertedLine += "sendKey(KEY_LEFT_ARROW);";
                            break;
                        case "RIGHT":
                            convertedLine += "sendKey(KEY_RIGHT_ARROW);";
                            break;
                        case "UP":
                            convertedLine += "sendKey(KEY_UP_ARROW);";
                            break;
                        case "DOWN":
                            convertedLine += "sendKey(KEY_DOWN_ARROW);";
                            break;
                        case "LEFTARROW":
                            convertedLine += "sendKey(KEY_LEFT_ARROW);";
                            break;
                        case "RIGHTARROW":
                            convertedLine += "sendKey(KEY_RIGHT_ARROW);";
                            break;
                        case "UPARROW":
                            convertedLine += "sendKey(KEY_UP_ARROW);";
                            break;
                        case "DOWNARROW":
                            convertedLine += "sendKey(KEY_DOWN_ARROW);";
                            break;
                        case "SPACE":
                            convertedLine += "sendKey(KEY_SPACE);";
                            break;
                        case "ENTER":
                            convertedLine += "sendKey(KEY_ENTER);";
                            break;
                        default:
                            break;
                    } //Switch End
                }
                catch
                {
                    MessageBox.Show("Error while converting!\nMaybe your Ducky Script is faulty?", "Compile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                //If a command was converted
                if ((splitLine[0] != "DEFAULTDELAY") && (splitLine[0] != "DEFAULT_DELAY") && (splitLine[0] != "REM"))
                {
                    //Add the converted Line to the List
                    convertedCommands.Add(convertedLine);
                    //Add the converted Line to the Log Box
                    feedAppend(line + "\n" + convertedLine + "\n");
                }
            } //For End

            convertedScript = "";
            convertedScript += "#include <" + textBoxHeaderName.Text + ".h>\n"; //Add the right Header Name for Keyboard usage
            if (useReducedStrings) //If STRING was used
            {
                convertedScript += "#include <avr/pgmspace.h>\n\n"; //Add the Header for the reducedStrings

                foreach (string reducedString in reducedStrings) //Add all reduced Strings to the Script
                {
                    convertedScript += reducedString + "\n";
                    feedAppend(reducedString);
                }

                convertedScript += "\nchar buffer[" + (reducedStringsMaxLength + 1) + "];\n"; //Add a buffer variable
                convertedScript += "#define GetPsz( x ) (strcpy_P(buffer, (char*)x))\n\n"; //Add the Function for access to the reduced Strings
            }
            else //If no STRING was used
                convertedScript += "\n"; //Add another Spacer
            if (useDefaultDelay)
            {
                convertedScript += "void waitFor( int d ) {" + textBoxHeaderName.Text + ".delay(d); waitFor(" + defaultDelay + ");}\n";
                convertedScript += "void sendModKey( int key, int mod ) { " + textBoxHeaderName.Text + ".sendKeyStroke(key, mod); " + textBoxHeaderName.Text + ".update(); waitFor(" + defaultDelay + ");}\n";
                convertedScript += "void sendKey( int key ) {  " + textBoxHeaderName.Text + ".sendKeyStroke(key); " + textBoxHeaderName.Text + ".update(); waitFor(" + defaultDelay + ");}\n";
                convertedScript += "void sendKeys(char t[]) {" + textBoxHeaderName.Text + ".print(t); " + textBoxHeaderName.Text + ".update(); waitFor(" + defaultDelay + ");}\n";
                convertedScript += "void sendLine(char t[]) {" + textBoxHeaderName.Text + ".print(t); " + textBoxHeaderName.Text + ".update(); sendKey(KEY_ENTER); waitFor(" + defaultDelay + ");}\n\n";
            }
            else
            {
                convertedScript += "void waitFor( int d ) {" + textBoxHeaderName.Text + ".delay(d);}\n";
                convertedScript += "void sendModKey( int key, int mod ) { " + textBoxHeaderName.Text + ".sendKeyStroke(key, mod); " + textBoxHeaderName.Text + ".update(); }\n";
                convertedScript += "void sendKey( int key ) {  " + textBoxHeaderName.Text + ".sendKeyStroke(key); " + textBoxHeaderName.Text + ".update(); }\n";
                convertedScript += "void sendKeys(char t[]) {" + textBoxHeaderName.Text + ".print(t); " + textBoxHeaderName.Text + ".update();}\n";
                convertedScript += "void sendLine(char t[]) {" + textBoxHeaderName.Text + ".print(t); " + textBoxHeaderName.Text + ".update(); sendKey(KEY_ENTER);}\n\n";
            }
            

            convertedScript += "void setup() \n{\n"; //Open the Setup function
            foreach (string command in convertedCommands) //Add all the Commands
            {
                convertedScript += "\t" + command + "\n";
            }
            convertedScript += "}\n\n"; //Close the Setup function
            convertedScript += "void loop() { }"; // Add the loop function
        }//Convert End

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            saveFile();
        }
        private void buttonOpenInIDE_Click(object sender, EventArgs e)
        {
            saveFile();

            string myDirectory = Path.GetDirectoryName(openFileDialogSelectRubber.FileName);
            string myFileName = Path.GetFileNameWithoutExtension(openFileDialogSelectRubber.FileName);

            if (!arduinoPathFound)
            {
                MessageBox.Show("Please Select installation Directory of your Arduino IDE.\nThe folder is named \"Arduino\".", "Select Install Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult result = folderBrowserDialogArduinoDirectory.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (File.Exists(folderBrowserDialogArduinoDirectory.SelectedPath + @"\arduino.exe"))
                    {
                        arduinoPath = folderBrowserDialogArduinoDirectory.SelectedPath + @"\arduino.exe";
                        arduinoPathFound = true;
                    }
                }
            }

            if (arduinoPathFound)
            {
                MessageBox.Show("Arduino.exe found!");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = arduinoPath;
                startInfo.Arguments = myDirectory + "\\" + myFileName + "\\" + myFileName + ".ino";
                Process.Start(startInfo);
            }
        }
        private void saveFile()
        {
            string myDirectory = Path.GetDirectoryName(openFileDialogSelectRubber.FileName);
            string myFileName = Path.GetFileNameWithoutExtension(openFileDialogSelectRubber.FileName);

            if (File.Exists(myDirectory + "\\" + myFileName + "\\" + myFileName + ".ino")) //If the file already exists
            {
                DialogResult result = MessageBox.Show("The File already exists. Do you want to overwrite it?", "Already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(myDirectory + "\\" + myFileName + "\\" + myFileName + ".ino");
                        File.AppendAllText(myDirectory + "\\" + myFileName + "\\" + myFileName + ".ino", convertedScript);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(myDirectory + "\\" + myFileName);
                    File.AppendAllText(myDirectory + "\\" + myFileName + "\\" + myFileName + ".ino", convertedScript);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void feedAppend(string message)
        {
            richTextBoxOutput.Text += message + "\n";
        }
        private void richTextBoxOutput_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBoxOutput.SelectionStart = richTextBoxOutput.Text.Length;
            // scroll it automatically
            richTextBoxOutput.ScrollToCaret();
        }
        
    }
}
