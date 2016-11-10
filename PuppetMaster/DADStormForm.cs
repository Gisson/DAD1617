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

namespace PuppetMaster {
    public partial class DADStormForm : Form {
        private PuppetMaster PM;
        Queue<int> stepPositions; //location of all valid commands on the "debugger"

        public DADStormForm() {
            PM = new PuppetMaster();
            stepPositions = new Queue<int>();
            /*forms & delegates shenanigans*/
            PM.Parser.PreviewTextBox_Update = new Action<String, LineSyntax>(previewTextBox_Update);
            InitializeComponent();
            previewTextBox.ReadOnly = true;
        }

        private void stepButton_Click(object sender, EventArgs e) {
            //scroll to current instruction
            previewTextBox.SelectionStart = stepPositions.Dequeue();
            previewTextBox.ScrollToCaret(); //previewTextBox.po
            //previewTextBox.scro
            //display step dot
            //stepPoint.Visible = true;
            //stepPoint.Location = new Point(stepPoint.Location.X, previewTextBox.SelectionStart);

            //order pm to step
            PM.executeInstructions(true);
            if (!stepPositions.Any()) { //disable buttons
                runButton.Enabled = false;
                stepButton.Enabled = false;
            }
        }


        private void runButton_Click(object sender, EventArgs e) {
            PM.executeInstructions(false);
            //disable buttons
            runButton.Enabled = false;
            stepButton.Enabled = false;
        }

        private void browseButton_Click(object sender, EventArgs e) {
            /*display dialog to choose file*/
            OpenFileDialog fDialog = new OpenFileDialog();
            openFileDialog1.Title = "Open Configuration File";
            openFileDialog1.Filter = "Config Files|*.config|All Files|*.*";
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory().ToString();
            openFileDialog1.ShowDialog();
            //store selected filename
            String configFilename = openFileDialog1.FileName;
            if (configFilename != null) {
                pathTextBox.Text = configFilename;
                //clean current preview text box
                previewTextBox.Text = "";

                //tell puppet master to start parsing
                PM.Parser.execute(configFilename);
            }
        }

        //populates preview box with the contents from the config file
        public void previewTextBox_Update(String line, LineSyntax lineSyntax) {
            // Make sure we're on the UI thread
            if (previewTextBox.InvokeRequired == false) {
                //prettify text depending on the syntax
                previewTextBox.DeselectAll();
                switch (lineSyntax) {
                    case LineSyntax.VALID: {
                            previewTextBox.SelectionColor = Color.White;
                            //if there is at least one valid line, enable execution buttons    
                            runButton.Enabled = true;
                            stepButton.Enabled = true;
                            //store position for the debugger
                            stepPositions.Enqueue(previewTextBox.TextLength);
                            break;
                        }
                    case LineSyntax.INVALID: {
                            previewTextBox.SelectionColor = Color.DarkGray;
                            previewTextBox.SelectionFont = new Font(previewTextBox.SelectionFont, FontStyle.Strikeout);
                            break;
                        }
                    case LineSyntax.COMMENT: {
                            previewTextBox.SelectionColor = Color.DarkGreen;
                            break;
                        }
                }
                previewTextBox.AppendText(line + Environment.NewLine);
            }
            else {
                //more weird forms stuff, don't ask, it just works
                Invoke(new Action<String, LineSyntax>(previewTextBox_Update), new object[] { line, lineSyntax });
            }
        }


    }
}
