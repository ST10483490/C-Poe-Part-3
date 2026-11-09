using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChatBotGUI
{
    /// <summary>
    /// Main GUI Window incorporating Task 1 ASCII art, voice greeting, and interactive chat elements.
    /// </summary>
    public partial class MainForm : Form
    {
        private ChatEngine chatEngine;
        private TextBox txtInput;
        private RichTextBox txtChatDisplay;
        private Button btnSend;

        public MainForm()
        {
            InitializeComponentCustom();
            chatEngine = new ChatEngine();

            // Play Task 1 audio greeting
            AudioPlayer.PlayGreeting(@"Audio\Audio Greeting.wav");

            // Initial bot prompt
            AppendBotMessage("Please enter your name to begin:");
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Cybersecurity Awareness Bot - Part 2 GUI";
            this.Size = new Size(850, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);

            // ASCII Art / Chat Display Box (Monospaced for ASCII alignment)
            txtChatDisplay = new RichTextBox();
            txtChatDisplay.Location = new Point(20, 20);
            txtChatDisplay.Size = new Size(795, 500);
            txtChatDisplay.BackColor = Color.FromArgb(20, 20, 20);
            txtChatDisplay.ForeColor = Color.LightGreen;
            txtChatDisplay.Font = new Font("Consolas", 10, FontStyle.Regular);
            txtChatDisplay.ReadOnly = true;
            this.Controls.Add(txtChatDisplay);

            // Input TextBox
            txtInput = new TextBox();
            txtInput.Location = new Point(20, 535);
            txtInput.Size = new Size(670, 30);
            txtInput.BackColor = Color.FromArgb(45, 45, 45);
            txtInput.ForeColor = Color.White;
            txtInput.Font = new Font("Segoe UI", 11);
            txtInput.KeyDown += TxtInput_KeyDown;
            this.Controls.Add(txtInput);

            // Send Button
            btnSend = new Button();
            btnSend.Location = new Point(705, 534);
            btnSend.Size = new Size(110, 32);
            btnSend.Text = "Send";
            btnSend.BackColor = Color.FromArgb(0, 122, 204);
            btnSend.ForeColor = Color.White;
            btnSend.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Click += BtnSend_Click;
            this.Controls.Add(btnSend);

            // Display Task 1 ASCII Header in GUI
            LoadAsciiHeader();
        }

        private void LoadAsciiHeader()
        {
            string asciiHeader = @"
╔═══════════════════════════════════════════════════════════════════════════════╗
║                      CYBERSECURITY AWARENESS ASSISTANT                        ║
╚═══════════════════════════════════════════════════════════════════════════════╝
";
            txtChatDisplay.AppendText(asciiHeader + "\n");
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            HandleUserSubmission();
        }

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                HandleUserSubmission();
            }
        }

        private void HandleUserSubmission()
        {
            string userInput = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            // Append User message
            txtChatDisplay.SelectionColor = Color.Cyan;
            txtChatDisplay.AppendText($"\nYou > {userInput}\n");
            txtInput.Clear();

            string response;
            if (!chatEngine.IsNameRegistered)
            {
                response = chatEngine.SetUserName(userInput);
            }
            else
            {
                response = chatEngine.ProcessInput(userInput);
            }

            // Append Bot response
            AppendBotMessage(response);
        }

        private void AppendBotMessage(string message)
        {
            txtChatDisplay.SelectionColor = Color.LightGreen;
            txtChatDisplay.AppendText($"Bot > {message}\n");
            txtChatDisplay.ScrollToCaret();
        }
    }
}