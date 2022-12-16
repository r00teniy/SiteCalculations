namespace SiteCalculations.Forms
{
    partial class MainForm
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
            this.errLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCity = new System.Windows.Forms.ComboBox();
            this.bCityCreate = new System.Windows.Forms.Button();
            this.bCreateReq = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbSite = new System.Windows.Forms.RadioButton();
            this.rbStage = new System.Windows.Forms.RadioButton();
            this.rbParking = new System.Windows.Forms.RadioButton();
            this.bCreateTable = new System.Windows.Forms.Button();
            this.cbStages_site = new System.Windows.Forms.CheckBox();
            this.cb_buildings = new System.Windows.Forms.CheckBox();
            this.cbStages_stages = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbStages_AllStages = new System.Windows.Forms.RadioButton();
            this.rbStages_SingleStage = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bCityDelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errLabel
            // 
            this.errLabel.AutoSize = true;
            this.errLabel.ForeColor = System.Drawing.Color.Red;
            this.errLabel.Location = new System.Drawing.Point(33, 210);
            this.errLabel.Name = "errLabel";
            this.errLabel.Size = new System.Drawing.Size(10, 13);
            this.errLabel.TabIndex = 17;
            this.errLabel.Text = " ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выберите город:";
            // 
            // cbCity
            // 
            this.cbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCity.FormattingEnabled = true;
            this.cbCity.Location = new System.Drawing.Point(119, 15);
            this.cbCity.Name = "cbCity";
            this.cbCity.Size = new System.Drawing.Size(139, 21);
            this.cbCity.TabIndex = 1;
            // 
            // bCityCreate
            // 
            this.bCityCreate.Location = new System.Drawing.Point(24, 40);
            this.bCityCreate.Name = "bCityCreate";
            this.bCityCreate.Size = new System.Drawing.Size(111, 21);
            this.bCityCreate.TabIndex = 3;
            this.bCityCreate.Text = "Создать новый";
            this.bCityCreate.UseVisualStyleBackColor = true;
            this.bCityCreate.Click += new System.EventHandler(this.bCityCreate_Click);
            // 
            // bCreateReq
            // 
            this.bCreateReq.Location = new System.Drawing.Point(16, 93);
            this.bCreateReq.Name = "bCreateReq";
            this.bCreateReq.Size = new System.Drawing.Size(242, 21);
            this.bCreateReq.TabIndex = 3;
            this.bCreateReq.Text = "Расчитать требуемые площадки и парковки";
            this.bCreateReq.UseVisualStyleBackColor = true;
            this.bCreateReq.Click += new System.EventHandler(this.bCreateReq_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Название площадки";
            // 
            // bName
            // 
            this.bName.Location = new System.Drawing.Point(138, 67);
            this.bName.Name = "bName";
            this.bName.Size = new System.Drawing.Size(120, 20);
            this.bName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Выберите тип таблицы для создания:";
            // 
            // rbSite
            // 
            this.rbSite.AutoSize = true;
            this.rbSite.Checked = true;
            this.rbSite.Location = new System.Drawing.Point(12, 4);
            this.rbSite.Name = "rbSite";
            this.rbSite.Size = new System.Drawing.Size(78, 17);
            this.rbSite.TabIndex = 4;
            this.rbSite.TabStop = true;
            this.rbSite.Text = "Площадка";
            this.rbSite.UseVisualStyleBackColor = true;
            this.rbSite.CheckedChanged += new System.EventHandler(this.rbSite_CheckedChanged);
            // 
            // rbStage
            // 
            this.rbStage.AutoSize = true;
            this.rbStage.Location = new System.Drawing.Point(96, 4);
            this.rbStage.Name = "rbStage";
            this.rbStage.Size = new System.Drawing.Size(49, 17);
            this.rbStage.TabIndex = 5;
            this.rbStage.Text = "Этап";
            this.rbStage.UseVisualStyleBackColor = true;
            this.rbStage.CheckedChanged += new System.EventHandler(this.rbStage_CheckedChanged);
            // 
            // rbParking
            // 
            this.rbParking.AutoSize = true;
            this.rbParking.Location = new System.Drawing.Point(151, 4);
            this.rbParking.Name = "rbParking";
            this.rbParking.Size = new System.Drawing.Size(75, 17);
            this.rbParking.TabIndex = 6;
            this.rbParking.Text = "Парковки";
            this.rbParking.UseVisualStyleBackColor = true;
            this.rbParking.CheckedChanged += new System.EventHandler(this.rbParking_CheckedChanged);
            // 
            // bCreateTable
            // 
            this.bCreateTable.Location = new System.Drawing.Point(65, 226);
            this.bCreateTable.Name = "bCreateTable";
            this.bCreateTable.Size = new System.Drawing.Size(130, 21);
            this.bCreateTable.TabIndex = 7;
            this.bCreateTable.Text = "Создать таблицу";
            this.bCreateTable.UseVisualStyleBackColor = true;
            this.bCreateTable.Click += new System.EventHandler(this.bCreateTable_Click);
            // 
            // cbStages_site
            // 
            this.cbStages_site.AutoSize = true;
            this.cbStages_site.Location = new System.Drawing.Point(13, 2);
            this.cbStages_site.Name = "cbStages_site";
            this.cbStages_site.Size = new System.Drawing.Size(110, 17);
            this.cbStages_site.TabIndex = 12;
            this.cbStages_site.Text = "Включить Этапы";
            this.cbStages_site.UseVisualStyleBackColor = true;
            // 
            // cb_buildings
            // 
            this.cb_buildings.AutoSize = true;
            this.cb_buildings.Location = new System.Drawing.Point(25, 188);
            this.cb_buildings.Name = "cb_buildings";
            this.cb_buildings.Size = new System.Drawing.Size(107, 17);
            this.cb_buildings.TabIndex = 13;
            this.cb_buildings.Text = "Включить Дома";
            this.cb_buildings.UseVisualStyleBackColor = true;
            // 
            // cbStages_stages
            // 
            this.cbStages_stages.FormattingEnabled = true;
            this.cbStages_stages.ItemHeight = 13;
            this.cbStages_stages.Location = new System.Drawing.Point(138, 184);
            this.cbStages_stages.Name = "cbStages_stages";
            this.cbStages_stages.Size = new System.Drawing.Size(120, 21);
            this.cbStages_stages.TabIndex = 10;
            this.cbStages_stages.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbParking);
            this.panel1.Controls.Add(this.rbStage);
            this.panel1.Controls.Add(this.rbSite);
            this.panel1.Location = new System.Drawing.Point(12, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 25);
            this.panel1.TabIndex = 18;
            // 
            // rbStages_AllStages
            // 
            this.rbStages_AllStages.AutoSize = true;
            this.rbStages_AllStages.Checked = true;
            this.rbStages_AllStages.Location = new System.Drawing.Point(13, 1);
            this.rbStages_AllStages.Name = "rbStages_AllStages";
            this.rbStages_AllStages.Size = new System.Drawing.Size(78, 17);
            this.rbStages_AllStages.TabIndex = 8;
            this.rbStages_AllStages.TabStop = true;
            this.rbStages_AllStages.Text = "Все этапы";
            this.rbStages_AllStages.UseVisualStyleBackColor = true;
            this.rbStages_AllStages.Visible = false;
            this.rbStages_AllStages.CheckedChanged += new System.EventHandler(this.rbStages_AllStages_CheckedChanged);
            // 
            // rbStages_SingleStage
            // 
            this.rbStages_SingleStage.AutoSize = true;
            this.rbStages_SingleStage.Location = new System.Drawing.Point(126, 2);
            this.rbStages_SingleStage.Name = "rbStages_SingleStage";
            this.rbStages_SingleStage.Size = new System.Drawing.Size(77, 17);
            this.rbStages_SingleStage.TabIndex = 9;
            this.rbStages_SingleStage.Text = "Один этап";
            this.rbStages_SingleStage.UseVisualStyleBackColor = true;
            this.rbStages_SingleStage.Visible = false;
            this.rbStages_SingleStage.CheckedChanged += new System.EventHandler(this.rbStages_SingleStage_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbStages_SingleStage);
            this.panel2.Controls.Add(this.rbStages_AllStages);
            this.panel2.Controls.Add(this.cbStages_site);
            this.panel2.Location = new System.Drawing.Point(12, 163);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(244, 21);
            this.panel2.TabIndex = 21;
            // 
            // bCityDelete
            // 
            this.bCityDelete.Location = new System.Drawing.Point(147, 40);
            this.bCityDelete.Name = "bCityDelete";
            this.bCityDelete.Size = new System.Drawing.Size(111, 21);
            this.bCityDelete.TabIndex = 3;
            this.bCityDelete.Text = "Удалить";
            this.bCityDelete.UseVisualStyleBackColor = true;
            this.bCityDelete.Click += new System.EventHandler(this.bCityDelete_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 250);
            this.Controls.Add(this.bCityDelete);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.errLabel);
            this.Controls.Add(this.cbStages_stages);
            this.Controls.Add(this.cb_buildings);
            this.Controls.Add(this.bCreateTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bCreateReq);
            this.Controls.Add(this.bCityCreate);
            this.Controls.Add(this.cbCity);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подсчёт площадки";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bCityCreate;
        private System.Windows.Forms.Button bCreateReq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox bName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbSite;
        private System.Windows.Forms.RadioButton rbStage;
        private System.Windows.Forms.RadioButton rbParking;
        private System.Windows.Forms.Button bCreateTable;
        private System.Windows.Forms.CheckBox cbStages_site;
        private System.Windows.Forms.CheckBox cb_buildings;
        private System.Windows.Forms.ComboBox cbStages_stages;
        public System.Windows.Forms.ComboBox cbCity;
        private System.Windows.Forms.Label errLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbStages_AllStages;
        private System.Windows.Forms.RadioButton rbStages_SingleStage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button bCityDelete;
    }
}