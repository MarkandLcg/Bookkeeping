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
    public partial class Bookkeeping : Form
    {
        private string userName;    //用户名
        private string userId;  //用户Id
        private string userMail;    //邮箱
        private string userPassword;    //密码

        private float inMoney;  //收入
        private float outMoney; //支出
        private float totalMoney;   //总计
        private UserInfoBll bll;
        private RecordInfoBll recordInfoBll;
        private string strInMoney = "收入";
        private string strOutMoney = "支出";
        private int inMoneyNumber;  //收入的项数
        private int outMoneyNumber; //支出的项数
        private DataTable dtInMoney;    //收入数据表格
        private DataTable dtOutMoney;   //支出数据表格
        private List<int> dtInMoneyRowList = new List<int>();   //收入表格行集合
        private List<int> dtOutMoneyRowList = new List<int>();  //支出表格行集合

        //DataTable loadDataDtInMoney;   //收入数据表格
        //DataTable loadDataDtOutMoney;   //支出数据表格
        private bool checkInMoney = false;  //收入是否选中
        private bool checkOutMoney = false; //支出是否选中

        private int pageSizeInMoney = 0;   //收入每页显示行数
        private int pageCountInMoney = 0;  //收入页数
        int pageCurrentInMoney = 0; //收入当前页号
        private int currentRowInMoney = 0; //收入当前记录行

        private int pageSizeOutMoney = 0;   //支出每页显示行数
        private int pageCountOutMoney = 0;  //支出页数
        int pageCurrentOutMoney = 0;    //支出当前页号
        private int currentRowOutMoney = 0; //支出当前记录行

        public static string photoPath; //图片路径
        public byte[] bytes;    //图片流

        public Bookkeeping()
        {
            InitializeComponent();
        }

        public Bookkeeping(string userName)
        {
            this.userName = userName;
            InitializeComponent();
        }

        /// <summary>
        /// 加载记账本页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bookkeeping_Load(object sender, EventArgs e)
        {
            dataGridViewOutMoney.ClearSelection();  //取消默认选中行
            dataGridViewInMoney.ClearSelection();  //取消默认选中行
            Init(userName);
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="userName"></param>
        public void Init(string userName)
        {
            inMoney = 0;
            outMoney = 0;
            totalMoney = 0;
            bll = new UserInfoBll();
            userId = bll.ExistUserInfo(userName)[0];    //获取用户Id
            userMail = bll.ExistUserInfo(userName)[1];  //获取邮箱
            labelName.Text = userName;
            labelId.Text = userId;
            labelMail.Text = userMail;
            
            bytes = bll.ExistUserPhoto(userName);   //获取头像
            MemoryStream ms = new MemoryStream(bytes);
            picPhoto.Image = Image.FromStream(ms);

            DataTableRefresh();

            //loadDataDtInMoney = dtInMoney;
            //loadDataDtOutMoney = dtOutMoney;
            /*InitDataTableOutMoney(dtOutMoney, outMoneyNumber);
            LoadDataOutMoney(dtOutMoney, outMoneyNumber);
            InitDataTableInMoney(dtInMoney, inMoneyNumber);
            LoadDataInMoney(dtInMoney, inMoneyNumber);*/
        }

        /// <summary>
        /// 初始化收入表格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="itemCount"></param>
        private void InitDataTableInMoney(DataTable dt, int itemCount)
        {
            pageSizeInMoney = 6;   //设置页面行数
            pageCountInMoney = (itemCount / pageSizeInMoney);    //计算出总页数
            if(itemCount % pageSizeInMoney > 0)
            {
                pageCountInMoney++;
            }
            pageCurrentInMoney = 1;  //当前页数从1开始
            currentRowInMoney = 0; //当前记录数从0开始
        }

        /// <summary>
        /// 初始化支出表格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="itemCount"></param>
        private void InitDataTableOutMoney(DataTable dt, int itemCount)
        {
            pageSizeOutMoney = 6;   //设置页面行数
            pageCountOutMoney = (itemCount / pageSizeOutMoney);    //计算出总页数
            if (itemCount % pageSizeOutMoney > 0)
            {
                pageCountOutMoney++;
            }
            pageCurrentOutMoney = 1;  //当前页数从1开始
            currentRowOutMoney = 0; //当前记录数从0开始
        }

        /// <summary>
        /// 加载支出表格数据
        /// </summary>
        /// <param name="dt"></param>
        private void LoadDataOutMoney(DataTable dt, int itemCount)
        {
            int currentStartRow = 0;    //当前页面开始记录行
            int currentEndRow = 0; //当前页面结束记录行
            if(dt == null)
            {
                return;
            }
            DataTable dtTemp = dt.Clone();  //克隆dt
            if(pageCurrentOutMoney == pageCountOutMoney)
            {
                currentEndRow = itemCount;
            }
            else
            {
                currentEndRow = pageSizeOutMoney * pageCurrentOutMoney;
            }
            currentStartRow = currentRowOutMoney;
            labelOutMoneyTotalPage.Text = pageCountOutMoney.ToString(); //页面总数
            txtOutMoneyCurrentPage.Text = pageCurrentOutMoney.ToString();   //当前页面

            //从元数据源复制记录行
            for(int i = currentStartRow; i < currentEndRow; i++)
            {
                dtTemp.ImportRow(dt.Rows[i]);
                currentRowOutMoney++;
            }
            dataGridViewOutMoney.DataSource = dtTemp;
            dataGridViewOutMoney.AllowUserToAddRows = false;

            dataGridViewOutMoney.ClearSelection();  //取消默认选中行
        }

        /// <summary>
        /// 加载收入表格数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="itemCount"></param>
        private void LoadDataInMoney(DataTable dt, int itemCount)
        {
            int currentStartRow = 0;    //当前页面开始记录行
            int currentEndRow = 0;  //当前页面结束记录行
            if(dt == null )
            {
                return;
            }    
            DataTable dtTemp = dt.Clone();  //克隆dt
            if (pageCurrentInMoney == pageCountInMoney)
            {
                currentEndRow = itemCount;
            }
            else
            {
                currentEndRow= pageSizeInMoney * pageCurrentInMoney;
            }
            currentStartRow = currentRowInMoney;
            labelInMoneyTotalPage.Text = pageCountInMoney.ToString();  //页面总数
            txtInMoneyCurrentPage.Text = pageCurrentInMoney.ToString();    //当前页面

            //从元数据源复制记录行
            for (int i = currentStartRow; i< currentEndRow; i++)
            {
                dtTemp.ImportRow(dt.Rows[i]);
                currentRowInMoney++;
            }
            dataGridViewInMoney.DataSource = dtTemp;
            dataGridViewInMoney.AllowUserToAddRows= false;

            dataGridViewInMoney.ClearSelection();   //取消默认选中行
        }

        /// <summary>
        /// 支出上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutMoneyPreviousPage_Click(object sender, EventArgs e)
        {
            pageCurrentOutMoney--;
            if(pageCurrentOutMoney <= 0)
            {
                MessageBox.Show("已经是第一页，请点击下一页查看！");
                return;
            }
            else
            {
                currentRowOutMoney = pageSizeOutMoney * (pageCurrentOutMoney - 1);
            }
            LoadDataOutMoney(dtOutMoney, outMoneyNumber);
        }

        /// <summary>
        /// 支出下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutMoneyNextPage_Click(object sender, EventArgs e)
        {
            pageCurrentOutMoney++;
            if(pageCurrentOutMoney > pageCountOutMoney)
            {
                MessageBox.Show("已经是最后一页，请点击上一页查看！");
                return;
            }
            else
            {
                currentRowOutMoney = pageSizeOutMoney * (pageCurrentOutMoney - 1);
            }
            LoadDataOutMoney(dtOutMoney, outMoneyNumber);    
        }

        /// <summary>
        /// 按回车切换支出页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOutMoneyCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) //判断按下的是否为回车键
            {
                pageCurrentOutMoney = Convert.ToInt32(txtOutMoneyCurrentPage.Text.ToString());
                if (pageCurrentOutMoney <= 0 || pageCurrentOutMoney > pageCountOutMoney)
                {
                    MessageBox.Show("页面不存在！");
                    return;
                }
                else
                {
                    currentRowOutMoney = pageSizeOutMoney * (pageCurrentOutMoney - 1);
                    LoadDataOutMoney(dtOutMoney, outMoneyNumber);
                }
            }
        }

        /// <summary>
        /// 收入上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInMoneyPreviousPage_Click(object sender, EventArgs e)
        {
            pageCurrentInMoney--;
            if(pageCurrentInMoney <= 0)
            {
                MessageBox.Show("已经是第一页，请点击下一页查看！");
                return;
            }
            else
            {
                currentRowInMoney = pageSizeInMoney * (pageCurrentInMoney - 1);
            }
            LoadDataInMoney(dtInMoney, inMoneyNumber);
        }

        /// <summary>
        /// 收入下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInMoneyNextPage_Click(object sender, EventArgs e)
        {
            pageCurrentInMoney++;
            if (pageCurrentInMoney > pageCountInMoney)
            {
                MessageBox.Show("已经是最后一页，请点击上一页查看！");
                return;
            }
            else
            {
                currentRowInMoney = pageSizeInMoney * (pageCurrentInMoney - 1);
            }
            LoadDataInMoney(dtInMoney, inMoneyNumber);
        }

        /// <summary>
        /// 按回车切换收入页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInMoneyCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) //判断按下的是否为回车键
            {
                pageCurrentInMoney = Convert.ToInt32(txtInMoneyCurrentPage.Text.ToString());
                if (pageCurrentInMoney <= 0 || pageCurrentInMoney > pageCountInMoney)
                {
                    MessageBox.Show("页面不存在！");
                    return;
                }
                else
                {
                    currentRowInMoney = pageSizeInMoney * (pageCurrentInMoney - 1);
                    LoadDataInMoney(dtInMoney, inMoneyNumber);
                }
            }
        }

        /// <summary>
        /// 点击支出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioBtnOutMoney_CheckedChanged(object sender, EventArgs e)
        {
            checkOutMoney = true;
            checkInMoney = false;
        }

        /// <summary>
        /// 点击收入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioBtnInMoney_CheckedChanged(object sender, EventArgs e)
        {
            checkInMoney = true;
            checkOutMoney = false;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordInfo recordInfo = new RecordInfo();
            recordInfo.UserID = Convert.ToInt64(userId.ToString());
            recordInfo.BillType = txtBillType.Text.ToString();
            if(checkInMoney)
            {
                recordInfo.InOrOut = strInMoney.ToString();
            }
            else if(checkOutMoney)
            {
                recordInfo.InOrOut = strOutMoney.ToString();
            }
            recordInfo.Money = Convert.ToSingle(txtMoney.Text.ToString().Trim());
            recordInfo.Remark = txtRemark.Text.ToString();
            recordInfo.RecordTime = Convert.ToDateTime(pickerRecordTime.Text);
            recordInfoBll = new RecordInfoBll();
            if (recordInfoBll.AddRecordInfo(recordInfo))
            {
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            DataTableRefresh();
        }

        /// <summary>
        /// 刷新数据表格
        /// </summary>
        private void DataTableRefresh()
        {
            recordInfoBll = new RecordInfoBll();
            float[] money = recordInfoBll.ExistMoney(Convert.ToInt64(userId));  //获取支出、收入、总计

            if (money != null)
            {
                inMoney = money[0];
                outMoney = money[1];
                totalMoney = money[2];
                labelInMoney.Text = inMoney.ToString();    //获取收入
                labelOutMoney.Text = outMoney.ToString();   //获取支出
                labelTotalMoney.Text = totalMoney.ToString(); //获取总计
            }
            else
            {
                MessageBox.Show("用户没有记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dtInMoney = recordInfoBll.ExistRecordInfo(strInMoney, Convert.ToInt64(userId));
            if (dtInMoney != null)
            {
                dataGridViewInMoney.DataSource = dtInMoney;
            }
            else
            {
                MessageBox.Show("用户收入记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dtOutMoney = recordInfoBll.ExistRecordInfo(strOutMoney, Convert.ToInt64(userId));
            if (dtOutMoney != null)
            {
                dataGridViewOutMoney.DataSource = dtOutMoney;
            }
            else
            {
                MessageBox.Show("用户支出记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            
            
            if(dtInMoney == null)
            {
                inMoneyNumber = 0;
            }
            else
            {
                inMoneyNumber = dtInMoney.Rows.Count;   //收入的项数
            }

            if(dtOutMoney == null)
            {
                outMoneyNumber = 0;
            }
            else
            {
                outMoneyNumber = dtOutMoney.Rows.Count; //支出的项数
            }

            labelInNumber.Text = inMoneyNumber.ToString();
            labelOutNumber.Text = outMoneyNumber.ToString();

            InitDataTableOutMoney(dtOutMoney, outMoneyNumber);
            LoadDataOutMoney(dtOutMoney, outMoneyNumber);
            InitDataTableInMoney(dtInMoney, inMoneyNumber);
            LoadDataInMoney(dtInMoney, inMoneyNumber);
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            long UserID = Convert.ToInt64(userId);
            string InOrOut = "";
            DateTime RecordTime = Convert.ToDateTime(pickerRecordTime.Text);
            if (checkInMoney)
            {
                InOrOut = strInMoney;
            }
            else if (checkOutMoney)
            {
                InOrOut = strOutMoney;
            }
            float money = recordInfoBll.ExistMoney(UserID, InOrOut, RecordTime);
            if (money != 0)
            {
                txtMoney.Text = money.ToString();
            }
            else
            {
                MessageBox.Show("信息输入错误，没有查询到数据，请选择支出按钮或者收入按钮和正确的日期！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (checkOutMoney)  //删除支出表格记录
            {
                //int outMoneyRowCount = dataGridViewOutMoney.SelectedRows.Count; 
                int outMoneyRowCount = dtOutMoneyRowList.Count();   //支出表格行数
                int outMoneyRow;    //支出表格行
                if (outMoneyRowCount > 0)
                {
                    long UserID = Convert.ToInt64(userId);
                    for (int i = 0; i < outMoneyRowCount; i++)
                    {
                        outMoneyRow = dtOutMoneyRowList[i];
                        RecordInfo recordInfo = new RecordInfo();
                        recordInfo.UserID = UserID;
                        recordInfo.BillType = dataGridViewOutMoney.Rows[outMoneyRow].Cells[0].Value.ToString().Trim();
                        recordInfo.InOrOut = strOutMoney;
                        recordInfo.Money = Convert.ToSingle(dataGridViewOutMoney.Rows[outMoneyRow].Cells[1].Value.ToString().Trim());
                        recordInfo.Remark = dataGridViewOutMoney.Rows[outMoneyRow].Cells[2].Value.ToString().Trim();
                        recordInfo.RecordTime = Convert.ToDateTime(dataGridViewOutMoney.Rows[outMoneyRow].Cells[3].Value.ToString().Trim());
                        if(!recordInfoBll.DeleteRecordInfo(recordInfo))
                        {
                            MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtOutMoneyRowList.Clear();
                    DataTableRefresh();
                }
                else
                {
                    MessageBox.Show("请选择支出或收入按钮和要删除数据的行！", "提示", MessageBoxButtons.OK | MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return;
                }
            }
            else if(checkInMoney)   //删除收入表格记录
            {
                int inMoneyRowCount = dtInMoneyRowList.Count();   //收入表格行数
                int inMoneyRow;    //收入表格行
                if (inMoneyRowCount > 0)
                {
                    long UserID = Convert.ToInt64(userId);
                    for(int i = 0; i < inMoneyRowCount; i++)
                    {
                        inMoneyRow = dtInMoneyRowList[i];
                        RecordInfo recordInfo = new RecordInfo();
                        recordInfo.UserID = UserID;
                        recordInfo.BillType = dataGridViewInMoney.Rows[inMoneyRow].Cells[0].Value.ToString().Trim();
                        recordInfo.InOrOut = strInMoney;
                        recordInfo.Money = Convert.ToSingle(dataGridViewInMoney.Rows[inMoneyRow].Cells[1].Value.ToString().Trim());
                        recordInfo.Remark = dataGridViewInMoney.Rows[inMoneyRow].Cells[2].Value.ToString().Trim();
                        recordInfo.RecordTime = Convert.ToDateTime(dataGridViewInMoney.Rows[inMoneyRow].Cells[3].Value.ToString().Trim());
                        if (!recordInfoBll.DeleteRecordInfo(recordInfo))
                        {
                            MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtInMoneyRowList.Clear();
                    DataTableRefresh();
                }
                else
                {
                    MessageBox.Show("请选择支出或收入按钮和要删除数据的行！", "提示", MessageBoxButtons.OK | MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        /// <summary>
        /// 获得支出数据表格的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewOutMoney_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dtOutMoneyRowList.Contains(e.RowIndex))
            {
                return;
            }
            else
            {
                dtOutMoneyRowList.Add(e.RowIndex);
            }
        }

        /// <summary>
        /// 获得支出数据表格的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewOutMoney_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtOutMoneyRowList.Contains(e.RowIndex))
            {
                return;
            }
            else
            {
                dtOutMoneyRowList.Add(e.RowIndex);
            }
        }

        /// <summary>
        /// 获得收入数据表格的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewInMoney_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtInMoneyRowList.Contains(e.RowIndex))
            {
                return;
            }
            else
            {
                dtInMoneyRowList.Add(e.RowIndex);
            }
        }

        /// <summary>
        /// 获得收入数据表格的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewInMoney_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtInMoneyRowList.Contains(e.RowIndex))
            {
                return;
            }
            else
            {
                dtInMoneyRowList.Add(e.RowIndex);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否退出登录！", "提示", MessageBoxButtons.OK | MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(result == DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogOff_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否注销用户并删除所有信息！", "提示", MessageBoxButtons.OK | MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                long UserID = Convert.ToInt64(userId);
                if(bll.DeleteUserInfo(UserID) && recordInfoBll.DeleteRecordInfo(UserID))
                {
                    MessageBox.Show("注销用户成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("注销用户失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 更换图像
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
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                photoPath = this.openFileDialog1.FileName;
                FileStream fs = new FileStream(photoPath, FileMode.Open, FileAccess.Read);
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                picPhoto.Image = Image.FromStream(fs);
                picPhoto.BringToFront();
                fs.Close();
            }
            long UserID = Convert.ToInt64(userId);
            if (bll.ChangeUserPhoto(UserID, bytes))
            {
                MemoryStream ms = new MemoryStream(bytes);
                picPhoto.Image = Image.FromStream(ms);
                MessageBox.Show("更换头像成功！");
            }
            else
            {
                MessageBox.Show("更换头像失败！");
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("请选择支出按钮或者收入按钮，确定要导出Excel的数据表格！", "提示", MessageBoxButtons.OK | MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            DataTable dt = new DataTable();
            string[] columnName = { "类型", "金额", "备注", "日期"};
            string pathName;
            DateTime dateTime = DateTime.Now;
            if (checkOutMoney)
            {
                dt = dtOutMoney;
                dt.TableName = "支出记录";
            }
            else if(checkInMoney)
            {
                dt = dtInMoney;
                dt.TableName = "收入记录";
            }
            if (result == DialogResult.OK)
            {
                if(dt == null)
                {
                    MessageBox.Show("导出Excel失败！");
                }
                else
                {
                    pathName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + userName + "-" + dt.TableName + "-" + dateTime.ToString("yyyy年MM月dd日 HH时mm分ss秒") + ".xls";
                    DataTableExportExcelHelper.DataTableExportExcel(dt, pathName, columnName);
                    MessageBox.Show("导出Excel成功！");
                }
            }
            else
            {
                return;
            }
        }


    }
}
