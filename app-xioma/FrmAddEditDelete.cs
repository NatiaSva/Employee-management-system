using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserInfo;

namespace app_xioma
{
    public partial class FrmAddEditDelete : Form
    {
        private int id;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string role;
        private string department;
        private DateTime dateOfEntry;
        private string remarks;
        private string address;
        private UserInf user;
        private UserInf[] users;
    

        public FrmAddEditDelete(UserInf obj)
        {
            InitializeComponent();
            if (obj != null)
            {
                user = obj;
                GetUserInfo();
            }
        }


        private void ResetUserInfo()
        {
            txtId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtDateOfBirth.Text = "";
            txtRole.Text = "";
            txtDepartment.Text = "";
            txtDateOfEntry.Text = "";
            txtRemarks.Text = "";
            txtAddress.Text = "";
            ActiveControl = txtId;
        }


        private void GetUserInfo()
        {
            txtId.Text = user.Id.ToString();
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtDateOfBirth.Text = user.DateOfBirth.ToString();
            txtRole.Text = user.Role;
            txtDepartment.Text = user.Department;
            txtDateOfEntry.Text = user.DateOfEntry.ToString();
            txtRemarks.Text = user.Remarks;
            txtAddress.Text = user.Address;

        }

        private void FrmAddEditDelete_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is Form1)
                {
                    frm.Show();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (user != null)
            {
                GetUserInfo();
                return;
            }
            try
            {
                id = int.Parse(txtId.Text);
            }
            catch
            {
                MessageBox.Show("You must insert ID");
                return;
            }

            firstName = txtFirstName.Text;
            lastName = txtLastName.Text;
            dateOfBirth = DateTime.Parse(txtDateOfBirth.Text);
            role = txtRole.Text;
            department = txtDepartment.Text;
            dateOfEntry = DateTime.Parse(txtDateOfEntry.Text);
            remarks = txtRemarks.Text;
            address = txtAddress.Text;

            if (firstName.Length == 0 || lastName.Length == 0 || role.Length == 0 || department.Length == 0 || address.Length == 0)
            {
                MessageBox.Show("You must fill all the fields");
                return;
            }
            else
            {

     
                HttpUtils.SendHttpGetRequest("http://localhost:5000/api/UserInfos/pull", (ls) =>
                {

                    users = JsonConvert.DeserializeObject<UserInf[]>(ls);

                    for (int i = 0; i < users.Length; i++)
                    {
                        if (users[i].Id == id)
                        {

                            MessageBox.Show("This ID is exists in the system");
                            Action action = () =>
                            {

                                txtId.Text = "";
                            };
                            this.Invoke(action);
                            return;
                        }
                    }

                    Action action1 = () =>
                    {
                        AddUserInfo();
                    };
                    this.Invoke(action1);

                });

             


            }
        }

        private void AddUserInfo()
        {
            HttpUtils.SendHttpPostRequest("http://localhost:5000/api/UserInfos/insert", (f) =>
            {
                bool isTrue = Convert.ToBoolean(f);
                if (isTrue)
                {
                    MessageBox.Show("Successfully inserted");

                    Action action = () =>
                    {
                        ResetUserInfo();
                        Close();
                    };
                    this.Invoke(action);
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }, Newtonsoft.Json.JsonConvert.SerializeObject(new UserInf { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth, Role = role, Department = department, DateOfEntry = dateOfEntry, Remarks = remarks, Address = address }));



        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (user == null)
            {
                MessageBox.Show("Please insert all fields and press Add button");
                return;
            }
            try
            {
                id = int.Parse(txtId.Text);
            }
            catch
            {
                MessageBox.Show("You must insert ID");
                return;
            }
            firstName = txtFirstName.Text;
            lastName = txtLastName.Text;
            dateOfBirth = DateTime.Parse(txtDateOfBirth.Text);
            role = txtRole.Text;
            department = txtDepartment.Text;
            dateOfEntry = DateTime.Parse(txtDateOfEntry.Text);
            remarks = txtRemarks.Text;
            address = txtAddress.Text;

            if (firstName.Length == 0 || lastName.Length == 0 || role.Length == 0 || department.Length == 0 || address.Length == 0)
            {
                MessageBox.Show("You must fill all the fields");
                return;
            }

            HttpUtils.SendHttpPostRequest("http://localhost:5000/api/UserInfos/update", (f) =>
            {
                bool isTrue = Convert.ToBoolean(f);
                if (isTrue)
                {
                    MessageBox.Show("Successfully updated");
                    Action action = () =>
                    {
                        user = null;
                        ResetUserInfo();
                        Close();
                    };
                    this.Invoke(action);
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }, Newtonsoft.Json.JsonConvert.SerializeObject(new UserInf { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth, Role = role, Department = department, DateOfEntry = dateOfEntry, Remarks = remarks, Address = address }));
        }
    


    }
}
