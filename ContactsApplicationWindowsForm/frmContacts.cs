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
    public partial class frmContacts : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode = enMode.AddNew;
        int _ContactID;
        clsContacts _contact;

        void _FillCountriesInComboBox()
        {

            DataTable dt = clsCountries.GetAllCountries();

            foreach (DataRow c in dt.Rows)
            {
                cmbCountries.Items.Add(c["CountryName"]);

            }

        }
        public frmContacts(int ContactID)
        {

            InitializeComponent();

            if (ContactID == -1)
            {

                _Mode = enMode.AddNew;

            }
            else
            {
                _Mode = enMode.Update;

                _ContactID = ContactID;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmContacts_Load(object sender, EventArgs e)
        {

            _FillCountriesInComboBox();

            switch (_Mode)
            {
                case enMode.AddNew:
                    _contact = new clsContacts();
                    lbContact.Text = "Add New Contact";


                    break;
                case enMode.Update:

                    _contact = clsContacts.Find(_ContactID);

                    //shut down edit Page Contact because the contact is Not found
                    if (_contact == null)
                    {

                        MessageBox.Show($"The Contact with iD {_ContactID} is Not Found", "Erorr");
                        this.Close();
                    }
                    lbContact.Text = "Edit Contat With ID = " + _contact.ContactID;
                    lbID.Text = _contact.ContactID.ToString();
                    txtFirstName.Text = _contact.FirstName;
                    txtLastName.Text = _contact.LastName;
                    txtEmail.Text = _contact.Email;
                    txtPhone.Text = _contact.Phone;
                    DTPDateOfBirth.Value = _contact.DateOfBirth;
                    cmbCountries.SelectedIndex = cmbCountries.FindString(clsCountries.Find(_contact.CountryID).CountryName);
                    txtAddress.Text = _contact.Address;

                    if (_contact.ImagePath != "")
                    {

                        pictureBox1.Load(_contact.ImagePath);
                    }

                    liblRemoveImage.Visible = (_contact.ImagePath != "");

                    break;
            }

        }

        private void libl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                pictureBox1.Load(openFileDialog1.FileName);
                _contact.ImagePath = openFileDialog1.FileName;

                liblRemoveImage.Visible = true;

            }


        }

        private void liblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            liblRemoveImage.Visible = false;
            pictureBox1.Image = null;
            _contact.ImagePath = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            // validation

            bool isValidation = true;


            if (txtFirstName.Text == String.Empty)
            {
                isValidation = false;

                errorProvider1.SetError(txtFirstName, "First Name Is Empty");

            }

            if (txtLastName.Text == String.Empty)
            {
                isValidation = false;
                errorProvider1.SetError(txtLastName, "Last Name Is Empty");
            }

            if (txtEmail.Text == String.Empty)
            {
                isValidation = false;
                errorProvider1.SetError(txtEmail, "Email Is Empty");

            }

            // Check if Email Is Right

            if (!txtEmail.Text.Contains("@") && txtEmail.Text != String.Empty)
            {
                isValidation = false;

                errorProvider1.SetError(txtEmail, "Enter Right Email With @");

            }

            if (!txtPhone.Text.All(char.IsDigit))
            {
                isValidation = false;
                errorProvider1.SetError(txtPhone, "Enter Number ");

            }

            if (txtAddress.Text == String.Empty)
            {
                isValidation = false;

                errorProvider1.SetError(txtAddress, "Fill Address");
            }

            // Check if Erorr in Validation
            if (!isValidation)
            {
                MessageBox.Show("Erorr Becaue Erorr in Data Entry", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // save After check Validation 

           _contact.FirstName = txtFirstName.Text;
            _contact.LastName = txtLastName.Text;
            _contact.Email = txtEmail.Text;
            _contact.Phone = txtPhone.Text;
            _contact.DateOfBirth = DTPDateOfBirth.Value;

            _contact.CountryID = clsCountries.Find(cmbCountries.Text).CountryID;
            _contact.Address = txtAddress.Text;
            




            switch (_Mode)
            {

                case enMode.AddNew:

                    if (_contact.Save())
                    {
                        _Mode = enMode.Update;
                        MessageBox.Show("The Contact Is Inserted Successfuly","Sucess",MessageBoxButtons.OK, MessageBoxIcon.Information);

                        lbContact.Text = $"Edit Contact With ID = {_contact.ContactID}";
                        lbID.Text  = _contact.ContactID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("the Contacts is Not Save ","Erorr",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                  break;


                     
                      case enMode.Update:
                    if (_contact.Save())
                    {
                        MessageBox.Show("The Contact Is Updated Successfuly", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
                    else
                    {
                        MessageBox.Show("the Contacts is Not Save ", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                        break;
            }



        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {

                e.Handled = true;

            }

            }

        private void txtJustCharacted_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

       
    }
    } 
        
    
