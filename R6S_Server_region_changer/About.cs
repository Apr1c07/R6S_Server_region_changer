using System.Windows.Forms;

namespace R6S_Server_region_changer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/Apr1c07");
        }

        private void About_Load(object sender, System.EventArgs e)
        {
            label2.Text ="       【R6S Server region changer : Ver "
                +System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion.ToString()
                +"】";
        }
    }
}