using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserInfo;
using System.Threading;
using SplashScreen;

namespace app_xioma
{
    public partial class Form1 : Form
    {

        private List<UserInf> allUserInfo;
        private UserInf[] userInfo;
        private UserInf currentObject;
        private string inputValue;
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(StartForm));
            t.Start();
            Thread.Sleep(5000);
            InitializeComponent();
            GetAllUserInfo();
            t.Abort();
    
        }


        public void StartForm()
        {
            Application.Run(new frmSplsashScreen());
        }
        private void GetAllUserInfo()
        {
            HttpUtils.SendHttpGetRequest("http://localhost:5000/api/UserInfos/pull", (ls) =>
            {

                userInfo = JsonConvert.DeserializeObject<UserInf[]>(ls);

                allUserInfo = userInfo.OfType<UserInf>().ToList();

                Action action = () => {
                    dataGridView1.DataSource = allUserInfo;
                };
                this.Invoke(action);

            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmAddEditDelete frm = new FrmAddEditDelete(null);
            this.Hide();
            frm.ShowDialog();
            GetAllUserInfo();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            currentObject = (UserInf)dataGridView1.CurrentRow.DataBoundItem;
            FrmAddEditDelete frm = new FrmAddEditDelete(currentObject);
            this.Hide();
            frm.ShowDialog();
            currentObject = null;
            GetAllUserInfo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            currentObject = (UserInf)dataGridView1.CurrentRow.DataBoundItem;
            if (MessageBox.Show("Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                HttpUtils.SendHttpPostRequest("http://localhost:5000/api/UserInfos/delete", (f) =>
                {
                    bool isTrue = Convert.ToBoolean(f);
                    if (isTrue)
                    {
                        MessageBox.Show("Successfully deleted");
                        Action action1 = () =>
                        {
                            currentObject = null;
                            GetAllUserInfo();
                        };
                        this.Invoke(action1);
                    }
                    else
                    {
                        MessageBox.Show("Error");
      

                    }
                }, Newtonsoft.Json.JsonConvert.SerializeObject(new UserInf { Id=currentObject.Id }));
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            inputValue = textBox1.Text;

            if (inputValue.Length != 0)
            {
               
                dataGridView1.DataSource = userInfo.Where(x => x.Id.ToString().Contains(textBox1.Text)).ToList();
            }
            else
            {
                MessageBox.Show("Please insert the value into the  text box to find a specific record");
                dataGridView1.DataSource = allUserInfo;

            }
        }
    }
}
