namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBoxMain = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxRoomNumber = new System.Windows.Forms.TextBox();
            this.richTextBoxInput = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBoxCurUser = new System.Windows.Forms.RichTextBox();
            this.textBoxAllRoom = new System.Windows.Forms.TextBox();
            this.textBoxCurRoom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.name_change_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxMain
            // 
            this.richTextBoxMain.Location = new System.Drawing.Point(12, 77);
            this.richTextBoxMain.Name = "richTextBoxMain";
            this.richTextBoxMain.ReadOnly = true;
            this.richTextBoxMain.Size = new System.Drawing.Size(492, 309);
            this.richTextBoxMain.TabIndex = 0;
            this.richTextBoxMain.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(461, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "방 참여";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(624, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "방 나가기";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxRoomNumber
            // 
            this.textBoxRoomNumber.Location = new System.Drawing.Point(274, 12);
            this.textBoxRoomNumber.MaxLength = 3;
            this.textBoxRoomNumber.Name = "textBoxRoomNumber";
            this.textBoxRoomNumber.Size = new System.Drawing.Size(100, 21);
            this.textBoxRoomNumber.TabIndex = 3;
            this.textBoxRoomNumber.Text = "방이름";
            this.textBoxRoomNumber.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // richTextBoxInput
            // 
            this.richTextBoxInput.Location = new System.Drawing.Point(12, 392);
            this.richTextBoxInput.Name = "richTextBoxInput";
            this.richTextBoxInput.Size = new System.Drawing.Size(492, 161);
            this.richTextBoxInput.TabIndex = 5;
            this.richTextBoxInput.Text = "";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(510, 392);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(189, 161);
            this.button4.TabIndex = 6;
            this.button4.Text = "전송";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(380, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "방 생성";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(93, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "모든 방 확인";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBoxCurUser
            // 
            this.richTextBoxCurUser.Location = new System.Drawing.Point(510, 77);
            this.richTextBoxCurUser.Name = "richTextBoxCurUser";
            this.richTextBoxCurUser.ReadOnly = true;
            this.richTextBoxCurUser.Size = new System.Drawing.Size(189, 309);
            this.richTextBoxCurUser.TabIndex = 9;
            this.richTextBoxCurUser.Text = "";
            // 
            // textBoxAllRoom
            // 
            this.textBoxAllRoom.Location = new System.Drawing.Point(111, 50);
            this.textBoxAllRoom.MaxLength = 999;
            this.textBoxAllRoom.Name = "textBoxAllRoom";
            this.textBoxAllRoom.ReadOnly = true;
            this.textBoxAllRoom.Size = new System.Drawing.Size(393, 21);
            this.textBoxAllRoom.TabIndex = 10;
            // 
            // textBoxCurRoom
            // 
            this.textBoxCurRoom.Location = new System.Drawing.Point(602, 48);
            this.textBoxCurRoom.MaxLength = 3;
            this.textBoxCurRoom.Name = "textBoxCurRoom";
            this.textBoxCurRoom.ReadOnly = true;
            this.textBoxCurRoom.Size = new System.Drawing.Size(97, 21);
            this.textBoxCurRoom.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(527, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "현재 방번호";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(111, 11);
            this.textBoxName.MaxLength = 5;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(118, 21);
            this.textBoxName.TabIndex = 13;
            // 
            // name_change_button
            // 
            this.name_change_button.Location = new System.Drawing.Point(12, 12);
            this.name_change_button.Name = "name_change_button";
            this.name_change_button.Size = new System.Drawing.Size(93, 23);
            this.name_change_button.TabIndex = 14;
            this.name_change_button.Text = "이름 변경";
            this.name_change_button.UseVisualStyleBackColor = true;
            this.name_change_button.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 565);
            this.Controls.Add(this.name_change_button);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCurRoom);
            this.Controls.Add(this.textBoxAllRoom);
            this.Controls.Add(this.richTextBoxCurUser);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.richTextBoxInput);
            this.Controls.Add(this.textBoxRoomNumber);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBoxMain);
            this.Name = "Form1";
            this.Text = "CLient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMain;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxRoomNumber;
        private System.Windows.Forms.RichTextBox richTextBoxInput;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBoxCurUser;
        private System.Windows.Forms.TextBox textBoxAllRoom;
        private System.Windows.Forms.TextBox textBoxCurRoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button name_change_button;
    }
}

