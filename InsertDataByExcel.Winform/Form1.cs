using GetDataByTcp.BLL;
using GetDataByTcp.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace InsertDataByExcel.Winform
{
    public partial class Form1 : Form
    {
        private List<Station_Model> stationList;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            stationList = new List<Station_Model>();
            CboDataTypeInit();
            //CboStationInit();
        }
        private void CboDataTypeInit()
        {
            // 假设有一个包含值和参数的数据源
            List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("5分钟值", 1),
                new KeyValuePair<string, int>("小时值", 2),
                new KeyValuePair<string, int>("日值", 3)
            };

            // 绑定数据源
            cboDataType.DataSource = data;
            // 设置显示值和参数的字段名
            cboDataType.DisplayMember = "Key";
            cboDataType.ValueMember = "Value";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (stationList.Count()==0)
            {
                MessageBox.Show("请选择站点");
                return;
            }
            if (dtpStart.Value.Date > dtpEnd.Value.Date)
            {
                MessageBox.Show("起始日期不允许大于结束日期");
                return;
            }
            if (dtpStart.Value.Year!=dtpEnd.Value.Year)
            {
                MessageBox.Show("起始日期与结束日期不允许跨年");
                return;
            }
            if (cboDataType.SelectedValue.ToString() == "1")
            {
                if (stationList.Count() >1)
                {
                    MessageBox.Show("5分钟值只能选择站点");
                    return;
                }
                Air_BLL air_BLL = new Air_BLL();
                Air_Model_Query query = new Air_Model_Query();
                query.TimePointStart = dtpStart.Value.ToString("yyyy-MM-dd 00:00:00");
                query.TimePointEnd = dtpEnd.Value.ToString("yyyy-MM-dd 23:59:59");
                string stationCode = stationList[0].StationCode;
                string tableName = $"Air_5m_{dtpStart.Value.Year}_{stationCode}A_Src";
                var list = air_BLL.GetAir5mNullDataList(query, tableName, stationCode);
                //MessageBox.Show(list.Count().ToString());
                if (list.Count > 0)
                {
                    ExportExcel(list);
                }
                else
                {
                    MessageBox.Show("该时间段没有缺失数据");
                }
            }
            if (cboDataType.SelectedValue.ToString() == "2")
            {
                Air_BLL air_BLL = new Air_BLL();
                Air_Model_Query query = new Air_Model_Query();
                query.TimePointStart = dtpStart.Value.ToString("yyyy-MM-dd 00:00:00");
                query.TimePointEnd = dtpEnd.Value.ToString("yyyy-MM-dd 23:59:59");
                query.StationCodeList = stationList.Select(s=>s.StationCode).ToList();
                string tableName = $"Air_h_{dtpStart.Value.Year}_pm25_Src";
                var list = air_BLL.GetAirhNullDataList(query, tableName);
                //MessageBox.Show(list.Count().ToString());
                if (list.Count > 0)
                {
                    ExportExcel(list);
                }
                else
                {
                    MessageBox.Show("该时间段没有缺失数据");
                }
            }
            if (cboDataType.SelectedValue.ToString() == "3")
            {
                Air_BLL air_BLL = new Air_BLL();
                Air_Model_Query query = new Air_Model_Query();
                query.TimePointStart = dtpStart.Value.ToString("yyyy-MM-dd 00:00:00");
                query.TimePointEnd = dtpEnd.Value.ToString("yyyy-MM-dd 23:59:59");
                query.StationCodeList = stationList.Select(s => s.StationCode).ToList();
                var list = air_BLL.GetAirDayNullDataList(query);
                //MessageBox.Show(list.Count().ToString());
                if (list.Count > 0)
                {
                    ExportExcel(list);
                }
                else
                {
                    MessageBox.Show("该时间段没有缺失数据");
                }
            }
        }
        private void ImportExcel(string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    Air_BLL bll = new Air_BLL();
                    XSSFWorkbook workbook = new XSSFWorkbook(file);
                    // 获取第一个工作表（可以根据需要修改索引）
                    ISheet sheet = workbook.GetSheetAt(0);
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        try
                        {
                            var row = sheet.GetRow(i);
                            Air_Model model = new Air_Model();
                            model.StationCode = row.GetCell(0).ToString();
                            model.TimePoint = Convert.ToDateTime(row.GetCell(1).ToString());
                            model.Value = Convert.ToDecimal(row.GetCell(2).ToString());
                            model.OperationTime = DateTime.Now;
                            if (cboDataType.SelectedValue.ToString() == "1")
                            {
                                string stationCode = stationList[0].StationCode;
                                if (stationCode==model.StationCode)
                                {
                                    string tableName = $"Air_5m_{dtpStart.Value.Year}_{stationCode}A_Src";
                                    bll.Insert_Air(model, tableName);
                                }
                                else
                                {
                                    txtShowInfo.Text += $"第{i + 1}行保存失败\r\n";
                                    txtShowInfo.Text += $"失败原因：本行数据不属于该站点\r\n";
                                }
                            }
                            if (cboDataType.SelectedValue.ToString() == "2")
                            {
                                string tableName = $"Air_h_{dtpStart.Value.Year}_pm25_Src";
                                bll.Insert_Air(model, tableName);
                            }
                            if (cboDataType.SelectedValue.ToString() == "3")
                            {
                                string tableName = "Air_day_pm25_Src";
                                bll.Insert_Air(model, tableName);
                            }
                            txtShowInfo.Text += $"第{i+1}行保存成功\r\n";
                        }
                        catch (Exception ex)
                        {
                            txtShowInfo.Text += $"第{i+1}行保存失败\r\n";
                            txtShowInfo.Text += $"失败原因：{ex.ToString()}\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("excel文件内容有问题，请检查是否有空值");
                txtShowInfo.Text += $"{ex.ToString()}\r\n";
            }
            btnImport.Enabled = true;
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            btnImport.Enabled = false;
            if (stationList.Count() == 0)
            {
                MessageBox.Show("请选择站点");
                return;
            }
            // 创建OpenFileDialog对象
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // 设置文件过滤器，支持.xlsx和.xls文件
            openFileDialog.Filter = "Excel文件 (*.xlsx)|*.xlsx";
            // 设置对话框标题
            openFileDialog.Title = "选择要导入的Excel文件";
            // 显示对话框并获取用户选择结果
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtShowInfo.Text += "正在导入excel，请耐心等待\r\n";
                Task.Run(() => {
                    ImportExcel(openFileDialog.FileName);
                });
            }
        }

        public void ExportExcel(List<Air_Model> list)
        {

            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            sheet.DefaultRowHeight = 300;
            //创建表头
            IRow rowHead = sheet.CreateRow(0);
            rowHead.CreateCell(0).SetCellValue("站点");
            sheet.SetColumnWidth(0, 256 * 25);

            rowHead.CreateCell(1).SetCellValue("时间");
            sheet.SetColumnWidth(1, 256 * 25);

            rowHead.CreateCell(2).SetCellValue("值");
            sheet.SetColumnWidth(2, 256 * 20);

            rowHead.CreateCell(3).SetCellValue("站点");
            sheet.SetColumnWidth(3, 256 * 40);

            for (int i = 0; i < list.Count; i++)
            {
                // 创建一个数据格式为文本类型的数据格式对象
                ICellStyle cellStyle = workbook.CreateCellStyle();
                IDataFormat dataFormat = workbook.CreateDataFormat();
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                IRow rowContent = sheet.CreateRow(i + 1);

                ICell cell0 = rowContent.CreateCell(0);
                cell0.CellStyle = cellStyle;
                cell0.SetCellValue(list[i].StationCode);

                ICell cell1 = rowContent.CreateCell(1);
                cell1.CellStyle = cellStyle;
                cell1.SetCellValue(list[i].TimePoint.ToString("yyyy-MM-dd HH:mm:ss"));


                ICell cell3 = rowContent.CreateCell(3);
                cell3.CellStyle = cellStyle;
                cell3.SetCellValue(stationList.Where(s=>s.StationCode== list[i].StationCode).FirstOrDefault().PositionName );
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Text files (*.r)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Filter = "xlsx (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.FileName = stationList[0].PositionName + dtpStart.Value.ToString("yyyyMMdd") + "-" + dtpEnd.Value.ToString("yyyyMMdd") + ".xlsx";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // 确保文件不存在或者用户允许覆盖
                    //if (File.Exists(filePath) &&
                    //    MessageBox.Show($"文件 {filePath} 已存在，是否覆盖？", "文件存在",
                    //        MessageBoxButtons.YesNo) != DialogResult.Yes)
                    //{
                    //    return;
                    //}
                    if (!filePath.EndsWith(".xlsx"))
                    {
                        filePath += ".xlsx";
                    }
                    using (FileStream file = new FileStream(filePath, FileMode.Create))
                    {
                        workbook.Write(file);
                    }
                    MessageBox.Show("文件保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存文件时发生错误");
                    txtShowInfo.Text += ex.Message+"\r\n";
                }
            }
        }

        private void btnStation_Click(object sender, EventArgs e)
        {

            ChooseStation newForm = new ChooseStation();
            newForm.StartPosition = FormStartPosition.CenterParent;

            if (newForm.ShowDialog() == DialogResult.OK)
            {
                // 获取Form2传递的值
                stationList = newForm.Tag as List<Station_Model>;
                listViewStation.Clear();
                // 设置ListView的视图模式为详细信息模式
                listViewStation.View = View.Details;
                listViewStation.Columns.Add("已选站点",500);
                //listViewStation.Items.Add(stationList[(0)].StationCode);
                // 添加一些示例数据到ListView
                for (int i = 0; i < stationList.Count(); i++)
                {
                    ListViewItem item = new ListViewItem(stationList[i].PositionName);
                    listViewStation.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (stationList.Count() != 1)
            {
                MessageBox.Show("请选择1个站点");
                return;
            }
            if (dtpStart.Value.Date > dtpEnd.Value.Date)
            {
                MessageBox.Show("起始日期不允许大于结束日期");
                return;
            }
            if (dtpStart.Value.Year != dtpEnd.Value.Year)
            {
                MessageBox.Show("起始日期与结束日期不允许跨年");
                return;
            }
            if (cboDataType.SelectedValue.ToString()!="1")
            {
                MessageBox.Show("本功能只可以补5分钟值");
                return;
            }
            Air_BLL air_BLL = new Air_BLL();
            Air_Model_Query query = new Air_Model_Query();
            query.TimePointStart = dtpStart.Value.ToString("yyyy-MM-dd 00:00:00");
            query.TimePointEnd = dtpEnd.Value.ToString("yyyy-MM-dd 23:59:59");
            string stationCode = stationList[0].StationCode;
            string tableName = $"Air_5m_{dtpStart.Value.Year}_{stationCode}A_Src";
            var nullDataList = air_BLL.GetAir5mNullDataList(query, tableName, stationCode);

            var machineNum = stationList[0].UniqueCode;
            AutoInsert5mData(tableName,machineNum,nullDataList);

        }


        public void AutoInsert5mData(string tableName, string machineNum, List<Air_Model> nullDataList)
        {
            Air_BLL air_BLL = new Air_BLL();
            Air_RealTime_PM25_BLL air_RealTime_PM25_BLL = new Air_RealTime_PM25_BLL();
            foreach (var item in nullDataList)
            {
                try
                {
                    var airRealTimeList = air_RealTime_PM25_BLL.GetAirRealTimePM25ListByQuery(machineNum, item.TimePoint.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss"), item.TimePoint.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (airRealTimeList.Count > 0)
                    {
                        item.Value = Math.Round(airRealTimeList.Average(s => s.Value), 0);
                        item.OperationTime = DateTime.Now;
                        air_BLL.Insert_Air(item, tableName);

                        txtShowInfo.Text += $"时间点：{item.TimePoint.ToString("yyyy-MM-dd HH:mm:ss")}，数据补入成功\r\n";
                    }
                    else
                    {
                        txtShowInfo.Text += $"时间点：{item.TimePoint.ToString("yyyy-MM-dd HH:mm:ss")}，数据补入失败\r\n";
                    }
                }
                catch (Exception ex)
                {
                    txtShowInfo.Text += $"时间点：{item.TimePoint.ToString("yyyy-MM-dd HH:mm:ss")}，数据补入出错\r\n";
                    txtShowInfo.Text += $"出错原因：{ex.ToString()}\r\n";
                    throw;
                }
            }
        }
    }
}
