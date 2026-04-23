namespace ChatSimple
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtIP = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txtPuerto = new TextBox();
            button1 = new Button();
            rtbHistorial = new RichTextBox();
            label3 = new Label();
            label4 = new Label();
            txtMensaje = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(41, 39);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(177, 23);
            txtIP.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 21);
            label1.Name = "label1";
            label1.Size = new Size(17, 15);
            label1.TabIndex = 1;
            label1.Text = "IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(271, 21);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 2;
            label2.Text = "Puerto";
            // 
            // txtPuerto
            // 
            txtPuerto.Location = new Point(271, 39);
            txtPuerto.Name = "txtPuerto";
            txtPuerto.Size = new Size(95, 23);
            txtPuerto.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(406, 38);
            button1.Name = "button1";
            button1.Size = new Size(136, 24);
            button1.TabIndex = 4;
            button1.Text = "Iniciar server";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // rtbHistorial
            // 
            rtbHistorial.Location = new Point(41, 113);
            rtbHistorial.Name = "rtbHistorial";
            rtbHistorial.Size = new Size(501, 193);
            rtbHistorial.TabIndex = 5;
            rtbHistorial.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 84);
            label3.Name = "label3";
            label3.Size = new Size(56, 15);
            label3.TabIndex = 6;
            label3.Text = "Mensajes";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 327);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 7;
            label4.Text = "Mensaje";
            // 
            // txtMensaje
            // 
            txtMensaje.Location = new Point(41, 345);
            txtMensaje.Name = "txtMensaje";
            txtMensaje.Size = new Size(367, 23);
            txtMensaje.TabIndex = 8;
            // 
            // button2
            // 
            button2.Location = new Point(435, 345);
            button2.Name = "button2";
            button2.Size = new Size(107, 24);
            button2.TabIndex = 9;
            button2.Text = "Enviar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(581, 396);
            Controls.Add(button2);
            Controls.Add(txtMensaje);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(rtbHistorial);
            Controls.Add(button1);
            Controls.Add(txtPuerto);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtIP);
            Name = "Form1";
            Text = "Chat Simple";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIP;
        private Label label1;
        private Label label2;
        private TextBox txtPuerto;
        private Button button1;
        private RichTextBox rtbHistorial;
        private Label label3;
        private Label label4;
        private TextBox txtMensaje;
        private Button button2;
    }
}
