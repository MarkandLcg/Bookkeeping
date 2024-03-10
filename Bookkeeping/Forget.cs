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
    public partial class Forget : Form
    {
        string mailVerificationCode;    //验证码
        int time1 = 60; //发送时间倒计时
        int time2 = 60 * 5; //验证码有效时间

        public Forget()
        {
            InitializeComponent();
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
            string sendMail = "********@qq.com";  //这里输入你用来发送邮件的邮箱
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
        /// 显示密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if(ckbPassword.Checked)
            {
                txtNewPassword.PasswordChar = new char();
                txtConfirmPassword.PasswordChar = new char();
            }
            else
            {
                txtNewPassword.PasswordChar = '*';
                txtConfirmPassword.PasswordChar = '*';
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            string userMailVerificationCode = txtMailVerificationCode.Text.Trim();
            string userPassword = txtNewPassword.Text.Trim();
            if(txtMail.Text.Trim() != "" && txtNewPassword.Text.Trim() != "" && txtConfirmPassword.Text.Trim() != "")
            {
                string userMail = txtMail.Text.Trim();
                UserInfoBll bll = new UserInfoBll();
                if(!bll.ExistUserMail(userMail))
                {
                    MessageBox.Show("用户还未注册！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(string.IsNullOrEmpty(userMailVerificationCode))
                {
                    MessageBox.Show("请输入验证码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMailVerificationCode.Focus();
                    return;
                }
                else if(userMailVerificationCode != mailVerificationCode)
                {
                    MessageBox.Show("验证码错误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMailVerificationCode.Focus();
                    return;
                }
                if(txtNewPassword.Text.Trim() == txtConfirmPassword.Text.Trim())
                {
                    if(bll.ForgetPassword(userPassword, userMail))
                    {
                        MessageBox.Show("密码修改成功！");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("密码修改失败！");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("两次输入密码不同，请确认密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if(txtMail.Text.Trim() == "")
            {
                MessageBox.Show("邮箱不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMail.Focus();
                return;
            }
            else if(txtNewPassword.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNewPassword.Focus();
            }
            else
            {
                MessageBox.Show("请输入完整信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMail.Focus();
                txtMailVerificationCode.Focus();
                txtNewPassword.Focus();
                txtConfirmPassword.Focus();
            }
        }
    }
}
