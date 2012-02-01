namespace TestRobot
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_connect = new System.Windows.Forms.Button();
            this.bt_disconnect = new System.Windows.Forms.Button();
            this.bt_goFront = new System.Windows.Forms.Button();
            this.lb_con = new System.Windows.Forms.Label();
            this.lb_state = new System.Windows.Forms.Label();
            this.bt_goBack = new System.Windows.Forms.Button();
            this.bt_goRight = new System.Windows.Forms.Button();
            this.bt_goLeft = new System.Windows.Forms.Button();
            this.bt_colorDectect = new System.Windows.Forms.Button();
            this.lb_colorDetected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bt_connect
            // 
            this.bt_connect.Location = new System.Drawing.Point(12, 12);
            this.bt_connect.Name = "bt_connect";
            this.bt_connect.Size = new System.Drawing.Size(100, 23);
            this.bt_connect.TabIndex = 0;
            this.bt_connect.Text = "Connection";
            this.bt_connect.UseVisualStyleBackColor = true;
            this.bt_connect.Click += new System.EventHandler(this.bt_connect_Click);
            // 
            // bt_disconnect
            // 
            this.bt_disconnect.Location = new System.Drawing.Point(12, 41);
            this.bt_disconnect.Name = "bt_disconnect";
            this.bt_disconnect.Size = new System.Drawing.Size(100, 23);
            this.bt_disconnect.TabIndex = 1;
            this.bt_disconnect.Text = "Déconnection";
            this.bt_disconnect.UseVisualStyleBackColor = true;
            this.bt_disconnect.Click += new System.EventHandler(this.bt_disconnect_Click);
            // 
            // bt_goFront
            // 
            this.bt_goFront.Location = new System.Drawing.Point(294, 134);
            this.bt_goFront.Name = "bt_goFront";
            this.bt_goFront.Size = new System.Drawing.Size(75, 23);
            this.bt_goFront.TabIndex = 2;
            this.bt_goFront.Text = "Avancer";
            this.bt_goFront.UseVisualStyleBackColor = true;
            this.bt_goFront.Click += new System.EventHandler(this.bt_gofront_Click);
            // 
            // lb_con
            // 
            this.lb_con.AutoSize = true;
            this.lb_con.Location = new System.Drawing.Point(118, 17);
            this.lb_con.Name = "lb_con";
            this.lb_con.Size = new System.Drawing.Size(82, 13);
            this.lb_con.TabIndex = 3;
            this.lb_con.Text = "Etat connection";
            // 
            // lb_state
            // 
            this.lb_state.AutoSize = true;
            this.lb_state.Location = new System.Drawing.Point(291, 118);
            this.lb_state.Name = "lb_state";
            this.lb_state.Size = new System.Drawing.Size(29, 13);
            this.lb_state.TabIndex = 4;
            this.lb_state.Text = "Stop";
            // 
            // bt_goBack
            // 
            this.bt_goBack.Location = new System.Drawing.Point(294, 211);
            this.bt_goBack.Name = "bt_goBack";
            this.bt_goBack.Size = new System.Drawing.Size(75, 23);
            this.bt_goBack.TabIndex = 5;
            this.bt_goBack.Text = "Reculer";
            this.bt_goBack.UseVisualStyleBackColor = true;
            this.bt_goBack.Click += new System.EventHandler(this.bt_goBack_Click);
            // 
            // bt_goRight
            // 
            this.bt_goRight.Location = new System.Drawing.Point(380, 171);
            this.bt_goRight.Name = "bt_goRight";
            this.bt_goRight.Size = new System.Drawing.Size(75, 23);
            this.bt_goRight.TabIndex = 6;
            this.bt_goRight.Text = "Droite";
            this.bt_goRight.UseVisualStyleBackColor = true;
            this.bt_goRight.Click += new System.EventHandler(this.bt_goRight_Click);
            // 
            // bt_goLeft
            // 
            this.bt_goLeft.Location = new System.Drawing.Point(212, 171);
            this.bt_goLeft.Name = "bt_goLeft";
            this.bt_goLeft.Size = new System.Drawing.Size(75, 23);
            this.bt_goLeft.TabIndex = 7;
            this.bt_goLeft.Text = "Gauche";
            this.bt_goLeft.UseVisualStyleBackColor = true;
            this.bt_goLeft.Click += new System.EventHandler(this.bt_goLeft_Click);
            // 
            // bt_colorDectect
            // 
            this.bt_colorDectect.Location = new System.Drawing.Point(294, 171);
            this.bt_colorDectect.Name = "bt_colorDectect";
            this.bt_colorDectect.Size = new System.Drawing.Size(75, 23);
            this.bt_colorDectect.TabIndex = 8;
            this.bt_colorDectect.Text = "Détecteur";
            this.bt_colorDectect.UseVisualStyleBackColor = true;
            this.bt_colorDectect.Click += new System.EventHandler(this.bt_colorDectect_Click);
            // 
            // lb_colorDetected
            // 
            this.lb_colorDetected.AutoSize = true;
            this.lb_colorDetected.Location = new System.Drawing.Point(291, 248);
            this.lb_colorDetected.Name = "lb_colorDetected";
            this.lb_colorDetected.Size = new System.Drawing.Size(56, 13);
            this.lb_colorDetected.TabIndex = 9;
            this.lb_colorDetected.Text = "En attente";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 373);
            this.Controls.Add(this.lb_colorDetected);
            this.Controls.Add(this.bt_colorDectect);
            this.Controls.Add(this.bt_goLeft);
            this.Controls.Add(this.bt_goRight);
            this.Controls.Add(this.bt_goBack);
            this.Controls.Add(this.lb_state);
            this.Controls.Add(this.lb_con);
            this.Controls.Add(this.bt_goFront);
            this.Controls.Add(this.bt_disconnect);
            this.Controls.Add(this.bt_connect);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_connect;
        private System.Windows.Forms.Button bt_disconnect;
        private System.Windows.Forms.Button bt_goFront;
        private System.Windows.Forms.Label lb_con;
        private System.Windows.Forms.Label lb_state;
        private System.Windows.Forms.Button bt_goBack;
        private System.Windows.Forms.Button bt_goRight;
        private System.Windows.Forms.Button bt_goLeft;
        private System.Windows.Forms.Button bt_colorDectect;
        private System.Windows.Forms.Label lb_colorDetected;
    }
}

