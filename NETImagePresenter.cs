using System;
using System.Drawing;
using System.Windows.Forms;

namespace NETImagePresenter
{
    public class MainForm : Form
    {
        private Form form2;
        private ComboBox comboBox;
        private ListBox listBox;
        private StatusBar statusBar;
        private PictureBox pictureBox;
        private Rectangle SelectedScreen;

        public MainForm()
        {
            this.Text = "NET Image Presenter";
            this.Size = new Size(640,240);
            this.MinimumSize = new Size(240,120);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Location = new Point(5,10);
            label.AutoSize = true;
            label.Text = "Drop image files here and show on:";

            comboBox = new ComboBox();
            comboBox.Location = new Point(187,7);
            comboBox.AutoSize = true;
            comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            for (int i = 1; i <= Screen.AllScreens.Length; i++)
            {
                comboBox.Items.Add("Screen " + i);
            }
            comboBox.SelectedIndex = comboBox.Items.Count - 1;

            listBox = new ListBox();
            listBox.Location = new Point(5,30);
            listBox.Size = new Size(615,150);
            listBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            listBox.IntegralHeight = false;
            listBox.AllowDrop = true;

            statusBar = new StatusBar();
            statusBar.Text = "Ready";

            this.Controls.AddRange(new Control[] { label, comboBox, listBox, statusBar });

            SelectedScreen = Screen.AllScreens[Screen.AllScreens.Length-1].WorkingArea;

            form2 = new Form();
            form2.StartPosition = FormStartPosition.Manual;
            form2.Location = SelectedScreen.Location;
            form2.WindowState = FormWindowState.Maximized;
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.BackColor = Color.Black;

            pictureBox = new PictureBox();
            pictureBox.Size = new Size(SelectedScreen.Width, SelectedScreen.Height);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            form2.Controls.Add(pictureBox);

            comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            listBox.Click += new EventHandler(this.listBox_Click);
            listBox.DragEnter += new DragEventHandler(listBox_DragEnter);
            listBox.DragDrop += new DragEventHandler(listBox_DragDrop);
            listBox.KeyDown += new KeyEventHandler(listBox_KeyDown);
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedScreen = Screen.AllScreens[comboBox.SelectedIndex].WorkingArea;
            form2.WindowState = FormWindowState.Normal;
            form2.Location = SelectedScreen.Location;
            form2.WindowState = FormWindowState.Maximized;
            pictureBox.Size = new Size(SelectedScreen.Width, SelectedScreen.Height);
        }

        private void listBox_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string File in FileList)
                listBox.Items.Add(File);
            statusBar.Text = "List contains " + listBox.Items.Count + " items";
        }

        private void listBox_Click(object sender, EventArgs e)
        {
            try {
                pictureBox.Image = Image.FromFile(listBox.SelectedItem.ToString());
                form2.Show();
            } catch {}
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
                listBox.Items.Remove(listBox.SelectedItem);
            statusBar.Text = "List contains " + listBox.Items.Count + " items";
        }
    }

    static class Program
    {
        [STAThread] static void Main()
        {
            Application.Run(new MainForm());
        }
    }
}