using Bookkeeping.BLL;
using Bookkeeping.Common;
using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping
{
    public partial class Register : Form
    {
        public static string photoPath; //图片路径
        public byte[] bytes;    //图片流
        string mailVerificationCode;    //验证码
        int time1 = 60; //发送邮件的时间为1分钟
        int time2 = 60 * 5; //验证码的有效时间为5分钟

        public Register()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetVerificationCode_Click(object sender, EventArgs e)
        {
            mailVerificationCode = MailVerificationCode.CreateMailVerificationCode(6);
            //获取收件人邮箱
            string receiveMail = txtMail.Text.Trim();
            //发件人邮箱
            string sendMail = "********@qq.com";   //这里输入你用来发送邮件的邮箱
            //标题
            string subject = "记账本邮箱验证码";
            //内容
            string body = "亲爱的用户：\r\n您正在验证身份，验证码是：" +
                mailVerificationCode + "\r\n5分钟内有效，为了您的帐号安全，请勿泄露和转发。如果非本人操作，请忽略。" +
                "\r\n服务由记账本提供。";
            //邮箱授权码
            string authorizationCode = "********";
            //判断邮箱是否正确，同时发送验证码
            if (string.IsNullOrEmpty(receiveMail))
            {
                MessageBox.Show("邮箱不能为空，请输入邮箱。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMail.Focus();
            }
            else if (!MailVerificationCode.CheckMail(receiveMail))
            {
                MessageBox.Show("输入的邮箱有误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMail.Focus();
            }
            else
            {
                if (MailVerificationCode.SendMail(sendMail, receiveMail, subject, body, authorizationCode))
                {
                    btnGetVerificationCode.Enabled = false;
                    timer1.Interval = 1000;
                    timer1.Start();
                    timer2.Interval = 1000;
                    timer2.Start();
                }
                else
                {
                    txtMailVerificationCode.Focus();
                }
            }
        }

        /// <summary>
        /// 一分钟计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(time1 > 0)   //一分钟以内不能发送验证码
            {
                time1--;
                btnGetVerificationCode.Text = "重新发送" + time1.ToString() + "s";
            }
            else   //超过一分钟才可以重新发送验证码
            {
                timer1.Stop();
                btnGetVerificationCode.Text = "重新获取验证码";
                btnGetVerificationCode.Enabled = true;
            }
        }

        /// <summary>
        /// 五分钟计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {

            if(time2 == 0)  //五分钟到了重新创建验证码
            {
                timer2.Stop();
                mailVerificationCode = MailVerificationCode.CreateMailVerificationCode(6);
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(txtName.Text.Trim() != "" && txtPassword.Text.Trim() != "")
            {
                string userName = txtName.Text.Trim();
                string userPassword = txtPassword.Text.Trim();
                string userMail = txtMail.Text.Trim();
                string userMailVerificationCode = txtMailVerificationCode.Text.Trim();
                UserInfoBll bll = new UserInfoBll();
                if(bll.ExistUserName(userName))
                {
                    MessageBox.Show("该用户名已存在，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if(string.IsNullOrEmpty(userMailVerificationCode))
                    {
                        MessageBox.Show("请输入验证码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtMailVerificationCode.Focus();
                        return;
                    }
                    else if(userMailVerificationCode != mailVerificationCode)
                    {
                        MessageBox.Show("验证码错误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtMailVerificationCode.Focus();
                        return;
                    }
                    long userId = bll.ExistUserId();
                    UserInfo userInfo = new UserInfo();
                    userInfo.UserID = userId;
                    userInfo.UserName = userName;
                    userInfo.UserPassword = userPassword;
                    userInfo.UserMail = userMail;
                    userInfo.UserPhoto = bytes;
                    if(bytes == null)
                    {
                        MessageBox.Show("请添加头像", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        picPhoto.Focus();
                        return;
                    }
                    else
                    {
                        if (bll.AddUserInfo(userInfo))
                        {
                            MessageBox.Show("注册成功！");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("注册失败！");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请将信息填写完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 添加头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picPhoto_Click(object sender, EventArgs e)
        {
            //文件对话框的初始目录为桌面
            this.openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            this.openFileDialog1.Filter = "JPEG(*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg|GIF(*.gif)|*.gif|PNG(*.png)|*.png|BMP(*.bmp)|*.bmp";
            this.openFileDialog1.FilterIndex = 0;
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Title = "选择头像";
            this.openFileDialog1.CheckPathExists = true;
            if(this.openFileDialog1.ShowDialog() == DialogResult.OK)    //图像设置了128×128大小，选择的图像超过这个大小会显示不全。
            {
                photoPath = this.openFileDialog1.FileName;
                FileStream fs = new FileStream(photoPath, FileMode.Open, FileAccess.Read);
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                picPhoto.Image = Image.FromStream(fs);
                picPhoto.BringToFront();
                fs.Close();
            }
        }

        /// <summary>
        /// 显示密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPassword.Checked)
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
