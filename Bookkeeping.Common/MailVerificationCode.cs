using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bookkeeping.Common
{
    public static class MailVerificationCode
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateMailVerificationCode(int length)
        {
            char code;
            int num;
            string mailVerificationCode = string.Empty;
            Random random = new Random();

            //生成指定长度的验证码
            //验证码可以包括大写字母、小写字母以及数字，需求不同可以更改取余的数字
            for(int i = 0; i < length; i++)
            {
                num = random.Next();
                if(num % 3 == 0)
                {
                    code = (char)('0' + (char)(num % 10));
                }
                else if(num % 3 == 1)
                {
                    code = (char)('A' + (char)(num % 26));
                }
                else
                {
                    code = (char)('a' + (char)(num % 26));
                }
                mailVerificationCode += code.ToString();
            }
            return mailVerificationCode;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="SendMail"></param>
        /// <param name="ReceiveMail"></param>
        /// <param name="MailSubject"></param>
        /// <param name="MailBody"></param>
        /// <param name="AuthorizationCode"></param>
        /// <returns></returns>
        public static bool SendMail(string SendMail, string ReceiveMail, string MailSubject, string MailBody, string AuthorizationCode)
        {
            //实例化一个发送邮件类
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址
            mailMessage.From = new MailAddress(SendMail);
            //收件人邮箱地址
            mailMessage.To.Add(new MailAddress(ReceiveMail));
            //邮件标题及内容
            mailMessage.Subject = MailSubject;
            mailMessage.Body = MailBody;

            //实例化一个SmtpClient类
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.qq.com";
            client.Port = 587;

            //安全加密
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //验证发件人身份
            client.Credentials = new NetworkCredential(SendMail, AuthorizationCode);

            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证QQ邮箱
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static bool CheckMail(string mail)
        {
            string str = @"^[1-9][0-9]{4,}@qq.com$";    //这里验证的QQ邮箱只有数字组成
            Regex regex = new Regex(str);
            if(regex.IsMatch(mail))
            {
                return true;
            }
            return false;
        }
    }
}
