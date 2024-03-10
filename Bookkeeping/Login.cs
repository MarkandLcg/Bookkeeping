using Bookkeeping.BLL;
using Bookkeeping.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            labelLoginVerificationCode.Text = LoginVerificationCode.CreateLoginVerificationCode(4);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string passwordMD5 = MD5Helper.GenerateMD5(password);
            UserInfoBll userInfoBll = new UserInfoBll();
            if (!userInfoBll.ExistUserName(userName))
            {
                MessageBox.Show("用户不存在，请注册！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (txtLoginVerificationCode.Text == labelLoginVerificationCode.Text)
            {
                if(userInfoBll.Login(userName, passwordMD5))
                {
                    MessageBox.Show("登录成功");
                    this.Hide();
                    Bookkeeping bookkeeping = new Bookkeeping(userName);
                    bookkeeping.ShowDialog();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if(txtLoginVerificationCode.Text != labelLoginVerificationCode.Text)
            {
                MessageBox.Show("验证码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
        }

        /// <summary>
        /// 更换验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelLoginVerificationCode_Click(object sender, EventArgs e)
        {
            labelLoginVerificationCode.Text = LoginVerificationCode.CreateLoginVerificationCode(4);
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForgetPassword_Click(object sender, EventArgs e)
        {
            Forget forget = new Forget();
            forget.ShowDialog();
        }

        /// <summary>
        /// 显示密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if(ckbPassword.Checked)
            {
                txtPassword.PasswordChar = new char();
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }
    }
}
