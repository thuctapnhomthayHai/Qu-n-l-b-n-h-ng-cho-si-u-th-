﻿using System;
using COMExcel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lý_bán_hàng_siêu_thị
{
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();
        }

        public static SqlConnection Con;  //Khai báo đối tượng kết nối        

        public static void Connect()
        {
            Con = new SqlConnection();   //Khởi tạo đối tượng
            Con.ConnectionString = @"Data Source=DESKTOP-EADVFVM;Initial Catalog=PMBanHangSieuThi;Integrated Security=True";
            Con.Open();//mở kết nối
            //kiểm tra kết nối
            if (Con.State == ConnectionState.Open)
                MessageBox.Show("Kết nối thành công");
            else MessageBox.Show("Không thể kết nối với dữ liệu");

        }

        public static void Disconnect()
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();   	//Đóng kết nối
                Con.Dispose(); 	//Giải phóng tài nguyên
                Con = null;
            }
        }

        //thực hiện câu lệnh SQL truy vấn dữ liệu từ CSDL đổ vào đối tượng bảng
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter MyData = new SqlDataAdapter(); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Tạo đối tượng thuộc lớp SqlCommand
            MyData.SelectCommand = new SqlCommand();
            MyData.SelectCommand.Connection = Con; //Kết nối cơ sở dữ liệu
            MyData.SelectCommand.CommandText = sql; //Lệnh SQL
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            MyData.Fill(table);
            return table;
        }

        //Hàm kiểm tra khoá trùng

        public static bool CheckKey(string sql)
        {
            SqlDataAdapter MyData = new SqlDataAdapter(sql, Con);
            DataTable table = new DataTable();
            MyData.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }



        //Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }

        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Con;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Dữ liệu đang được dùng, không thể xoá...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }
        DataTable KhachHang; //Lưu dữ liệu bảng KHÁCH HÀNG
       
        private void frmCustomer_Load(object sender, EventArgs e)
        {
            Connect(); //Mở kết nối
            txtma.Enabled = false;
            LoadDataGridView();
        }


        public void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaKH,TenKH,DiaChi,SDT,Email From KhachHang";
            KhachHang = GetDataToTable(sql); //lấy dữ liệu
            DataGridView.DataSource = KhachHang;

            DataGridView.Columns[0].HeaderText = "Mã khách hàng";
            DataGridView.Columns[1].HeaderText = "Tên khách hàng";

            DataGridView.Columns[2].HeaderText = "Địa chỉ";
            DataGridView.Columns[3].HeaderText = "Điện thoại";
            DataGridView.Columns[4].HeaderText = "Email";

            DataGridView.Columns[0].Width = 150;
            DataGridView.Columns[1].Width = 200;
            DataGridView.Columns[2].Width = 150;
            DataGridView.Columns[3].Width = 150;
            DataGridView.Columns[4].Width = 150;

            DataGridView.AllowUserToAddRows = false;
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtma.Focus();
                return;
            }
            if (KhachHang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtma.Text = DataGridView.CurrentRow.Cells["MaKH"].Value.ToString();
            txtten.Text = DataGridView.CurrentRow.Cells["TenKH"].Value.ToString();
            txtdc.Text = DataGridView.CurrentRow.Cells["Diachi"].Value.ToString();
            txtsdt.Text = DataGridView.CurrentRow.Cells["SDT"].Value.ToString();
            txtemail.Text = DataGridView.CurrentRow.Cells["Email"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnHuy.Enabled = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        public void ResetValue()
        {
            txtma.ResetText();
            txtten.ResetText();
            txtdc.ResetText();
            txtsdt.ResetText();
            txtemail.ResetText();
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
           
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnHuy.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValue();
            txtma.Enabled = true;
            txtma.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (KhachHang.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtma.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtten.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtten.Focus();
                return;
            }
            if (txtdc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdc.Focus();
                return;
            }
            if (txtsdt.Text == "(   )     -")
            {
                MessageBox.Show("Bạn phải nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtsdt.Focus();
                return;
            }
            if (txtemail.Text == "   ")
            {
                MessageBox.Show("Bạn phải nhập email", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtemail.Focus();
                return;
            }

            sql = "UPDATE KhachHang SET  TenKH=N'" + txtten.Text.Trim().ToString() +
                    "',DiaChi=N'" + txtdc.Text.Trim().ToString() +
                    "',SDT='" + txtsdt.Text.ToString() +
                    "',Email=N'" + txtemail.Text.Trim().ToString() +
                    "' WHERE MaKH=N'" + txtma.Text + "'";
            RunSQL(sql);
            LoadDataGridView();
            ResetValue();
            btnHuy.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (KhachHang.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtma.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE KhachHang WHERE MaKH=N'" + txtma.Text + "'";
                RunSqlDel(sql);
                LoadDataGridView();
                ResetValue();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtma.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtma.Focus();
                return;
            }
            if (txtten.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtma.Focus();
                return;
            }
            if (txtdc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdc.Focus();
                return;
            }
            if (txtsdt.Text == "(   )     -")
            {
                MessageBox.Show("Bạn phải nhập điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtsdt.Focus();
                return;
            }
            if (txtemail.Text == "  ")
            {
                MessageBox.Show("Bạn phải nhập email", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtemail.Focus();
                return;
            }



            sql = "SELECT MaKH FROM KhachHang WHERE MaKH=N'" + txtma.Text.Trim() + "'";

            if (CheckKey(sql))
            {
                MessageBox.Show("Mã khách hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtma.Focus();
                txtma.Text = "";
                return;
            }
            sql = "INSERT INTO KhachHang(MaKH,TenKH, DiaChi,SDT, Email) VALUES (N'" + txtma.Text.Trim() + "',N'" + txtten.Text.Trim() + "',N'" + txtdc.Text.Trim() + "','" + txtsdt.Text + "',N'" + txtemail.Text.Trim() + "')";
            RunSQL(sql);
            LoadDataGridView();
            ResetValue();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnHuy.Enabled = false;
            btnLuu.Enabled = false;
            txtma.Enabled = false;
        }

        private void txtma_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtten_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtdc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtsdt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtsdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtten_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !((e.KeyChar >= 65 && e.KeyChar <= 122) || e.KeyChar == 8);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {

            try
            {


                if (txtTimKiem.Text.Trim() == "")
                {
                    MessageBox.Show("Đề Nghị Bạn Nhập Từ Khóa Càn Tìm!", "Thông Báo!");
                    return;
                }
                else
                {
                    if (cbTimKiem.Text == "Mã khách hàng")
                    {
                        DataGridView.DataSource = GetDataToTable("select * from KhachHang where MaKH  = " + txtTimKiem.Text.Trim() + " ");
                    }
                    if (cbTimKiem.Text == "Tên khách hàng")
                    {
                        DataGridView.DataSource = GetDataToTable("select * from KhachHang where TenKH like '%" + txtTimKiem.Text.Trim() + "%'");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbTimKiem_Click(object sender, EventArgs e)
        {
            cbTimKiem.DisplayMember = "Text";
            cbTimKiem.ValueMember = "Value";

            cbTimKiem.Items.Add(new { Text = "Mã khách hàng", Value = "Tên khách hàng" });
        }

        private void btnIndanhsach_Click(object sender, EventArgs e)
        {
            SaveFileDialog fsave = new SaveFileDialog();

            fsave.Filter = "(Tất cả các tệp)|*.*|(các tệp excel)|*.xlsx";
            fsave.ShowDialog();

            if (fsave.FileName != "")
            {
                COMExcel.Application exApp = new COMExcel.Application();
                COMExcel.Workbook exBook;
                COMExcel.Worksheet exSheet;
                COMExcel.Range exRange;

                exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
                exSheet = exBook.Worksheets[1];
                // Định dạng chung
                exRange = exSheet.Cells[1, 1];
                exRange.Range["A1:B3"].Font.Size = 14;
                exRange.Range["A1:B3"].Font.Name = "Times new roman";
                exRange.Range["A1:B3"].Font.Bold = true;
                exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời

                exRange.Range["A1:A1"].ColumnWidth = 7;

                exRange.Range["B1:B1"].ColumnWidth = 15;

                exRange.Range["A1:B1"].MergeCells = true;
                exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A1:B1"].Value = "Quản lý bán hàng siêu thị";

                exRange.Range["A1: A100"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["D1: D100"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["F1: F100"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["G1: G100"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["H1: H100"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;

                exRange.Range["A2:B2"].MergeCells = true;
                exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A2:B2"].Value = "Khách Hàng";

                exRange.Range["A3:B3"].MergeCells = true;
                exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;

                exRange.Range["C2:E2"].Font.Size = 16;
                exRange.Range["C2:E2"].Font.Name = "Times new roman";
                exRange.Range["C2:E2"].Font.Bold = true;
                exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
                exRange.Range["C2:E2"].MergeCells = true;
                exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["C2:E2"].Value = "Danh Sách Khách Hàng";

                exRange.Range["A5"].ColumnWidth = 15;
                exRange.Range["B5"].ColumnWidth = 25;
                exRange.Range["C5"].ColumnWidth = 15;
                exRange.Range["D5"].ColumnWidth = 15;
                exRange.Range["E5"].ColumnWidth = 30;
                exRange.Range["F5"].ColumnWidth = 15;
                exRange.Range["G5"].ColumnWidth = 15;
                exRange.Range["H5"].ColumnWidth = 15;


                exRange.Range["A5:I5"].Font.Size = 14;
                exRange.Range["A5:I5"].Font.Bold = true;
                exRange.Range["A5:I5"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;

                //Lấy tên cột dữ liệu
                for (int i = 0; i < DataGridView.ColumnCount; i++)
                {
                    exSheet.Cells[5, i + 1] = DataGridView.Columns[i].HeaderText;
                }
                //Đổ dữ liệu vào sheet
                for (int i = 0; i < DataGridView.RowCount; i++)
                {
                    for (int j = 0; j < DataGridView.ColumnCount; j++)
                    {
                        exSheet.Cells[i + 6, j + 1] = DataGridView.Rows[i].Cells[j].Value;
                    }
                }

                DateTime d = DateTime.Now;
                exRange.Range["C3:E3"].Value = "Hà Nội, ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
                exRange.Range["C3:E3"].MergeCells = true;
                exRange.Range["C3:E3"].Font.Italic = true;
                exRange.Range["C3:E3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;


                exApp.Visible = true;

                exBook.SaveAs(fsave.FileName);
            }
            else
            {
                MessageBox.Show("Bạn phải nhập tên!");
            }
        }
    
    }
}
