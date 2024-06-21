using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KOZELDOM
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Создание экземпляра игровой формы
            GameForm gameForm = new GameForm();

            

            // Покажите игровую форму
            gameForm.Show();

            // Скрыть текущую
            this.Hide(); 
        }
    }
}
