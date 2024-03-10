using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Common
{
    public static class LoginVerificationCode
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateLoginVerificationCode(int length)
        {
            char code;
            int num;
            string loginVerificationCode = string.Empty;
            Random random = new Random();

            //生成指定长度的验证码
            //验证码可以包括大写字母、小写字母以及数字，需求不同可以更改取余的数字
            for (int i = 0; i < length; i++)
            {
                num = random.Next();
                if (num % 3 == 0)
                {
                    code = (char)('0' + (char)(num % 10));
                }
                else if (num % 3 == 1)
                {
                    code = (char)('A' + (char)(num % 26));
                }
                else
                {
                    code = (char)('a' + (char)(num % 26));
                }
                loginVerificationCode += code.ToString();
            }
            return loginVerificationCode;
        }
    }
}
