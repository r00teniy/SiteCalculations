namespace SiteCalculations.Forms
{
    partial class ParkingModelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bLong = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLong = new System.Windows.Forms.ComboBox();
            this.cbGuest = new System.Windows.Forms.ComboBox();
            this.bShort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bGuest = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bSchool = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bKindergarten = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bHospital = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.createParkingButton = new System.Windows.Forms.Button();
            this.bName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bLong
            // 
            this.bLong.Location = new System.Drawing.Point(307, 21);
            this.bLong.Name = "bLong";
            this.bLong.Size = new System.Drawing.Size(100, 20);
            this.bLong.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Кол-во постоянных парковок на ";
            // 
            // cbLong
            // 
            this.cbLong.FormattingEnabled = true;
            this.cbLong.Items.AddRange(new object[] {
            "человека",
            "квартиру",
            "м2 площади"});
            this.cbLong.Location = new System.Drawing.Point(180, 20);
            this.cbLong.Name = "cbLong";
            this.cbLong.Size = new System.Drawing.Size(121, 21);
            this.cbLong.TabIndex = 10;
            this.cbLong.TabStop = false;
            // 
            // cbGuest
            // 
            this.cbGuest.FormattingEnabled = true;
            this.cbGuest.Items.AddRange(new object[] {
            "человека",
            "квартиру",
            "м2 площади"});
            this.cbGuest.Location = new System.Drawing.Point(180, 47);
            this.cbGuest.Name = "cbGuest";
            this.cbGuest.Size = new System.Drawing.Size(121, 21);
            this.cbGuest.TabIndex = 13;
            this.cbGuest.TabStop = false;
            // 
            // bShort
            // 
            this.bShort.Location = new System.Drawing.Point(307, 48);
            this.bShort.Name = "bShort";
            this.bShort.Size = new System.Drawing.Size(100, 20);
            this.bShort.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Кол-во гостевых парковок на ";
            // 
            // bGuest
            // 
            this.bGuest.Location = new System.Drawing.Point(307, 75);
            this.bGuest.Name = "bGuest";
            this.bGuest.Size = new System.Drawing.Size(100, 20);
            this.bGuest.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(291, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Кол-во временных парковок на  м2 встроенных помещ.";
            // 
            // bSchool
            // 
            this.bSchool.Location = new System.Drawing.Point(307, 101);
            this.bSchool.Name = "bSchool";
            this.bSchool.Size = new System.Drawing.Size(100, 20);
            this.bSchool.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(223, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Кол-во парковок для школ на 1 учащегося";
            // 
            // bKindergarten
            // 
            this.bKindergarten.Location = new System.Drawing.Point(307, 127);
            this.bKindergarten.Name = "bKindergarten";
            this.bKindergarten.Size = new System.Drawing.Size(100, 20);
            this.bKindergarten.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(239, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Кол-во парковок для садиков на 1 учащегося";
            // 
            // bHospital
            // 
            this.bHospital.Location = new System.Drawing.Point(307, 153);
            this.bHospital.Name = "bHospital";
            this.bHospital.Size = new System.Drawing.Size(100, 20);
            this.bHospital.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(274, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Кол-во парковок для больниц на 1 посетителя в сут.";
            // 
            // createParkingButton
            // 
            this.createParkingButton.Location = new System.Drawing.Point(303, 182);
            this.createParkingButton.Name = "createParkingButton";
            this.createParkingButton.Size = new System.Drawing.Size(100, 22);
            this.createParkingButton.TabIndex = 24;
            this.createParkingButton.Text = "Создать";
            this.createParkingButton.UseVisualStyleBackColor = true;
            this.createParkingButton.Click += new System.EventHandler(this.createParkingButton_Click);
            // 
            // bName
            // 
            this.bName.Location = new System.Drawing.Point(186, 182);
            this.bName.Name = "bName";
            this.bName.Size = new System.Drawing.Size(100, 20);
            this.bName.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(181, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Введите имя набора параметров: ";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(120, 205);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(129, 13);
            this.errorLabel.TabIndex = 26;
            this.errorLabel.Text = "Ошибка! Выберите имя.";
            this.errorLabel.Visible = false;
            // 
            // ParkingModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 224);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.bName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.createParkingButton);
            this.Controls.Add(this.bHospital);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bKindergarten);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bSchool);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bGuest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbGuest);
            this.Controls.Add(this.bShort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLong);
            this.Controls.Add(this.bLong);
            this.Controls.Add(this.label4);
            this.Name = "ParkingModelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Создание параметров парковок";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox bLong;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox bShort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox bGuest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox bSchool;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox bKindergarten;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox bHospital;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button createParkingButton;
        private System.Windows.Forms.TextBox bName;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox cbLong;
        public System.Windows.Forms.ComboBox cbGuest;
        private System.Windows.Forms.Label errorLabel;
    }
}