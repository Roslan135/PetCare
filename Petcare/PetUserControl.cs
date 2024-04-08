using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetFriends
{
    public partial class PetUserControl : UserControl
    {
        public PetUserControl()
        {
            InitializeComponent();
        }
        private Image _ico1, _ico2, _ico3, _ico4;
        private string _petcat, _pettype, _petdesc, _petloc, _post, _contact;
        public event EventHandler OnSelect = null;
        public Image Icon1
        {
            get { return _ico1; }
            set { _ico1 = value; PetPic.Image = value;}
        }
        public Image Icon2
        {
            get { return _ico2;}
            set { _ico2 = value; PetPic2.Image = value; }
        }
        public Image Icon3
        {
            get { return _ico3; }
            set { _ico3 = value; PetPic3.Image = value; }
        }
        public Image Icon4
        {
            get { return _ico4;}
            set { _ico4 = value; PetPic4.Image = value; }
        }
        public string PetCategory
        {
            get { return _petcat; }
            set { _petcat = value; Pet_C.Text = value; }
        }
        public string PetType
        {
            get { return _pettype; }
            set { _pettype = value; Pet_T.Text = value; }
        }
        public string PetDescription
        {
            get { return _petdesc; }
            set { _petdesc = value; Pet_D.Text = value; }
        }
        public string PetLocation
        {
            get { return _petloc; }
            set { _petloc = value; Pet_L.Text = value; }
        }
        public string PetPosted
        {
            get { return _post; }
            set { _post = value; Post_P.Text = value; }
        }
        public string PetContact
        {
            get { return _contact; }
            set { _contact = value; Post_C.Text = value; }
        }
        private void MoreInfoBtn_Click(object sender, EventArgs e)
        {
            OnSelect?.Invoke(this, e);
        }
        private void PetUserControl_Load(object sender, EventArgs e)
        {
            PetPic2.Visible = false;
            PetPic3.Visible = false;
            PetPic4.Visible = false;
        }
    }
}
