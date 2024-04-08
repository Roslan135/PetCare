using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace PetFriends
{
    public partial class RegLog : Form
    {
        public RegLog()
        {
            InitializeComponent();
        }
        //Database connection
        static string cone = ConfigurationManager.ConnectionStrings["PFC"].ConnectionString;
        SqlConnection Con = new SqlConnection(cone);
        //RegLog close button
        private void closeBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RegLog_Load(object sender, EventArgs e)
        {
            VisibleComponents();
        }
        private void VisibleComponents()
        {
            //Register/login form
            regPanel.Visible = false;
            uaeMess.Visible = false;
            pdneMess.Visible = false;
            siP.Visible = false;
            suP.Location = new Point(578, 553);
            suP.Visible = true;
        }
        //Timer form opacity
        private void timerForm_Tick(object sender, EventArgs e)
        {
            if (Opacity == 1)
            {
                timerForm.Stop();
            }
            Opacity += .2;
        }
        //signIn button
        private void signIn_Click(object sender, EventArgs e)
        {
            if (suP.Visible == false)
            {
                ResetL();
                regPanel.Visible = false;
                logPanel.Visible = true;
                siP.Visible = false;
                suP.Visible = true;
            }
            else
            {
                logPanel.Visible = false;
                regPanel.Visible = true;
                suP.Visible = false;
            }
        }
        //signUp button
        private void signUp_Click(object sender, EventArgs e)
        {
            if (siP.Visible == false)
            {
                ResetR();
                logPanel.Visible = false;
                regPanel.Location = new Point(503, 101);
                regPanel.Visible = true;
                suP.Visible = false;
                siP.Location = new Point(596, 554);
                siP.Visible = true;
            }
            else
            {
                regPanel.Visible = false;
                logPanel.Visible = true;
                siP.Visible = false;
            }
        }
        //Reset registration data
        private void ResetR()
        {
            FName.Text = "";
            UEmail.Text = "";
            UPass.Text = "";
        }
        //Reset login data
        private void ResetL()
        {
            emLog.Text = "";
            passLog.Text = "";
        }
        //Register user button
        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (FName.Text == "" || UEmail.Text == "" || UPass.Text == "")
            {
                MessageBox.Show("Enter the requested information!");
            }
            else
            {
                Con.Open();
                byte[] img = null;
                string querys = "select * from UserTbl where Full_Email ='" + UEmail.Text + "'";
                SqlCommand cmds = new SqlCommand(querys, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmds);
                DataSet ds = new DataSet();
                da.Fill(ds);
                int a;
                a = ds.Tables[0].Rows.Count;
                if (a == 1)
                {
                    uaeMess.Visible = true;
                    TimerError.Start();
                }
                else
                {
                    if (UPass.Text.Length < 6 || UPass.Text.Length > 50)
                    {
                        MessageBox.Show("The password can have a minimum of 6 and a maximum of 50 characters.");
                    }
                    else
                    {
                        string query = "insert into UserTbl values('" + FName.Text + "','" + UEmail.Text + "','" + UPass.Text + "','" + img + "')";
                        SqlCommand cmd = new SqlCommand(query, Con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Profile created.");
                        ResetR();
                        Con.Close();
                    }
                }
                Con.Close();
            }
        }
        //Login user button
        public static string accem;
        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (emLog.Text == "" || passLog.Text == "")
            {
                MessageBox.Show("Enter the requested information!");
            }
            else
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from UserTbl where Full_Email='" + emLog.Text + "' and Full_Password='" + passLog.Text + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                accem = emLog.Text;
                if (dt.Rows[0][0].ToString() == "1")
                {
                    this.Hide();
                    UserDash usd = new UserDash();
                    usd.Show();
                }
                else
                {
                    pdneMess.Visible = true;
                    TimerError2.Start();
                }
            }
            Con.Close();
        }
        //User already exists message
        private void TimerError_Tick(object sender, EventArgs e)
        {
            if (TimerError.Interval == 1800)
            {
                uaeMess.Visible = false;
                TimerError.Stop();
            }
        }
        //Linkedin direct link profile
        private void lnBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/edis-vrtagic22/");
        }
        //YouTube direct link channel
        private void ytBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/c/DonMarquez21");
        }
        //GitHub direct link profile
        private void gitBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/EdisVrtagic");
        }

        private void TimerError2_Tick(object sender, EventArgs e)
        {
            if (TimerError2.Interval == 1800)
            {
                pdneMess.Visible = false;
                TimerError2.Stop();
            }
        }
    }
}
