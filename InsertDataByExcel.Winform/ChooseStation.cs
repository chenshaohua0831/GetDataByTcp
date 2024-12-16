using GetDataByTcp.BLL;
using GetDataByTcp.Model;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertDataByExcel.Winform
{
    public partial class ChooseStation : Form
    {
        public ChooseStation()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            CboStationInit();

            // 设置窗体的最小尺寸与当前尺寸相同，从而不允许调整窗体大小
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }
        private void CboStationInit()
        {
            dataGridView1.ReadOnly = false;
            Station_BLL station_BLL = new Station_BLL();
            var stationList = station_BLL.GetAllStationList();
            // 绑定数据源
            dataGridView1.DataSource = stationList;


            //Station_Model model = new Station_Model();
            //model.StationCode = "1";
            //model.PositionName = "2";
            //List<Station_Model> list = new List<Station_Model>();
            //list.Add(model);
            //dataGridView1.DataSource = list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var list = GetCheckedStations();
            if (list.Count==0)
            {
                MessageBox.Show("请选择站点");
            }
            else
            {
                this.Tag = list;
                this.DialogResult = DialogResult.OK;
            }
        }

        private List<Station_Model> GetCheckedStations()
        {
            List<Station_Model> checkStationList = new List<Station_Model>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["Column1"] as DataGridViewCheckBoxCell;
                if (checkBoxCell.Value!=null)
                {
                    if (checkBoxCell != null && (bool)checkBoxCell.Value)
                    {
                        Station_Model model = new Station_Model();
                        model.StationCode = row.Cells[1].Value?.ToString();
                        model.PositionName = row.Cells[2].Value?.ToString();
                        model.UniqueCode = row.Cells["UniqueCode"].Value?.ToString();
                        checkStationList.Add(model);
                    }
                }
            }
            return checkStationList;
        }
    }
}
