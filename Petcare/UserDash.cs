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
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace PetFriends
{
    public partial class UserDash : Form
    {
        public UserDash()
        {
            InitializeComponent();
        }
        //Database connection
        static string conse = ConfigurationManager.ConnectionStrings["PFC"].ConnectionString;
        SqlConnection Con = new SqlConnection(conse);
        //String image location
        string imgLoc = "";
        //Load user info
        public static string emailacc;
        private void UserDash_Load(object sender, EventArgs e)
        {
            VisibleComponents();
            getUserData();
            emailacc = RegLog.accem;
        }
        //Visible components
        private void VisibleComponents()
        {
            closImg1.Visible = false;
            closImg2.Visible = false;
            closImg3.Visible = false;
            closImg4.Visible = false;
            webBrowser1.Visible = false;
            searchLoc.Visible = false;
            searchLocBtn.Visible = false;
            mapType.Visible = false;
            searchLoc.Location = new Point(519, 17);
            searchLocBtn.Location = new Point(863, 17);
            webBrowser1.Location = new Point(263, 99);
            mapType.Location = new Point(519, 58);
            GenerateDynamicPostControl();
            flowLayoutPanel1.Location = new Point(264, 174);
            flowLayoutPanel2.Location = new Point(307, 66);
            flowLayoutPanel2.Visible = false;
            panel2.Location = new Point(542, 19);
            panel3.Location = new Point(398, 6);
            panel4.Location = new Point(357, 33);
            panel4.Visible = false;
            panel3.Visible = false;
        }
        //User data load
        private void getUserData()
        {
            try
            {
                string querys = "select Full_Name,Profile_Img from UserTbl where Full_Email = '" + RegLog.accem + "'";
                Con.Open();
                SqlCommand cmds = new SqlCommand(querys, Con);
                SqlDataReader reader = cmds.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    FullN.Text = reader[0].ToString();
                    byte[] img = (byte[])(reader[1]);
                    if (img == null)
                    {
                        profilePic.Image = null;
                        Con.Close();
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(img);
                        profilePic.Image = Image.FromStream(ms);
                        Con.Close();
                    }
                }
                else
                {
                    Con.Close();
                }
            }
            catch
            {
                Con.Close();
            }

        }
        //Add profile picture button
        private void addProfilePicBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                imgLoc = opendlg.FileName.ToString();
                profilePic.ImageLocation = imgLoc;
                try
                {
                    byte[] images = null;
                    FileStream fs = new FileStream(imgLoc, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    images = br.ReadBytes((int)fs.Length);
                    Con.Open();
                    string query = "update UserTbl set Profile_Img = @images where Full_Email = '" + RegLog.accem + "';";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.Add(new SqlParameter("@images", images));
                    int n = cmd.ExecuteNonQuery();
                    Con.Close();
                }
                catch
                {

                }
            }
        }
        //Logout button
        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegLog rlog = new RegLog();
            rlog.Show();
        }
        //Open image 1
        private void brImg1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                imgLoc = opendlg.FileName.ToString();
                Img1.ImageLocation = imgLoc;
                panImg1.Visible = false;
                closImg1.Visible = true;
            }
        }
        //Open image 2
        private void brImg2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                imgLoc = opendlg.FileName.ToString();
                Img2.ImageLocation = imgLoc;
                panImg2.Visible = false;
                closImg2.Visible = true;
            }
        }
        //Open image 3
        private void brImg3_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                imgLoc = opendlg.FileName.ToString();
                Img3.ImageLocation = imgLoc;
                panImg3.Visible = false;
                closImg3.Visible = true;
            }
        }
        //Open image 4
        private void brImg4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                imgLoc = opendlg.FileName.ToString();
                Img4.ImageLocation = imgLoc;
                panImg4.Visible = false;
                closImg4.Visible = true;
            }
        }
        //Close image 1
        private void closImg1_Click(object sender, EventArgs e)
        {
            Img1.Image = null;
            panImg1.Visible = true;
            closImg1.Visible = false;
        }
        //Close image 2
        private void closImg2_Click(object sender, EventArgs e)
        {
            Img2.Image = null;
            panImg2.Visible = true;
            closImg2.Visible = false;
        }
        //Close image 3
        private void closImg3_Click(object sender, EventArgs e)
        {
            Img3.Image = null;
            panImg3.Visible = true;
            closImg3.Visible = false;
        }
        //Close image 4
        private void closImg4_Click(object sender, EventArgs e)
        {
            Img4.Image = null;
            panImg4.Visible = true;
            closImg4.Visible = false;
        }
        //Posted button
        private void postBtn_Click(object sender, EventArgs e)
        {
            if(PetCatBox.SelectedIndex == -1 || PetTypeBox.SelectedIndex == -1 || PetLocation.Text == "" || PostedBox.Text == "" || DescBox.Text == "" || Img1.Image == null || Img2.Image == null
                || Img3.Image == null || Img4.Image == null)
            {
                MessageBox.Show("Enter the requested information!");
            }
            else
            {
                Con.Open();
                string query = "Insert into PostedTbl(Name_Post,Email_Post,Pet_Category,Pet_Type,Pet_Location,Post_Contact,Pet_Desc,Pet_Img1,Pet_Img2,Pet_Img3,Pet_Img4)values(@Name_Post,@Email_Post,@Pet_Category,@Pet_Type,@Pet_Location,@Post_Contact,@Pet_Desc,@Pet_Img1,@Pet_Img2,@Pet_Img3,@Pet_Img4)";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        Image img1 = Img1.Image, img2 = Img2.Image, img3 = Img3.Image, img4 = Img4.Image;
                        byte[] arr1,arr2,arr3,arr4;
                        ImageConverter converter1 = new ImageConverter();
                        arr1 = (byte[])converter1.ConvertTo(img1, typeof(byte[]));
                        arr2 = (byte[])converter1.ConvertTo(img2, typeof(byte[]));
                        arr3 = (byte[])converter1.ConvertTo(img3, typeof(byte[]));
                        arr4 = (byte[])converter1.ConvertTo(img4, typeof(byte[]));
                        cmd.Parameters.AddWithValue("@Name_Post", FullN.Text);
                        cmd.Parameters.AddWithValue("@Email_Post", emailacc);
                        cmd.Parameters.AddWithValue("@Pet_Category", PetCatBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Pet_Type", PetTypeBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Pet_Location", PetLocation.Text);
                        cmd.Parameters.AddWithValue("@Post_Contact", PostedBox.Text);
                        cmd.Parameters.AddWithValue("@Pet_Desc", DescBox.Text);
                        cmd.Parameters.AddWithValue("@Pet_Img1", arr1.ToArray());
                        cmd.Parameters.AddWithValue("@Pet_Img2", arr2.ToArray());
                        cmd.Parameters.AddWithValue("@Pet_Img3", arr3.ToArray());
                        cmd.Parameters.AddWithValue("@Pet_Img4", arr4.ToArray());
                        cmd.ExecuteNonQuery();
                        ResetData();
                        GenerateDynamicPostControl();
                        Con.Close();
                    }
                }
                catch
                {

                }
            }
        }
        //Reset data
        private void ResetData()
        {
            PetCatBox.SelectedIndex = -1;
            PetTypeBox.SelectedIndex = -1;
            PetLocation.Text = "";
            PostedBox.Text = "";
            DescBox.Text = "";
            Img1.Image = null;
            panImg1.Visible = true;
            closImg1.Visible = false;
            Img2.Image = null;
            panImg2.Visible = true;
            closImg2.Visible = false;
            Img3.Image = null;
            panImg3.Visible = true;
            closImg3.Visible = false;
            Img4.Image = null;
            panImg4.Visible = true;
            closImg4.Visible = false;
        }
        //Add pet button
        private void addPetBtn_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            panel4.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            webBrowser1.Visible = false;
            searchLoc.Visible = false;
            searchLocBtn.Visible = false;
            mapType.SelectedIndex = -1;
            mapType.Visible = false;
        }
        //Home page button
        private void HomePageBtn_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = true;
            flowLayoutPanel2.Visible = false;
            panel4.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            webBrowser1.Visible = false;
            searchLoc.Visible = false;
            searchLocBtn.Visible = false;
            mapType.SelectedIndex = -1;
            mapType.Visible = false;
        }
        //Gallery buttton
        private void GalleryBtn_Click(object sender, EventArgs e)
        {
            GenerateDynamicGallControl();
            flowLayoutPanel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            flowLayoutPanel2.Visible = true;
            panel4.Visible = false;
            webBrowser1.Visible = false;
            searchLoc.Visible = false;
            searchLocBtn.Visible = false;
            mapType.SelectedIndex = -1;
            mapType.Visible = false;
        }
        //Google maps button
        private void GMapsBtn_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            webBrowser1.Visible = true;
            searchLoc.Visible = true;
            searchLocBtn.Visible = true;
            mapType.Visible = true;
        }
        //Reading posts from the database
        public DataTable ReadPostTable()
        {
            string query = "SELECT * FROM PostedTbl";
            SqlCommand cmd = new SqlCommand(query, Con);
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch
            {
                throw;
            }
        }
        //Reading gallery from the database
        public DataTable ReadGalleryTable()
        {
            string query = "SELECT * FROM PostedTbl where Email_Post = '" + RegLog.accem + "'";
            SqlCommand cmd = new SqlCommand(query, Con);
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch
            {
                throw;
            }
        }
        //Dynamic gallery control list
        public void GenerateDynamicGallControl()
        {
            flowLayoutPanel2.Controls.Clear();
            DataTable dt = ReadGalleryTable();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    GalleryUserControl[] listGall = new GalleryUserControl[dt.Rows.Count];
                    for (int i = 0; i < 1; i++)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            listGall[i] = new GalleryUserControl();
                            MemoryStream ms1 = new MemoryStream((byte[])row["Pet_Img1"]);
                            listGall[i].Icons1 = new Bitmap(ms1);
                            MemoryStream ms2 = new MemoryStream((byte[])row["Pet_Img2"]);
                            listGall[i].Icons2 = new Bitmap(ms2);
                            MemoryStream ms3 = new MemoryStream((byte[])row["Pet_Img3"]);
                            listGall[i].Icons3 = new Bitmap(ms3);
                            MemoryStream ms4 = new MemoryStream((byte[])row["Pet_Img4"]);
                            listGall[i].Icons4 = new Bitmap(ms4);
                            flowLayoutPanel2.Controls.Add(listGall[i]);
                        }
                    }
                }
            }
        }
        //Dynamic post control list
        public void GenerateDynamicPostControl()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dt = ReadPostTable();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    PetUserControl[] listPost = new PetUserControl[dt.Rows.Count];
                    for (int i = 0; i < 1; i++)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            listPost[i] = new PetUserControl();
                            MemoryStream ms1 = new MemoryStream((byte[])row["Pet_Img1"]);
                            listPost[i].Icon1 = new Bitmap(ms1);
                            MemoryStream ms2 = new MemoryStream((byte[])row["Pet_Img2"]);
                            listPost[i].Icon2 = new Bitmap(ms2);
                            MemoryStream ms3 = new MemoryStream((byte[])row["Pet_Img3"]);
                            listPost[i].Icon3 = new Bitmap(ms3);
                            MemoryStream ms4 = new MemoryStream((byte[])row["Pet_Img4"]);
                            listPost[i].Icon4 = new Bitmap(ms4);
                            listPost[i].PetCategory = row["Pet_Category"].ToString();
                            listPost[i].PetType = "Category: " + row["Pet_Type"].ToString();
                            listPost[i].PetDescription = "Description: " + row["Pet_Desc"].ToString();
                            listPost[i].PetLocation = "Location: " + row["Pet_Location"].ToString();
                            listPost[i].PetPosted = "Posted by: " + row["Name_Post"].ToString();
                            listPost[i].PetContact = "Contact: " + row["Post_Contact"].ToString();
                            flowLayoutPanel1.Controls.Add(listPost[i]);
                            listPost[i].OnSelect += new EventHandler(this.DetailsControl_Click);
                        }
                    }
                }
            }
        }
        //Animal details
        void DetailsControl_Click(object sender, EventArgs es)
        {
            PetUserControl obj = (PetUserControl)sender;
            Det_Cat.Text = obj.PetType;
            Det_Loc.Text = obj.PetLocation;
            Det_Post.Text = obj.PetPosted;
            Det_Cont.Text = obj.PetContact;
            Det_Desc.Text = obj.PetDescription;
            Det_Pic1.Image = obj.Icon2;
            Det_Pic2.Image = obj.Icon3;
            Det_Pic3.Image = obj.Icon4;
            panel2.Visible = false;
            flowLayoutPanel1.Visible = false;
            panel4.Visible = true;
        }
        //Search box type
        private void searchBoxType_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in flowLayoutPanel1.Controls)
            {
                var wdg = (PetUserControl)item;
                wdg.Visible = wdg.PetType.ToLower().ToLower().Contains(searchBoxType.Text.Trim().ToLower());
            }
        }
        //Search all type
        private void SearchAll_Click(object sender, EventArgs e)
        {
            GenerateDynamicPostControl();
        }
        //Search dog type
        private void SearchDogs_Click(object sender, EventArgs e)
        {
            foreach (var item1 in flowLayoutPanel1.Controls)
            {
                var wdg1 = (PetUserControl)item1;
                wdg1.Visible = wdg1.PetCategory.ToLower().ToLower().Contains(DogsLabel.Text.Trim().ToLower());
            }
        }
        //Search other type
        private void SearchOther_Click(object sender, EventArgs e)
        {
            foreach (var item3 in flowLayoutPanel1.Controls)
            {
                var wdg3 = (PetUserControl)item3;
                wdg3.Visible = wdg3.PetCategory.ToLower().ToLower().Contains(OtherLabel.Text.Trim().ToLower());
            }
        }
        //Search cat type
        private void CatsSearchBox_Click(object sender, EventArgs e)
        {
            foreach (var item2 in flowLayoutPanel1.Controls)
            {
                var wdg2 = (PetUserControl)item2;
                wdg2.Visible = wdg2.PetCategory.ToLower().ToLower().Contains(CatLabel.Text.Trim().ToLower());
            }
        }
        //Close button details animal
        private void closeDetBtn_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel2.Visible = true;
            flowLayoutPanel1.Visible = true;
            mapType.SelectedIndex = -1;

        }
        //Search location google maps
        private void searchLocBtn_Click(object sender, EventArgs e)
        {
            if (searchLoc.Text == "" || mapType.SelectedIndex == -1)
            {
                MessageBox.Show("Enter the requested information!");
            }
            else
            {
                string[] types = new string[] { "m", "k", "h", "p", "e" };
                string url = string.Format("http://maps.google.com/maps?t={0}&q=loc:{1}",
                types[mapType.SelectedIndex], searchLoc.Text);
                webBrowser1.Navigate(url);
            }
        }
    }
}
