using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace R6S_Server_region_changer
{
    public partial class CreateProfile : Form
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedString,
        uint nSize,
        string lpFileName);

        public CreateProfile()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            combo1_ini();
        }

        private void combo1_ini()
        {
            comboBox1.Items.Add("Steam");
            comboBox1.Items.Add("Uplay");
            comboBox1.Items.Add("Epic");
            comboBox1.SelectedIndex = 0;
        }//サーバー一覧

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\My Games\\Rainbow Six - Siege";
                string[] directoryCount = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);

                if (directoryCount.Length == 1)
                {
                    openFileDialog1.InitialDirectory = directoryCount[0];
                }
                else
                {
                    openFileDialog1.InitialDirectory = directory;
                }

                openFileDialog1.FileName = "GameSettings";
                openFileDialog1.Filter = "INI ファイル (.ini)|*.ini|all|*.*";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = openFileDialog1.FileName;
                }
            }
            else
            {
                string str = textBox2.Text;
                str = str.Remove(str.Length - 17, 17);

                openFileDialog1.InitialDirectory = str;

                openFileDialog1.FileName = "GameSettings";
                openFileDialog1.Filter = "INI ファイル (.ini)|*.ini|all|*.*";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = openFileDialog1.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool check_same_name = false;

            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
                {
                    if (textBox1.Text == Properties.Settings.Default.profile_name[i])
                    {
                        check_same_name = true;
                    }
                }
            }

            if (Properties.Settings.Default.profile_name.Count < 5)
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (check_same_name == false)
                    {
                        StringBuilder DataCenterHint = new StringBuilder(1024);
                        GetPrivateProfileString(
                            "ONLINE",
                            "DataCenterHint",
                            "0",
                            DataCenterHint,
                            Convert.ToUInt32(DataCenterHint.Capacity),
                            textBox2.Text);

                        if (DataCenterHint.ToString() == "0")
                        {
                            MessageBox.Show("サーバーリージョンを読み込めません。GameSettings.iniの場所を確認して下さい。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string R6S_exe = "";
                            if (comboBox1.Text == "Steam")
                            {
                                if (System.IO.File.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"C:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"D:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"D:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"E:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"E:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"F:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"F:\Program Files (x86)\Steam\steamapps\common\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                            }

                            if (comboBox1.Text == "Uplay")
                            {
                                if (System.IO.File.Exists(@"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"D:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"D:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"E:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"E:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"F:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"F:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Tom Clancy's Rainbow Six Siege\RainbowSix.exe";
                                }
                            }

                            if (comboBox1.Text == "Epic")
                            {
                                if (System.IO.File.Exists(@"C:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"C:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"D:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"D:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"E:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"E:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe";
                                }
                                if (System.IO.File.Exists(@"F:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe"))
                                {
                                    R6S_exe = @"F:\Program Files\Epic Games\RainbowSixSiege\RainbowSix.exe";
                                }
                            }

                            Properties.Settings.Default.profile_name.Add(textBox1.Text);
                            Properties.Settings.Default.profile_gamesettings.Add(textBox2.Text);
                            Properties.Settings.Default.profile_exe.Add(R6S_exe);
                            Properties.Settings.Default.profile_platform.Add(comboBox1.Text);
                            Properties.Settings.Default.Save();

                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("同じ名前のプロファイルが、既に存在します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("未入力の項目があります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("保存可能なプロファイルは5個までです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            string url_IDBase = "https://r6tab.com/api/search.php?platform=uplay&search=";
            string url = url_IDBase + textBox1.Text;

            WebClient wc = new WebClient();

            Stream st = wc.OpenRead(url);
            StreamReader sr = new StreamReader(st);

            string str = sr.ReadToEnd();

            sr.Close();
            st.Close();
            if (textBox1.Text != "")
            {
                if (str.Length < 20)
                {
                    MessageBox.Show("該当ユーザーが存在しませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(str.Length<300)
                {

                    string[] arr = str.Split('"');

                        var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\My Games\\Rainbow Six - Siege\\";
                    directory = directory + arr[5] + "\\GameSettings.ini";

                    if (System.IO.File.Exists(directory))
                    {
                        textBox2.Text = directory;
                    }
                    else
                    {
                        MessageBox.Show("指定されたユーザー名のファイルが存在しませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string[] arr = str.Split('"');
                    string directory;
                    bool ck=false;
                    for (int i = 0; i < 1305; i++) //50人分の検索 頭3　一人のデータ自体26　けつ2
                    {
                        if (arr[i] == "p_id")
                        {
                           directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\My Games\\Rainbow Six - Siege\\";
                            directory = directory + arr[i + 2] + "\\GameSettings.ini";

                            if (System.IO.File.Exists(directory))
                            {
                                textBox2.Text = directory;
                                ck = true;
                                break;
                            }

                        }
                    }
                    if(ck==false)
                    {
                        MessageBox.Show("指定されたユーザー名のファイルが存在しませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("ユーザー名を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}