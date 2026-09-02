using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnyKey
{
    public partial class Form1 : Form
    {
        Game1 Game_Parent;

        public Form1(Game1 parent)
        {
            InitializeComponent();
            Game_Parent = parent;
        }

        public string get_Level_Name()
        {
            return listBox1.SelectedItem.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(Game_Parent.Level.all_Levels());
        }
    }
}