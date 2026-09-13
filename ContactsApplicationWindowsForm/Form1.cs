using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContactsBussinesLayer;

namespace ContactsApplicationWindowsForm
{
    public partial class Form1 : Form
    {
        private void _LoadDataContacts()
        {


            DataTable dt = clsContacts.GetAllContact();

            dtgContacts.DataSource = dt;


        }
        private void _FillCountriesInCompoBox()
        {

            DataTable dt = clsCountries.GetAllCountries();


            foreach (DataRow row in dt.Rows)
            {
                comboBox1.Items.Add(row["CountryName"]);

            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            _LoadDataContacts();
            _FillCountriesInCompoBox();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmContacts frm = new frmContacts(-1);
            frm.ShowDialog();

            _LoadDataContacts();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dtgContacts.SelectedRows.Count > 0)
            {

                if (MessageBox.Show("Are you sure delete this Contact ?", "Sure", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    int ID = Convert.ToInt16(dtgContacts.SelectedRows[0].Cells[0].Value);

                    if (clsContacts.DeletContactByID(ID))
                    {

                        MessageBox.Show("the Deleted Contact Is Successfuly", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _LoadDataContacts();
                    }
                    else
                    {
                        MessageBox.Show("the Deleted Contact Is Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
        }
        private void dtgContacts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dtgContacts.ClearSelection();
                dtgContacts.Rows[e.RowIndex].Selected = true;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = 0;

            id = Convert.ToUInt16(dtgContacts.SelectedCells[0].Value);


            frmContacts frm = new frmContacts(id);
            frm.ShowDialog();

            _LoadDataContacts();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _LoadDataContacts();
            DataTable dt = dtgContacts.DataSource as DataTable;

            DataView dataViewCountry = dt.DefaultView;
            string value = comboBox1.SelectedItem.ToString();

            if (comboBox1.SelectedItem.ToString() == "AllCountries")
            {
                dataViewCountry.RowFilter ="";
            }
            else
            {
                int CountryID = Convert.ToInt16(clsCountries.Find(value).CountryID);


                dataViewCountry.RowFilter = $"CountryID={CountryID}";

            }

                

            dtgContacts.DataSource = dataViewCountry;


        }
    }





}



        
    

