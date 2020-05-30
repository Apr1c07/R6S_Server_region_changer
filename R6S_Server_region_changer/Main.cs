using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace R6S_Server_region_changer
{
    public partial class Main : Form
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        [DllImport("KERNEL32.DLL")]
        private static extern uint WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        private Timer t;
        private bool refresh_checkRun = true;
        public int pre_index;

        public Main()
        {
            InitializeComponent();
            Updater z;
            z = new Updater();
            z.CheckForUpdates();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            get_time();
            timer1.Interval = 1000;
            timer1.Enabled = true;

            if (Properties.Settings.Default.quit_apps == true)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            if (Properties.Settings.Default.gamesettings_set_check == false)
            {
                Properties.Settings.Default.profile_name = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.profile_gamesettings = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.profile_exe = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.profile_platform = new System.Collections.Specialized.StringCollection();
                ini_gamesetting_dr();
            }
            else
            {
                combobox1_refresh(1);
                //read_server();
            }
        }

        private void ini_gamesetting_dr()
        {
            DialogResult dr = MessageBox.Show("初期設定をします。プロファイルを作成してください。" + Environment.NewLine +
                  Environment.NewLine, "初期設定");

            CreateProfile createProfile_ini = new CreateProfile();
            createProfile_ini.FormClosed += this.createProfile_ini_FormClosed;
            createProfile_ini.Show();
            createProfile_ini.TopMost = true;
        }//GameSettings.iniのファイルパスの初期設定

        private void read_server()
        {
            if (System.IO.File.Exists(Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]))
            {
                StringBuilder DataCenterHint = new StringBuilder(1024);
                GetPrivateProfileString(
                    "ONLINE",
                    "DataCenterHint",
                    "0",
                    DataCenterHint,
                    Convert.ToUInt32(DataCenterHint.Capacity),
                    Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]);

                for (int i = 0; i <= comboBox1.Items.Count-1; i++)
                {
                    if (((KeyValuePair<string, string>)comboBox1.Items[i]).Value.ToString() == DataCenterHint.ToString())
                    {
                        comboBox1.SelectedIndex = i;
                    }
                }
            }
            else
            {
                MessageBox.Show("指定されたGamesettings.iniファイルが存在しません。");
            }


        }//現在のサーバーをインデックスに設定

        private void check_run_R6S()
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("rainbowsix");

            if (ps.Length != 0)
            {
                if (toolStripComboBox1.Items.Count == 0)
                {
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "プロファイルを作成して下さい。";
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    comboBox1.Enabled = false;
                    toolStripComboBox1.Enabled = false;
                }
                else
                {
                    if (refresh_checkRun)
                    {
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        toolStripStatusLabel1.Text = "注意：現在Rainbow Six Siegeが起動中です。変更する際は必ずゲームを終了して下さい。";
                    }
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    comboBox1.Enabled = true;
                    toolStripComboBox1.Enabled = true;
                }
            }
            else
            {
                if (toolStripComboBox1.Items.Count == 0)
                {
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "プロファイルを作成して下さい。";
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    comboBox1.Enabled = false;
                    toolStripComboBox1.Enabled = false;
                }
                else
                {
                    if (refresh_checkRun)
                    {
                        toolStripStatusLabel1.ForeColor = Color.Black;
                        toolStripStatusLabel1.Text = "現在Rainbow Six Siegeは起動していません。";
                    }
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = true;
                    comboBox1.Enabled = true;
                    toolStripComboBox1.Enabled = true;
                }
            }
        }//R6S起動チェック

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(Properties.Settings.Default.profile_exe[Properties.Settings.Default.combobox1_selected_index]);
                if (checkBox1.Checked == true) this.Close();
            }
            catch
            {
                MessageBox.Show("ゲーム実行ファイルが見つかりませんでした。"
                    + Environment.NewLine + "ゲーム実行ファイルの場所を更新して下さい。" + Environment.NewLine
                    + Environment.NewLine + "通常の保存場所は以下です。"
                    + Environment.NewLine + "【Steam】 : C:\\Program Files(x86)\\Steam\\steamapps\\common\\Tom Clancy's Rainbow Six Siege\\RainbowSix.exe"
                    + Environment.NewLine + "【Uplay】 : C:\\Program Files (x86)\\Ubisoft\\Ubisoft Game Launcher\\games\\Tom Clancy's Rainbow Six Siege\\RainbowSix.exe"
                    , "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string[] directoryCount = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);

                openFileDialog1.InitialDirectory = directory;

                openFileDialog1.FileName = "RainbowSix.exe";
                openFileDialog1.Filter = "exe ファイル (.exe)|*.exe|all|*.*";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.profile_exe[Properties.Settings.Default.combobox1_selected_index] = openFileDialog1.FileName;
                    Properties.Settings.Default.Save();
                }
            }
        }//R6S起動

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("rainbowsix");
            foreach (System.Diagnostics.Process p in ps) p.Kill();
        }//R6S終了

        private void button3_Click(object sender, EventArgs e)
        {
            WritePrivateProfileString(
            "ONLINE",
            "DataCenterHint",
            comboBox1.SelectedValue.ToString(),
            Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]);

            StringBuilder DataCenterHint = new StringBuilder(1024);
            GetPrivateProfileString(
                "ONLINE",
                "DataCenterHint",
                "0",
                DataCenterHint,
                Convert.ToUInt32(DataCenterHint.Capacity),
                Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]);

            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "接続サーバーを「" + DataCenterHint.ToString() + "」に変更しました。";
            Properties.Settings.Default.username_selected = toolStripComboBox1.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            refresh_checkRun = false;
            time_start();
        }//GameSettings.iniへの書き込み

        private void timer1_Tick(object sender, EventArgs e)
        {
            check_run_R6S();
        }//R6Sの起動確認

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            About form2 = new About();
            form2.Show();
        }

        private void New_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProfile createProfile = new CreateProfile();
            createProfile.FormClosed += this.createProfile_FormClosed;
            createProfile.Show();
        }//プロファイル新規作成

        private void Remove_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Text = "";

            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                Properties.Settings.Default.profile_name.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_gamesettings.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_exe.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_platform.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.Save();

                combobox1_refresh(3);
            }
            else
            {
                MessageBox.Show("プロファイルが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//プロファイル削除

        private void Edit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeProfile changeProfile = new ChangeProfile();
            changeProfile.FormClosed += this.changeProfile_FormClosed;
            changeProfile.Show();
        }//プロファイル編集

        private void createProfile_ini_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                Properties.Settings.Default.gamesettings_set_check = true;
                Properties.Settings.Default.Save();

                StringBuilder DataCenterHint = new StringBuilder(1024);
                GetPrivateProfileString(
                    "ONLINE",
                    "DataCenterHint",
                    "0",
                    DataCenterHint,
                    Convert.ToUInt32(DataCenterHint.Capacity),
                    Properties.Settings.Default.profile_gamesettings[0]);

                toolStripComboBox1.Items.Add(Properties.Settings.Default.profile_name[0]);
                toolStripComboBox1.SelectedIndex = 0;
            }
            else
            {
                removeToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void createProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            combobox1_refresh(0);
        }

        private void changeProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            combobox1_refresh(0);
        }

        private void combobox1_refresh(int option)//0:新規データをインデックスに　　1:前回選択したプロファイルをインデックスに 　　 3:プロファイル削除後
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Text = "";

            for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
            {
                toolStripComboBox1.Items.Add(Properties.Settings.Default.profile_name[i]);
            }

            switch (option)
            {
                case 0:
                    if (Properties.Settings.Default.profile_name.Count != 0)
                    {
                        removeToolStripMenuItem.Enabled = true;
                        editToolStripMenuItem.Enabled = true;
                        button3.Enabled = true;
                        //toolStripComboBox1.SelectedIndex = Properties.Settings.Default.profile_name.Count - 1;
                        toolStripComboBox1.SelectedIndex = pre_index;
                    }
                    else
                    {
                        removeToolStripMenuItem.Enabled = false;
                        editToolStripMenuItem.Enabled = false;
                        button3.Enabled = false;
                    }
                    break;

                case 1:
                    removeToolStripMenuItem.Enabled = true;
                    editToolStripMenuItem.Enabled = true;
                    button3.Enabled = true;
                    for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
                    {
                        if (Properties.Settings.Default.profile_name[i] == Properties.Settings.Default.username_selected)
                        {
                            toolStripComboBox1.SelectedIndex = i;
                        }
                        else
                        {
                            toolStripComboBox1.SelectedIndex = 0;
                        }
                    }
                    break;

                case 3:
                    if (Properties.Settings.Default.profile_name.Count != 0)
                    {
                        for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
                        {
                            if (Properties.Settings.Default.profile_name[i] == Properties.Settings.Default.username_selected)
                            {
                                toolStripComboBox1.SelectedIndex = i;
                            }
                            else
                            {
                                toolStripComboBox1.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        Properties.Settings.Default.gamesettings_set_check = false;
                        Properties.Settings.Default.Save();
                        comboBox1.SelectedIndex = 0;
                        removeToolStripMenuItem.Enabled = false;
                        editToolStripMenuItem.Enabled = false;
                    }
                    break;
            }
        }

        private void get_time()
        {
            DateTime utc = DateTime.UtcNow;
            DateTime now = DateTime.Now;
            TimeSpan eus = new TimeSpan(5, 0, 0);//-5
            TimeSpan cus = new TimeSpan(6, 0, 0);//-6
            TimeSpan scus = new TimeSpan(6, 0, 0);//-6
            TimeSpan wus = new TimeSpan(8, 0, 0);//-8
            TimeSpan sbr = new TimeSpan(3, 0, 0);//-3
            TimeSpan neu = new TimeSpan(2, 0, 0);//1~3
            TimeSpan weu = new TimeSpan(0, 0, 0);//0~1
            TimeSpan safn = new TimeSpan(2, 0, 0);//+2
            TimeSpan eas = new TimeSpan(8, 0, 0);//+8
            TimeSpan seas = new TimeSpan(8, 0, 0);//+7~9
            TimeSpan eau = new TimeSpan(9, 30, 0);//+9.5
            TimeSpan seau = new TimeSpan(10, 0, 0);//10
            TimeSpan wja = new TimeSpan(9, 0, 0);//+9

            DateTime eus_utc = utc - eus;
            DateTime cus_utc = utc - cus;
            DateTime scus_utc = utc - scus;
            DateTime wus_utc = utc - wus;
            DateTime sbr_utc = utc - sbr;
            DateTime neu_utc = utc + neu;
            DateTime weu_utc = utc + weu;
            DateTime safn_utc = utc + safn;
            DateTime eas_utc = utc + eas;
            DateTime seas_utc = utc + seas;
            DateTime eau_utc = utc + eau;
            DateTime seau_utc = utc + seau;
            DateTime wja_utc = utc + wja;

            KeyValuePair<string, String>[] AuthGroup = new KeyValuePair<string, String>[] {
                                                    new KeyValuePair<string, String>("default","default"),
                                                    new KeyValuePair<string, String>("eastus(アメリカ東部) <" + eus_utc.ToString("MM/dd HH:mm") + ">","eastus"),
                                                    new KeyValuePair<string, String>("centralus(アメリカ中部) <" + cus_utc.ToString("MM/dd HH:mm") + ">","centralus"),
                                                    new KeyValuePair<string, String>("southcentralus(アメリカ南中部) <" + scus_utc.ToString("MM/dd HH:mm") + ">","southcentralus"),
                                                    new KeyValuePair<string, String>("westus(アメリカ西部) <" + wus_utc.ToString("MM/dd HH:mm") + ">","westus"),
                                                    new KeyValuePair<string, String>("brazilsouth(ブラジル南部) <" + sbr_utc.ToString("MM/dd HH:mm") + ">","brazilsouth"),
                                                    new KeyValuePair<string, String>("northeurope(ヨーロッパ北部) <" + neu_utc.ToString("MM/dd HH:mm") + ">","northeurope"),
                                                    new KeyValuePair<string, String>("westeurope(ヨーロッパ西部) <" + weu_utc.ToString("MM/dd HH:mm") + ">","westeurope"),
                                                    new KeyValuePair<string, String>("southafricanorth(南アフリカ北部) <" + safn_utc.ToString("MM/dd HH:mm") + ">","southafricanorth"),
                                                    new KeyValuePair<string, String>("eastasia(アジア東部) <" + eas_utc.ToString("MM/dd HH:mm") + ">","eastasia"),
                                                    new KeyValuePair<string, String>("southeastasia(南東アジア) <" + seas_utc.ToString("MM/dd HH:mm") + ">","southeastasia"),
                                                    new KeyValuePair<string, String>("australiaeast(オーストラリア東部) <" + eau_utc.ToString("MM/dd HH:mm") + ">","australiaeast"),
                                                    new KeyValuePair<string, String>("australiasoutheast(オーストラリア南東部) <" + seau_utc.ToString("MM/dd HH:mm") + ">","australiasoutheast"),
                                                    new KeyValuePair<string, String>("japanwest(西日本) <" + wja_utc.ToString("MM/dd HH:mm") + ">","japanwest"),
                                                    };
            comboBox1.DataSource = AuthGroup;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";

            label3.Text = now.ToString("MM/dd HH:mm");
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.combobox1_selected_index = toolStripComboBox1.SelectedIndex;
            Properties.Settings.Default.Save();
            read_server();
            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "プロファイルを「" + Properties.Settings.Default.profile_name[Properties.Settings.Default.combobox1_selected_index] + "」に設定しました。";
            refresh_checkRun = false;
            time_start();
        }

        public class ColorPanel : StatusBarPanel
        {
            private Font _font = null;

            public Font Font
            {
                set
                {
                    _font = value;

                    if (this.Parent != null)
                    {
                        this.Parent.Refresh();
                    }
                }
                get { return _font; }
            }

            private Color _foreColor = Color.Empty;

            public Color ForeColor
            {
                set
                {
                    _foreColor = value;

                    if (this.Parent != null)
                    {
                        this.Parent.Refresh();
                    }
                }
                get { return _foreColor; }
            }

            private Color _backColor = Color.Empty;

            public Color BackColor
            {
                set
                {
                    _backColor = value;

                    if (this.Parent != null)
                    {
                        this.Parent.Refresh();
                    }
                }
                get { return _backColor; }
            }

            public ColorPanel(StatusBar sb)
            {
                this.Style = StatusBarPanelStyle.OwnerDraw;

                sb.DrawItem += new StatusBarDrawItemEventHandler(sb_DrawItem);
            }

            private void sb_DrawItem(object sender,
                StatusBarDrawItemEventArgs sbdevent)
            {
                if (sbdevent.Panel == this)
                {
                    StringFormat sf = new StringFormat();
                    if (this.Alignment == HorizontalAlignment.Left)
                        sf.Alignment = StringAlignment.Near;
                    else if (this.Alignment == HorizontalAlignment.Center)
                        sf.Alignment = StringAlignment.Center;
                    else if (this.Alignment == HorizontalAlignment.Right)
                        sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;

                    Brush foreBrush;
                    if (_foreColor != Color.Empty)
                        foreBrush = new SolidBrush(_foreColor);
                    else
                        foreBrush = new SolidBrush(sbdevent.ForeColor);
                    Brush backBrush;
                    if (_backColor != Color.Empty)
                        backBrush = new SolidBrush(_backColor);
                    else
                        backBrush = new SolidBrush(sbdevent.BackColor);

                    Font fnt;
                    if (_font != null)
                        fnt = _font;
                    else
                        fnt = sbdevent.Font;

                    sbdevent.Graphics.FillRectangle(
                        backBrush, sbdevent.Bounds);

                    sbdevent.Graphics.DrawString(
                        this.Text, fnt, foreBrush, sbdevent.Bounds, sf);

                    foreBrush.Dispose();
                    backBrush.Dispose();
                }
            }
        }//ToolStripの文字色変更用クラス

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            About form2 = new About();
            form2.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProfile createProfile = new CreateProfile();
            createProfile.FormClosed += this.createProfile_FormClosed;
            createProfile.Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pre_index = toolStripComboBox1.SelectedIndex;
            ChangeProfile changeProfile = new ChangeProfile();
            changeProfile.FormClosed += this.changeProfile_FormClosed;
            changeProfile.Show();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Text = "";

            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                Properties.Settings.Default.profile_name.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_gamesettings.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_exe.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_platform.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.Save();

                combobox1_refresh(3);
            }
            else
            {
                MessageBox.Show("プロファイルが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void time_start()
        {
            t = new Timer();
            t.Tick += new EventHandler(MyEvent);
            t.Interval = 3000; // ミリ秒単位で指定
            t.Start();
        }

        private void MyEvent(object sender, EventArgs e)
        {
            t.Stop();
            refresh_checkRun = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}