using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace InstaPhoto
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// class 
        /// </summary>
        Execute _ex = new Execute();


        /// <summary>
        /// Varible
        /// </summary>
        string Tag_name = "";
        string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        AddInDatanase _add = new AddInDatanase();
        /// <summary>
        /// Action
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
        }

        
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
           
        }


        private void btn_search_Click(object sender, EventArgs e)
        {
            if (Execute.HasConnection())
            {
                Tag_name = txt_tag.Text.Trim();
                GCLass.Tag_Name = Tag_name;
                GCLass.Table_name = Tag_name;
                _add.createTable(Tag_name);
                path = path + "\\InstaPhoto\\" + Tag_name;
                GCLass.Path = path;
                GCLass._path_ID = path;
                _ex.Create_folder(path);
                if (!File.Exists(GCLass._path_ID + "//ID.txt"))
                {
                    Execute.Savevarible(GCLass.Path, "0");
                }
                _ex.List_information(Tag_name);

                lbl_internet.Text = "Internet status: good";
                dgv_list.DataSource = _add.ListPicture(Tag_name);
                Timer.Start();
                lbl_count.Text = dgv_list.RowCount.ToString();
                btn_search.Enabled = false;
            }
            else
            {
               lbl_internet.Text = "Internet status: Connection lost";
                MessageBox.Show("Connection Lost Please Check Internet connection", "InstaPhoto Error Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(Execute.HasConnection())
            {
                
                Invoke(new RefreshReceivedDgv(RfshReceivedDgv), dgv_list);
                
                lbl_count.Text = "Number of photo: " + (dgv_list.RowCount - 1).ToString();
                lbl_internet.Text = "Internet status: good";
            }
            else
            {
                lbl_internet.Text = "Internet status: Connection lost";
            }
           
        }

        public delegate void RefreshReceivedDgv(DataGridView dgvRequest);
        private void RfshReceivedDgv(DataGridView dgvRequest)
        {
            dgvRequest.DataSource = null;
            _ex.List_timer(Tag_name);
            dgvRequest.DataSource = _add.ListPicture(Tag_name);
        }
    }
}
